using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SS;
using SpreadsheetGUI;

namespace SpreadsheetController
{
    public class Controller
    {
        // todo: add doc comments on everything

        private HashSet<Window> windows;
        private IView firstWindow;
        private delegate IView CreateView();

        // todo: rename
        private const string DEFAULTFILENAME = "new.ss";

        public Controller(string filename, IView firstWindow)
        {
            Spreadsheet ss = CreateOrLoadFile(filename);
            Window window = new Window(filename, firstWindow, ss);

            // todo: how do? once do, use everywhere.
            HEREHEREHERE
            CreateView = firstWindow.CreateView();

            windows = new HashSet<Window>() { window };
        }

        // todo: rename
        #region Window/File Operations

        private Window OpenNewWindow(string filename)
        {
            Spreadsheet ss = CreateOrLoadFile(filename);
            Window window = new Window(filename, windows.First().GetNewView(), ss);
            windows.Add(window);
            return window;
        }

        private void OpenNewWindow()
        {
            OpenNewWindow(DEFAULTFILENAME);
        }

        private Spreadsheet CreateOrLoadFile(string filename)
        {
            // todo: figure out how to detect whether there's something to load or not
            return CreateFile(filename);
        }

        // File > New
        private Spreadsheet CreateFile(string filename)
        {
            Spreadsheet ss = new Spreadsheet();
            ss.Save(new System.IO.StreamWriter(filename));
            return ss;
        }

        // File > Open
        private Spreadsheet LoadFile(string filename)
        {
            return new Spreadsheet
                (new System.IO.StreamReader(filename), new System.Text.RegularExpressions.Regex(""));
        }

        // File > Save && File > Save To
        private void SaveFile(string filename)
        {

        }

        private void CloseFile(string filename)
        {

        }

        #endregion

        //todo: must handle the following events:

        //event FileOperation NewFile;
        //event FileOperation OpenFile;
        //event FileOperation SaveFile;
        //event Close CloseFile;
        //event ChangeContent SetContents;
    }

    internal class Window
    {
        private string filename;
        private IView view;
        private Spreadsheet model;

        public Window(string filename, IView view, Spreadsheet model)
        {
            this.filename = filename;
            this.view = view;
            this.model = model;
        }

        public IView GetNewView()
        {
            return view.CreateView();
        }
    }
}
