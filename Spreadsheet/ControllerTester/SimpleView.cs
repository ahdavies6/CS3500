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
    /// Simulated implementation of IView, which the controller will treat the same as an actual GUI view.
    /// </summary>
    public class SimpleView : IView
    {
        public string path
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

        //Each of the events followed by a method to fire the event
        public event EventHandler NewFile;
        public void FireNewFile()
        {
            NewFile?.Invoke(this, new EventArgs());
        }

        public event OpenFileEventHandler OpenFile;
        public void FireOpenFile(string p)
        {
            OpenFile?.Invoke(this, new FileEventArgs(p));
        }

        public event SaveFileEventHandler SaveFile;
        public void FireSaveFile(string p)
        {
            SaveFile?.Invoke(this, new FileEventArgs(p));
        }

        public event EventHandler CloseFile;
        public void FireCloseFile()
        {
            CloseFile?.Invoke(this, new FormClosingEventArgs(CloseReason.ApplicationExitCall, false));
        }

        public event SetContentsEventHandler SetContents;
        public void FireSetContents(string cellName, string cellContents)
        {
            SetContents?.Invoke(this, new SetContentsEventArgs(cellName, cellContents));
        }

        public SimpleView(string p)
        {
            path = p;
        }

        public bool ClosePrompt()
        {
            return Close;
        }


        public void DisplayContents(string cellName, string cellValue)
        {
            //Do nothing
        }

        public IView GetNew()
        {
            return new SimpleView(path);
        }

        public IView GetNew(string path)
        {
            return new SimpleView(path);
        }

        public void UnableToLoad(string p)
        {
            if (ThrowException)
            {
                throw new ExceptionForTesting();
            }
        }

        public class ExceptionForTesting : Exception { }
    }
}
