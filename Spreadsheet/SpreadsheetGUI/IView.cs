using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SpreadsheetGUI
{

    /// <summary>
    /// Handles the OpenFile event, provided the object that sent the event, and FileEventArgs that contain
    /// the read path for the view.
    /// </summary>
    public delegate void OpenFileEventHandler(object sender, FileEventArgs e);

    /// <summary>
    /// Handles the SaveFile event, provided the object that sent the event, and FileEventArgs that contain
    /// the write path for the view.
    /// </summary>
    public delegate void SaveFileEventHandler(object sender, FileEventArgs e);

    /// <summary>
    /// Handles the SetContents event, provided the object that sent the event, and SetContentsEventArgs that
    /// contain the name of the cell whose contents are being set, and the contents to which it is being set.
    /// </summary>
    public delegate void SetContentsEventHandler(object sender, SetContentsEventArgs e);

    /// <summary>
    /// Interface for a View object (which will be implemented by a GUI).
    /// </summary>
    public interface IView
    {

        /// <summary>
        /// File path of the spreadsheet
        /// </summary>
        string path { get; }

        /// <summary>
        /// Returns a new instance of an IView.
        /// </summary>
        IView GetNew();

        /// <summary>
        /// Overload of GetNew() but this time passes a path that the model will be loaded from
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        IView GetNew(string path);

        /// <summary>
        /// Called when the user creates a new Spreadsheet.
        /// </summary>
        event EventHandler NewFile;

        /// <summary>
        /// Called when the user opens a Spreadsheet.
        /// </summary>
        event OpenFileEventHandler OpenFile;


        /// <summary>
        /// Called when the user saves a Spreadsheet
        /// </summary>
        event SaveFileEventHandler SaveFile;

        /// <summary>
        /// Called when the user attempts to close a Spreadsheet.
        /// </summary>
        event EventHandler CloseFile;

        /// <summary>
        /// Asks the user whether they'd like to close the view, as it hasn't been saved to the model file
        /// since it was last edited.
        /// Only called if the model has been changed since the last save.
        /// </summary>
        bool ClosePrompt();


        /// <summary>
        /// Called when the user modifies the contents of a cell in a Spreadsheet.
        /// </summary>
        event SetContentsEventHandler SetContents;

        /// <summary>
        /// Displays the contents of cell (cellName) as value (cellValue).
        /// </summary>
        void DisplayContents(string cellName, string cellValue);

        /// <summary>
        /// Creates a message dialog saying how it was not possible to open the specified file.
        /// The message will say what file could not be opened.
        /// An empty file will still be created.
        /// Returns true if the file should close otherwise false means the file should not close
        /// </summary>
        void UnableToLoad(string p);
    }



    /// <summary>
    /// EventArgs used for FileEvents. Contains the path of the file
    /// </summary>
    public class FileEventArgs : EventArgs
    {
        public string path
        {
            get;
            private set;
        }


        /// <summary>
        /// Creates a new FileEventArgs with the filename stored.
        /// </summary>
        public FileEventArgs(string filename)
        {
            path = filename;
        }
    }


    /// <summary>
    /// Derived from EventArgs; to be used in a method that instantiates SetContentsEventHandler.
    /// Contains a string CellName, which is the name of the cell to set the contents of, and
    /// a string CellContents, which are the contents to which the cell is being set.
    /// </summary>
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
}
