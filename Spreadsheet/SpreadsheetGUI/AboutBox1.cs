using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    partial class AboutBox1 : Form
    {
        public AboutBox1()
        {
            InitializeComponent();
            this.Text = String.Format("About {0}", AssemblyTitle);
            this.textBoxDescription.Text = AssemblyDescription;
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                //object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                //if (attributes.Length == 0)
                //{
                //    return "";
                //}
                //return ((AssemblyDescriptionAttribute)attributes[0]).Description;
                string[] show =
                {
                    "Help Information:",
                    "",
                    "To use the Spreadsheet, simply click a cell to begin editing. You may select other cells by clicking them, or by pressing the arrow keys to select adjacent cells.",
                    "Once the desired cell is selected, type its desired contents into the 'Current Cell:' textbox and press enter.",
                    "Cell contents may be set to strings of text, rational decimal numbers, or Formulas.",
                    "To set a cell's content to a Formula, prefix your desired Formula with a '='. Then, type out what Formula you'd like the cell to compute, including rational decimal numbers, arithemtic operators (+, -, *, or /), and desired cell names (e.g. 'A1', 'b14', 'Z51') to be operated on.",
                    "",
                    "Menu buttons:",
                    "File > New: Opens a new empty Spreadsheet in a new window.",
                    "File > Open: Opens the desired Spreadsheet in a new window.",
                    "File > Save: Saves the currently opened Spreadsheet to its current location.",
                    "File > Save As...: Saves the currently opened Spreadsheet to a new location.",
                    "File > Close: Closes the currently opened Spreadsheet. Will prompt the user to confirm a closure if the Spreadsheet has not been saved since it was last edited."
                };
                string result = "";
                foreach (string s in show)
                {
                    result = result + s + "\r\n";
                }
                return result;
            }
            private set
            {
                
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void textBoxDescription_TextChanged(object sender, EventArgs e)
        {

        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
