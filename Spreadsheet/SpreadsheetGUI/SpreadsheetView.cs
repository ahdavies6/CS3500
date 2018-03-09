using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    class SpreadsheetView : IView
    {
        /// <summary>
        /// Returns a new instance of an IView.
        /// </summary>
        public IView GetNew()
        {
            return new SpreadsheetView();
        }

        /// <summary>
        /// Called when the user creates a new Spreadsheet.
        /// </summary>
        public event EventHandler NewFile;

        /// <summary>
        /// Called when the user opens a Spreadsheet.
        /// </summary>
        public event OpenFileEventHandler OpenFile;

        /// <summary>
        /// Called when the user saves a Spreadsheet.
        /// </summary>
        public event EventHandler SaveFile;

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
        public event SetContentsEventHandler SetContents;

        /// <summary>
        /// Displays the contents of cell (cellName) as value (cellValue).
        /// </summary>
        public void DisplayContents(string cellName, string cellValue)
        {
            throw new NotImplementedException();
        }
    }
}
