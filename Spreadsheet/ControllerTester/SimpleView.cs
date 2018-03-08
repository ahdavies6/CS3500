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
        /// Called when the user saves a Spreadsheet.
        /// </summary>
        public event EventHandler SaveFile;

        ///// <summary>
        ///// Called when the user closes a Spreadsheet.
        ///// </summary>
        //public event EventHandler CloseFile;

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
