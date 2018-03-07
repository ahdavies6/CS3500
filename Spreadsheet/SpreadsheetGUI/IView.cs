using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Handler delegate used for handling Spreadsheet file creation, opening, and saving.
    /// </summary>
    public delegate void FileOperation(string filename);

    /// <summary>
    /// Handler delegate used for closing a Spreadsheet.
    /// </summary>
    public delegate void Close();

    /// <summary>
    /// Handler delegate used for changes to Spreadsheet cell contents.
    /// </summary>
    public delegate void ChangeContent(string cellName, string contents);

    /// <summary>
    /// Interface for a View object (which will be implemented by a GUI).
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Called when the user creates a new Spreadsheet.
        /// </summary>
        event FileOperation NewFile;

        /// <summary>
        /// Called when the user opens a Spreadsheet.
        /// </summary>
        event FileOperation OpenFile;

        /// <summary>
        /// Called when the user saves a Spreadsheet.
        /// </summary>
        event FileOperation SaveFile;

        /// <summary>
        /// Called when the user closes a Spreadsheet.
        /// </summary>
        event Close CloseFile;

        /// <summary>
        /// Called when the user modifies the contents of a cell in a Spreadsheet.
        /// </summary>
        event ChangeContent SetContents;
    }
}
