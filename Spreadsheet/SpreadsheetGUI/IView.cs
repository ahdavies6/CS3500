using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    // old:
    ///// <summary>
    ///// Handler delegate used for handling Spreadsheet file creation, opening, and saving.
    ///// </summary>
    //public delegate void FileOperation(string filename);

    ///// <summary>
    ///// Handler delegate used for closing a Spreadsheet.
    ///// </summary>
    //public delegate void Close();

    ///// <summary>
    ///// Handler delegate used for changes to Spreadsheet cell contents.
    ///// </summary>
    //public delegate void ChangeContent(string cellName, string contents);

    ///// <summary>
    ///// Interface for a View object (which will be implemented by a GUI).
    ///// </summary>
    //public interface IView
    //{
    //    /// <summary>
    //    /// Called when the user creates a new Spreadsheet.
    //    /// </summary>
    //    event FileOperation NewFile;

    //    /// <summary>
    //    /// Called when the user opens a Spreadsheet.
    //    /// </summary>
    //    event FileOperation OpenFile;

    //    /// <summary>
    //    /// Called when the user saves a Spreadsheet.
    //    /// </summary>
    //    event FileOperation SaveFile;

    //    /// <summary>
    //    /// Called when the user closes a Spreadsheet.
    //    /// </summary>
    //    event Close CloseFile;

    //    /// <summary>
    //    /// Called when the user modifies the contents of a cell in a Spreadsheet.
    //    /// </summary>
    //    event ChangeContent SetContents;
    //}

    public delegate void SetContentsEventHandler(object sender, SetContentsEventArgs e);
    public delegate void OpenFileEventHandler(object sender, OpenFileEventArgs e);

    /// <summary>
    /// Interface for a View object (which will be implemented by a GUI).
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Returns a new instance of an IView.
        /// </summary>
        IView GetNew();

        /// <summary>
        /// Called when the user creates a new Spreadsheet.
        /// </summary>
        event EventHandler NewFile;

        /// <summary>
        /// Called when the user opens a Spreadsheet.
        /// </summary>
        event OpenFileEventHandler OpenFile;

        /// <summary>
        /// Called when the user saves a Spreadsheet.
        /// </summary>
        event EventHandler SaveFile;

        /// <summary>
        /// Called when the user closes a Spreadsheet.
        /// </summary>
        event EventHandler CloseFile;

        /// <summary>
        /// Called when the user modifies the contents of a cell in a Spreadsheet.
        /// </summary>
        event SetContentsEventHandler SetContents;
    }

    public class SetContentsEventArgs : EventArgs
    {
        /// <summary>
        /// The cell whose contents are being set.
        /// </summary>
        public string CellName
        {
            get;
            private set;
        }
        
        /// <summary>
        /// The contents the cell is being changed to.
        /// </summary>
        public string CellContents
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a new SetContentsEventArgs regarding the cell whose contents are being set (cellName)
        /// and its new contents (cellContents).
        /// </summary>
        public SetContentsEventArgs(string cellName, string cellContents)
        {
            this.CellName = cellName;
            this.CellContents = cellContents;
        }
    }

    public class OpenFileEventArgs : EventArgs
    {
        /// <summary>
        /// The filename of the file to open.
        /// </summary>
        public string Filename
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a new OpenFileEventArgs regarding the filename of the file to open.
        /// </summary>
        public OpenFileEventArgs(string filename)
        {
            this.Filename = filename;
        }
    }
}
