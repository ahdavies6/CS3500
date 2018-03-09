using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetGUI;

namespace ControllerTester
{
    /// <summary>
    /// Simulated implementation of IView, which the controller will treat the same as an actual GUI view.
    /// </summary>
    public class SimpleView : IView
    {
        /// <summary>
        /// Returns a new instance of SimpleView as an IView.
        /// </summary>
        public IView GetNew()
        {
            return new SimpleView();
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
        /// Called when the user saves a Spreadsheet to its current working filepath.
        /// </summary>
        public event EventHandler SaveFile;

        /// <summary>
        /// Called when the user saves a Spreadsheet to a new filepath.
        /// </summary>
        public event SaveFileAsEventHandler SaveFileAs;

        /// <summary>
        /// Called when the user attempts to close a Spreadsheet.
        /// </summary>
        public event EventHandler CloseFile;

        /// <summary>
        /// Asks the user whether they'd like to close the view, as it hasn't been saved to the model file
        /// since it was last edited.
        /// Only called if the model has been changed since the last save.
        /// </summary>
        public bool ClosePrompt()
        {
            // todo: change this for future test
            return true;
        }

        /// <summary>
        /// Actually closes the view.
        /// </summary>
        public void CloseView()
        {
            // todo: change this for future test
        }

        /// <summary>
        /// Called when the user modifies the contents of a cell in a Spreadsheet.
        /// </summary>
        public event SetContentsEventHandler SetContents;

        /// <summary>
        /// Displays the contents of cell (cellName) as value (cellValue).
        /// </summary>
        public void DisplayContents(string cellName, string cellValue)
        {

        }
    }
}
