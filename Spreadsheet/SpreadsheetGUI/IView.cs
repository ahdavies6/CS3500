using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Handles the OpenFile event, provided the object that sent the event, and OpenFileEventArgs that contain
    /// the read destination for the view.
    /// </summary>
    public delegate void OpenFileEventHandler(object sender, OpenFileEventArgs e);

    /// <summary>
    /// Handles the OpenFile event, provided the object that sent the event, and OpenFileEventArgs that contain
    /// the write destination for the view.
    /// </summary>
    public delegate void SaveFileEventHandler(object sender, SaveFileEventArgs e);

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
        event SaveFileEventHandler SaveFile;

        // todo: decide whether to keep this?
        ///// <summary>
        ///// Called when the user attempts to close a Spreadsheet.
        ///// </summary>
        //event EventHandler CloseFile;

        ///// <summary>
        ///// Asks the user whether they'd like to close the view, as it hasn't been saved to the model file
        ///// since it was last edited.
        ///// Only called if the model has been changed since the last save.
        ///// </summary>
        //bool ClosePrompt();

        ///// <summary>
        ///// Actually closes the view.
        ///// </summary>
        //void CloseView();

        /// <summary>
        /// Called when the user modifies the contents of a cell in a Spreadsheet.
        /// </summary>
        event SetContentsEventHandler SetContents;

        /// <summary>
        /// Displays the contents of cell (cellName) as value (cellValue).
        /// </summary>
        void DisplayContents(string cellName, string cellValue);
    }

    /// <summary>
    /// Derived from EventArgs; to be used in a method that instantiates OpenFileEventHandler.
    /// Contains a TextReader Input, which is the source from which to read, and a TextWriter
    /// Output, which is the source being written to.
    /// </summary>
    public class OpenFileEventArgs : EventArgs
    {
        /// <summary>
        /// The input source to read the Spreadsheet from.
        /// </summary>
        public TextReader Input
        {
            get;
            private set;
        }

        /// <summary>
        /// The output source being written to.
        /// </summary>
        public TextWriter Output
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a new OpenFileEventArgs regarding the source of input being read from and
        /// the destination being written to.
        /// </summary>
        public OpenFileEventArgs(TextReader input, TextWriter output)
        {
            this.Input = input;
            this.Output = output;
        }

        /// <summary>
        /// Creates a new OpenFileEventArgs reading from and writing to file (filename).
        /// </summary>
        public OpenFileEventArgs(string filename) :
            this(new StreamReader(filename), new StreamWriter(filename))
        {
            // simply calls the previous constructor, with input and output as the file (filename)
        }
    }

    /// <summary>
    /// Derived from EventArgs; to be used in a method that instantiates SaveFileEventHandler.
    /// Contains a TextWriter Output, which is the source being written to.
    /// </summary>
    public class SaveFileEventArgs : EventArgs
    {
        /// <summary>
        /// The output source being written to.
        /// </summary>
        public TextWriter Output
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a new SaveFileEventArgs regarding the output source being written to.
        /// </summary>
        public SaveFileEventArgs(TextWriter output)
        {
            this.Output = output;
        }

        /// <summary>
        /// Creates a new SaveFileEventArgs regarding the output source being written to (file (filename)).
        /// </summary>
        public SaveFileEventArgs(string filename) : this(new StreamWriter(filename))
        {
            // simply calls the previous constructor, with output as the file (filename)
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
