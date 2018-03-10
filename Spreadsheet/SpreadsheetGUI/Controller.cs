using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SS;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Formulas;

namespace SpreadsheetGUI
{
    /// <summary>
    /// A controller manipulates its model (Spreadsheet) according to user actions in its view (IView, which will
    /// be the GUI). Allows interaction between the model and the view without any direct references.
    /// </summary>
    public class Controller
    {
        /// <summary>
        /// Context of the application (open models/views).
        /// </summary>
        public SpreadsheetApplicationContext Context;

        /// <summary>
        /// View (GUI) for the model (Spreadsheet).
        /// </summary>
        private IView view;

        /// <summary>
        /// Model (Spreadsheet) for the view (GUI).
        /// </summary>
        private AbstractSpreadsheet ss;

        /// <summary>
        /// Creates a new Controller controlling IView (GUI) _view which is editing a model (Spreadsheet)
        /// </summary>
        public Controller(IView view)
        {
            this.view = view;

            if (!this.view.Path.Equals(""))
            {
                ss = CreateOrLoadFile(this.view.Path);

                foreach (string cell in ss.GetNamesOfAllNonemptyCells())
                {
                    this.view.DisplayContents(cell, ss.GetCellValue(cell).ToString());
                }
            }
            else
            {
                ss = new Spreadsheet(new Regex("^[A-Z]*[1-99]*$"));
            }

            this.view.NewFile += HandleNew;
            this.view.OpenFile += HandleOpen;
            this.view.SaveFile += HandleSave;
            this.view.SetContents += HandleChange;
            this.view.CloseFile += HandleClose;
        }

        /// <summary>
        /// If the source file at path exists, reads that source into a Spreadsheet and returns it.
        /// Else, calls view.UnableToLoad, which will inform the user that the Spreadsheet could not
        /// be opened, and opens a new Spreadsheet instead.
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
            this.Context.RunNew(e.Path);
        }

        /// <summary>
        /// Handles a view's Save event.
        /// </summary>
        private void HandleSave(object sender, FileEventArgs e)
        {
            using (TextWriter t = new StreamWriter(e.Path))
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
            catch (FormulaFormatException)
            {
                view.DisplayContents(cellName, "Bad Formula");
                return;
            }

            //Case that the celLContents is just an emptry string. In which case, ISet cells will be empty and 
            // the cell value needs to be manually updated in the gui
            //If ISet is not empty, then it must contain cellName since it is one of the cells that must get updated
            //Thus it will override this operation anyway - this means theres no need for an if statement
            view.DisplayContents(cellName, cellContents);

            // Only reaches this point if adding the new value was succesful, and did not cause
            // a circular dependency
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
        /// Handles the closing of the spreadsheet.
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
