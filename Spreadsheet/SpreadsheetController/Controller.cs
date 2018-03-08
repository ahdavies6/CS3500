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
        /// in file (filename).
        /// </summary>
        public Controller(string filename, IView firstView)
        {
            Spreadsheet ss = CreateOrLoadFile(filename);
            Window window = new Window(filename, firstView, ss);
            getNewView = firstView.GetNew;

            windows = new HashSet<Window> { window };
        }

        /// <summary>
        /// Returns the window which is editing file (filename).
        /// If no such file is open, throws a KeyNotFoundException.
        /// </summary>
        private Window GetWindow(string filename)
        {
            foreach (Window window in windows)
            {
                if (window.Filename == filename)
                {
                    return window;
                }
            }

            throw new KeyNotFoundException("There is no file with that filename being edited.");
        }

        /// <summary>
        /// Returns the window which is editing file (filename).
        /// If no such file is open, throws a KeyNotFoundException.
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
        /// Opens a new window editing file (filename).
        /// </summary>
        private void OpenNewWindow(string filename)
        {
            IView view = getNewView();
            view.NewFile += HandleNew;
            view.OpenFile += HandleOpen;
            view.SaveFile += HandleSave;
            view.SetContents += HandleChange;

            Spreadsheet ss = CreateOrLoadFile(filename);

            Window window = new Window(filename, view, ss);
            windows.Add(window);
        }

        /// <summary>
        /// Opens a new window editing file DEFAULTFILENAME ("new.ss").
        /// </summary>
        private void OpenNewWindow()
        {
            OpenNewWindow(DEFAULTFILENAME);
        }

        /// <summary>
        /// If the file (filename) exists, reads that file into a Spreadsheet and returns it.
        /// Otherwise, creates a new file (filename) and returns it as a Spreadsheet.
        /// </summary>
        private Spreadsheet CreateOrLoadFile(string filename)
        {
            if (File.Exists(filename))
            {
                return LoadFile(filename);
            }
            else
            {
                return CreateFile(filename);
            }
        }

        /// <summary>
        /// Creates a new file (filename) and returns it as a Spreadsheet.
        /// Accessed in the GUI via File > New.
        /// </summary>
        private Spreadsheet CreateFile(string filename)
        {
            Spreadsheet ss = new Spreadsheet();
            ss.Save(new StreamWriter(filename));
            return ss;
        }

        /// <summary>
        /// Loads a file (filename), reads that file into a Spreadsheet, and returns it.
        /// Accessed in the GUI via File > Open.
        /// </summary>
        private Spreadsheet LoadFile(string filename)
        {
            return new Spreadsheet(new StreamReader(filename), new Regex("^[A-Z][1-99]$"));
        }

        /// <summary>
        /// Saves an open file at destination (filename).
        /// Accessed in the GUI via File > Save or File > Save To.
        /// </summary>
        private void SaveFile(string filename)
        {
            GetWindow(filename).Model.Save(new StreamWriter(filename));
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
            OpenNewWindow(e.Filename);
        }

        /// <summary>
        /// Handles a view's SaveFile event.
        /// </summary>
        private void HandleSave(object sender, EventArgs e)
        {
            SaveFile(GetWindow((IView)sender).Filename);
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
    /// Each window is editing file Filename in view View containing model Model.
    /// </summary>
    internal class Window
    {
        /// <summary>
        /// The filename of the open file.
        /// </summary>
        public string Filename
        {
            get;
            private set;
        }
        
        /// <summary>
        /// The view (GUI) that the user is editing the model through.
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
        /// Creates a new Window editing file (filename) in view (view) containing model (model).
        /// </summary>
        public Window(string filename, IView view, Spreadsheet model)
        {
            this.Filename = filename;
            this.View = view;
            this.Model = model;
        }
    }
}
