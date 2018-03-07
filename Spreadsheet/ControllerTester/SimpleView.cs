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
        /// The name of the file being accessed in events.
        /// </summary>
        readonly string filename;

        /// <summary>
        /// Initializes SimpleView by storing filename (for later use in events).
        /// </summary>
        public SimpleView(string filename)
        {
            this.filename = filename;
        }

        /// <summary>
        /// Called when the user creates a new Spreadsheet.
        /// </summary>
        public event FileOperation NewFile;

        /// <summary>
        /// Called when the user opens a Spreadsheet.
        /// </summary>
        public event FileOperation OpenFile;

        /// <summary>
        /// Called when the user saves a Spreadsheet.
        /// </summary>
        public event FileOperation SaveFile;

        /// <summary>
        /// Called when the user closes a Spreadsheet.
        /// </summary>
        public event Close CloseFile;

        /// <summary>
        /// Called when the user modifies the contents of a cell in a Spreadsheet.
        /// </summary>
        public event ChangeContent SetContents;
    }
}
