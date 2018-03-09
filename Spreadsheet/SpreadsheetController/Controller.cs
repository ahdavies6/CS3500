using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SS;
using SpreadsheetGUI;
using System.IO;
using System.Text.RegularExpressions;

namespace SpreadsheetController
{
    public delegate IView GetNewView();

    public class Controller
    {
        /// <summary>
        /// The windows that are open in the program.
        /// </summary>
        private HashSet<Window> windows;

        /// <summary>
        /// Delegate that returns a new instance of an IView.
        /// </summary>
        GetNewView getNewView;

        /// <summary>
        /// The default filename to open/edit.
        /// </summary>
        private const string DEFAULTFILENAME = "new.ss";

        /// <summary>
        /// Creates a new Controller controlling IView (GUI) firstView which is editing a model (Spreadsheet)
        /// at source (input).
        /// </summary>
        public Controller(TextReader input, IView firstView)
        {
            Spreadsheet ss = CreateOrLoadFile(input);
            Window window = new Window(input, firstView, ss);
            getNewView = firstView.GetNew;

            windows = new HashSet<Window> { window };
        }

        /// <summary>
        /// Returns the window which is editing source (input).
        /// If no such source is open, throws a KeyNotFoundException.
        /// </summary>
        private Window GetWindow(TextReader input)
        {
            foreach (Window window in windows)
            {
                if (window.Input == input)
                {
                    return window;
                }
            }

            throw new KeyNotFoundException("There is no source with that signature being edited.");
        }

        /// <summary>
        /// Returns the window which is editing source contained in (view).
        /// If no such source is open, throws a KeyNotFoundException.
        /// </summary>
        private Window GetWindow(IView view)
        {
            foreach (Window window in windows)
            {
                if (window.View == view)
                {
                    return window;
                }
            }

            throw new KeyNotFoundException("There is no window with that view being edited.");
        }

        #region Window Operations

        /// <summary>
        /// Opens a new window editing source (input).
        /// </summary>
        private void OpenNewWindow(TextReader input)
        {
            IView view = getNewView();
            view.NewFile += HandleNew;
            view.OpenFile += HandleOpen;
            view.SaveFile += HandleSave;
            view.SetContents += HandleChange;

            Spreadsheet ss = CreateOrLoadFile(input);

            Window window = new Window(input, view, ss);
            windows.Add(window);
        }

        /// <summary>
        /// Opens a new window editing source at DEFAULTFILENAME ("new.ss").
        /// </summary>
        private void OpenNewWindow()
        {
            OpenNewWindow(new StreamReader(DEFAULTFILENAME));
        }

        /// <summary>
        /// If the source file at (input) exists, reads that source into a Spreadsheet and returns it.
        /// Otherwise, creates a new source (input) and returns it as a Spreadsheet.
        /// </summary>
        private Spreadsheet CreateOrLoadFile(TextReader input)
        {
            // todo: remove deprecated
            //if (File.Exists(input.ToString()))
            //{
            //    return LoadFile(input);
            //}
            try
            {
                return LoadFile(input);
            }
            catch //whichever exception this throws
            {
                return CreateFile(input);
            }
        }

        /// <summary>
        /// Creates a new file at the filename in source input and returns it as a Spreadsheet.
        /// Accessed in the GUI via File > New.
        /// </summary>
        private Spreadsheet CreateFile(TextReader input)
        {
            Spreadsheet ss = new Spreadsheet();
            ss.Save(new StreamWriter(input.ToString()));
            return ss;
        }

        /// <summary>
        /// Loads a source (input), reads that source into a Spreadsheet, and returns it.
        /// Accessed in the GUI via File > Open.
        /// </summary>
        private Spreadsheet LoadFile(TextReader input)
        {
            return new Spreadsheet(input, new Regex("^[A-Z][1-99]$"));
        }

        /// <summary>
        /// Saves an open source at destination source's filename.
        /// Accessed in the GUI via File > Save or File > Save To.
        /// </summary>
        private void SaveFile(TextWriter output)
        {
            GetWindow(new StreamReader(output.ToString())).Model.Save(new StreamWriter(output.ToString()));
        }

        ///// <summary>
        ///// Closes whichever window is editing file (filename).
        ///// Accessed in the GUI via File > Close.
        ///// </summary>
        //private void CloseFile(string filename)
        //{
        //    Window window = GetWindow(filename);
        //}

        /// <summary>
        /// Sets the contents of a cell (cellName) in model Spreadsheet to contents (cellContents).
        /// </summary>
        private void SetCellContents(Window window, string cellName, string cellContents)
        {
            HashSet<string> cells = (HashSet<string>)window.Model.SetContentsOfCell(cellName, cellContents);

            foreach (string cell in cells)
            {
                object value = window.Model.GetCellValue(cell);

                if (value is string)
                {
                    window.View.DisplayContents(cell, (string)value);
                }
                else if (value is double)
                {
                    value = (double)value;
                    window.View.DisplayContents(cell, value.ToString());
                }
                else if (value is FormulaError)
                {
                    value = (FormulaError)value;
                    window.View.DisplayContents(cell, value.ToString());
                }
            }
        }

        #endregion

        #region IView EventHandlers

        /// <summary>
        /// Handles a view's NewFile event.
        /// </summary>
        private void HandleNew(object sender, EventArgs e)
        {
            OpenNewWindow();
        }
        
        /// <summary>
        /// Handles a view's OpenFile event.
        /// </summary>
        private void HandleOpen(object sender, OpenFileEventArgs e)
        {
            OpenNewWindow(e.Input);
        }

        /// <summary>
        /// Handles a view's SaveFile event.
        /// </summary>
        private void HandleSave(object sender, SaveFileEventArgs e)
        {
            SaveFile(e.Output);
        }

        ///// <summary>
        ///// Handles a view's CloseFile event.
        ///// </summary>
        //private void HandleClose(object sender, EventArgs e)
        //{
        //    CloseFile(GetWindow((IView)sender).Filename);
        //}

        /// <summary>
        /// Handles a view's SetContents event.
        /// </summary>
        private void HandleChange(object sender, SetContentsEventArgs e)
        {
            SetCellContents(GetWindow((IView)sender), e.CellName, e.CellContents);
        }

        #endregion
    }

    /// <summary>
    /// A window that is open in the program.
    /// Each window is editing source Input in view View containing model Model.
    /// </summary>
    internal class Window
    {
        /// <summary>
        /// The source of the open Spreadsheet.
        /// </summary>
        public TextReader Input
        {
            get;
            private set;
        }
        
        /// <summary>
        /// The view (GUI) that the user is editing the model (Spreadsheet) through.
        /// </summary>
        public IView View
        {
            get;
            private set;
        }

        /// <summary>
        /// The model (Spreadsheet) that the user is indirectly editing through the view.
        /// </summary>
        public Spreadsheet Model
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a new Window editing source (input) in view (view) containing model (model).
        /// </summary>
        public Window(TextReader input, IView view, Spreadsheet model)
        {
            this.Input = input;
            this.View = view;
            this.Model = model;
        }
    }
}
