using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Copied and modified from Joe Zachary's FileAnalysisApplicationContext.cs from the class examples
    /// Keeps track of how many top level spreadsheets are running
    /// </summary>
    public class SpreadsheetApplicationContext : ApplicationContext
    {

        // Number of open forms
        private int windowCount = 0;

        // Singleton ApplicationContext
        private static SpreadsheetApplicationContext context;

        /// <summary>
        /// Private constructor for singleton pattern
        /// </summary>
        private SpreadsheetApplicationContext()
        {
        }

        /// <summary>
        /// Returns the one DemoApplicationContext.
        /// </summary>
        public static SpreadsheetApplicationContext GetContext()
        {
            if (context == null)
            {
                context = new SpreadsheetApplicationContext();
            }
            return context;
        }

        /// <summary>
        /// Runs a form in this application context. Path is used to set the path to the spreadsheet file. 
        /// An empty string is passed in to represent a new spreadsheet.
        /// </summary>
        public void RunNew(string path)
        {
            // Create the window and the controller
            Form1 window = new Form1(path);
            (new Controller(window)).Context = SpreadsheetApplicationContext.GetContext();

            // One more form is running
            windowCount++;

            // When this form closes, we want to find out
            window.FormClosed += (o, e) => { if (--windowCount <= 0) ExitThread(); };

            // Run the form
            window.Show();
        }
    }
}
