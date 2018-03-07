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
        event EventHandler NewFile;

        /// <summary>
        /// Called when the user opens a Spreadsheet.
        /// </summary>
        event EventHandler OpenFile;

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
        event EventHandler SetContents;
    }
}
