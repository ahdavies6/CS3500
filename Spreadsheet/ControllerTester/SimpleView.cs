using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpreadsheetGUI;

namespace ControllerTester
{
    /// <summary>
    /// Simulated implementation of IView, which the controller will treat the same as an actual GUI view,
    /// as the controller can only access Views through IView.
    /// </summary>
    public class SimpleView : IView
    {
        public string Path
        {
            get; set;
        }

        /// <summary>
        /// Helper property that determines what CLosePrompt returns
        /// </summary>
        public bool Close
        {
            get; set;
        }

        /// <summary>
        /// Property that determines whether UnableToLoad throws an exception
        /// </summary>
        public bool ThrowException
        {
            get; set;
        }

        /// <summary>
        /// Fired when the user opens a new window.
        /// </summary>
        public event EventHandler NewFile;

        /// <summary>
        /// Fires NewFile event.
        /// </summary>
        public void FireNewFile()
        {
            NewFile?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Fired when the user opens a file.
        /// </summary>
        public event OpenFileEventHandler OpenFile;

        /// <summary>
        /// Fires OpenFile event.
        /// </summary>
        public void FireOpenFile(string p)
        {
            OpenFile?.Invoke(this, new FileEventArgs(p));
        }

        /// <summary>
        /// Fired when the user saves a file.
        /// </summary>
        public event SaveFileEventHandler SaveFile;

        /// <summary>
        /// Fires SaveFile event.
        /// </summary>
        public void FireSaveFile(string p)
        {
            SaveFile?.Invoke(this, new FileEventArgs(p));
        }

        /// <summary>
        /// Fired when the user closes a file.
        /// </summary>
        public event EventHandler CloseFile;

        /// <summary>
        /// Fires CloseFile event.
        /// </summary>
        public void FireCloseFile()
        {
            CloseFile?.Invoke(this, new FormClosingEventArgs(CloseReason.ApplicationExitCall, false));
        }

        /// <summary>
        /// Fired when the user sets the contents of a cell in the Spreadsheet.
        /// </summary>
        public event SetContentsEventHandler SetContents;

        /// <summary>
        /// Fires SetContents event.
        /// </summary>
        public void FireSetContents(string cellName, string cellContents)
        {
            SetContents?.Invoke(this, new SetContentsEventArgs(cellName, cellContents));
        }

        /// <summary>
        /// Creates a new SimpleView with (file)Path (p).
        /// </summary>
        public SimpleView(string p)
        {
            Path = p;
        }

        /// <summary>
        /// Asks the user if they actually want to close the window.
        /// </summary>
        public bool ClosePrompt()
        {
            return Close;
        }

        /// <summary>
        /// Included to fulfill IView spec; will be implemented in the actual GUI, but needn't do
        /// anything in SimpleView.
        /// </summary>
        public void DisplayContents(string cellName, string cellValue)
        {
            //Do nothing
        }

        /// <summary>
        /// Returns a new instance of SimpleView at the current filepath.
        /// </summary>
        public IView GetNew()
        {
            return new SimpleView(Path);
        }

        /// <summary>
        /// Returns a newe instance of SimpleView at filepath (path).
        /// </summary>
        public IView GetNew(string path)
        {
            return new SimpleView(path);
        }

        /// <summary>
        /// Throws exception when a file could not be loaded.
        /// </summary>
        public void UnableToLoad(string p)
        {
            if (ThrowException)
            {
                throw new ExceptionForTesting();
            }
        }

        /// <summary>
        /// Included for testing purposes; has no actual function.
        /// </summary>
        public class ExceptionForTesting : Exception { }
    }
}
