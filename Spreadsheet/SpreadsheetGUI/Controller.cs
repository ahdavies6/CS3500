using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SS;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    public delegate IView GetNewView();

    public class Controller
    {

        /// <summary>
        /// Context of the application
        /// </summary>
        public SpreadsheetApplicationContext Context;

        /// <summary>
        /// The gui of the spreadsheet
        /// </summary>
        private IView view;

        /// <summary>
        /// The model of the spreadsheet
        /// </summary>
        private AbstractSpreadsheet ss;

        /// <summary>
        /// Creates a new Controller controlling IView (GUI) _view which is editing a model (Spreadsheet)
        /// </summary>
        public Controller(IView _view)
        {
            view = _view;
            if (!view.path.Equals(""))
            {
                ss = CreateOrLoadFile(view.path);
                foreach (string cell in ss.GetNamesOfAllNonemptyCells())
                {
                    view.DisplayContents(cell, ss.GetCellValue(cell).ToString());
                }
            }
            else
            {
                ss = new Spreadsheet(new Regex("^[A-Z]*[1-99]*$"));
            }
            view.NewFile += HandleNew;
            view.OpenFile += HandleOpen;
            view.SaveFile += HandleSave;
            view.SetContents += HandleChange;
            view.CloseFile += HandleClose;
        }


        /// <summary>
        /// If the source file at path exists, reads that source into a Spreadsheet and returns it.
        /// </summary>
        private Spreadsheet CreateOrLoadFile(string path)
        {
            try
            {
                using (TextReader t = new StreamReader(path))
                {
                    return new Spreadsheet(t, new Regex("^[A-Z][1-99]$"));
                }
            }
            catch (System.Xml.XmlException)
            {
                view.UnableToLoad(path);
                return new Spreadsheet(new Regex("^[A-Z][1-99]$"));
            }
        }


        /// <summary>
        /// Handles a view's NewFile event.
        /// </summary>
        private void HandleNew(object sender, EventArgs e)
        {
            Context.RunNew("");
        }

        /// <summary>
        /// Handles a view's OpenFile event.
        /// </summary>
        private void HandleOpen(object sender, FileEventArgs e)
        {
            this.Context.RunNew(e.path);
        }

        /// <summary>
        /// Handles a view's Save event.
        /// </summary>
        private void HandleSave(object sender, FileEventArgs e)
        {
            using (TextWriter t = new StreamWriter(e.path))
            {
                ss.Save(t);
            }
        }

        /// <summary>
        /// Handles a view's SetContents event.
        /// </summary>
        private void HandleChange(object sender, SetContentsEventArgs e)
        {
            SetCellContents(e.CellName, e.CellContents);
        }

        /// <summary>
        /// Sets the contents of a cell (cellName) in model Spreadsheet to contents (cellContents).
        /// </summary>
        private void SetCellContents(string cellName, string cellContents)
        {
            ISet<string> cells;

            try
            {
                cells = ss.SetContentsOfCell(cellName, cellContents);
            }
            catch (SS.CircularException)
            {
                view.DisplayContents(cellName, "Circular Formula");
                return;
            }

            //If adding the new value was succesful without a circular dependency
            foreach (string cell in cells)
            {
                object value = ss.GetCellValue(cell);

                if (value is string)
                {
                    view.DisplayContents(cell, (string)value);
                }
                else if (value is double)
                {
                    value = (double)value;
                    view.DisplayContents(cell, value.ToString());
                }
                else if (value is FormulaError)
                {
                    value = (FormulaError)value;
                    view.DisplayContents(cell, "Bad Formula");
                }
            }
        }

        /// <summary>
        /// Handles the closing of the spreadsheet
        /// </summary>
        private void HandleClose(object sender, EventArgs e)
        {
            if (ss.Changed)
            {
                if (!view.ClosePrompt())
                {
                    ((FormClosingEventArgs)e).Cancel = true;
                }
            }
        }
    }

}
