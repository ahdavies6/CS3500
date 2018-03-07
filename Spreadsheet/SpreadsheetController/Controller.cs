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

        // todo: rename
        private const string DEFAULTFILENAME = "new.ss";

        public Controller(IView firstWindow)
        {
            windows = new HashSet<Window>();
            this.firstWindow = firstWindow;
        }

        // todo: rename
        #region Window/File Operations

        public void OpenNewWindow(string filename)
        {
            // todo: edit so we know whether to CreateFile or LoadFile
            Spreadsheet ss = CreateFile(filename);
            Window window = new Window(filename, firstWindow.CreateView(), ss);
            windows.Add(window);
        }

        public void OpenNewWindow()
        {
            OpenNewWindow(DEFAULTFILENAME);
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
        public string Filename { get; private set; }
        public IView View { get; private set; }
        public Spreadsheet Spreadsheet { get; private set; }

        public Window(string filename, IView view, Spreadsheet spreadsheet)
        {
            this.Filename = filename;
            this.View = view;
            this.Spreadsheet = spreadsheet;
        }
    }
}
