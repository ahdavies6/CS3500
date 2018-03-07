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
