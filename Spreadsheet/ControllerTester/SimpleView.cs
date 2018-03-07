using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetGUI;

namespace ControllerTester
{
    public class SimpleView : IView
    {
        /// <summary>
        /// Returns a new instance of SimpleView as an IView.
        /// </summary>
        public IView CreateView()
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
        public event EventHandler OpenFile;

        /// <summary>
        /// Called when the user saves a Spreadsheet.
        /// </summary>
        public event EventHandler SaveFile;

        /// <summary>
        /// Called when the user closes a Spreadsheet.
        /// </summary>
        public event EventHandler CloseFile;

        /// <summary>
        /// Called when the user modifies the contents of a cell in a Spreadsheet.
        /// </summary>
        public event EventHandler SetContents;
    }
}
