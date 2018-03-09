using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    public partial class Form1 : Form, IView
    {
        /// <summary>
        /// File path of the spreadsheet
        /// </summary>
        public string path
        {
            get;
            private set;
        }

        /// <summary>
        /// Single Arg constructor that makes the form and sets the path to the passed path
        /// </summary>
        /// <param name="path"></param>
        public Form1(string path)
        {
            InitializeComponent();
            this.path = path;

        }

        /// <summary>
        /// Zero arg constructor that does the same thing as the one arg but passes an emptry string instead
        /// </summary>
        public Form1(): this("")
        {

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
        /// Called when the user saves a Spreadsheet to its current working filepath.
        /// </summary>
        public event SaveFileEventHandler SaveFile;

        /// <summary>
        /// Called when the user attempts to close a Spreadsheet.
        /// </summary>
        public event EventHandler CloseFile;

        /// <summary>
        /// Called when the user modifies the contents of a cell in a Spreadsheet.
        /// </summary>
        public event SetContentsEventHandler SetContents;

        /// <summary>
        /// Returns a new instance of an IView.
        /// </summary>
        public IView GetNew()
        {
            Form1 newss = new Form1("");
            return newss;
        }

        /// <summary>
        /// Overload of GetNew() but this time passes a path that the model will be loaded from
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IView GetNew(string p)
        {
            Form1 newss = new Form1(p);
            return newss;
        }

        /// <summary>
        /// Displays the contents of cell (cellName) as value (cellValue).
        /// </summary>
        public void DisplayContents(string cellName, string cellValue)
        {
            int col = ConvertColToInt(cellName[0]);
            int row = int.Parse(cellName.Substring(1));
            row--;
            this.spreadsheetPanel1.SetValue(col, row, cellValue);
        }

        /// <summary>
        /// Creates a message dialog saying how it was not possible to open the specified file.
        /// The message will say what file could not be opened.
        /// An empty file will still be created.
        /// Returns true if the file should close otherwise false means the file should not close
        /// </summary>
        public bool ClosePrompt()
        {
            DialogResult result = MessageBox.Show("There are unsaved changes! Do you want to save or cancel?", "Warning!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.SaveSpreadSheet(this, new EventArgs());
                return true;
            }
            else if (result == DialogResult.No)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Creates a message dialog saying how it was not possible to open the specified file.
        /// The message will say what file could not be opened.
        /// An empty file will still be created.
        /// </summary>
        public void UnableToLoad(string p)
        {
            MessageBox.Show("The file " + p + "Could not be opened.");
        }

        /// <summary>
        /// Helper method that converts the given char col into an int representing the column
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        private int ConvertColToInt(char col)
        {
            return ((int)col) - 65;
        }

        /// <summary>
        /// Grabs the current cell name, value, and content. When the user clicks a cell this is updated
        /// </summary>
        /// <param name="ss"></param>
        private void SelectCell(SSGui.SpreadsheetPanel ss)
        {
            int col;
            int row;
            string val;

            ss.GetSelection(out col, out row);
            ss.GetValue(col, row, out val);

            if (val.Equals(""))
            {
                val = "Currently Empty";
            }
            this.CurrentCellValue.Text = "Current Value: " + val;
            this.CurrentCellName.Text = "Current Cell: " + ConvertColIntoLetter(col) + row.ToString();
        }

        /// <summary>
        /// Given a col number, the number is converted into a letter
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        private string ConvertColIntoLetter(int col)
        {
            return "" + (char)(col + 65);
        }

        /// <summary>
        /// Deals with the using of arrow keys.
        /// If left arrow, right arrow, up arrow, or down arrow are used, the highlighted cell is accordingly changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyboardInput(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Up)
            {
                int col, row;
                this.spreadsheetPanel1.GetSelection(out col, out row);
                if (row != 0)
                {
                    row--;
                }
                this.spreadsheetPanel1.SetSelection(col, row);
            }
            else if (e.KeyCode == Keys.Down)
            {
                int col, row;
                this.spreadsheetPanel1.GetSelection(out col, out row);
                if (row != 98)
                {
                    row++;
                }
                this.spreadsheetPanel1.SetSelection(col, row);
            }
            else if (e.KeyCode == Keys.Left)
            {
                int col, row;
                this.spreadsheetPanel1.GetSelection(out col, out row);
                if (col != 0)
                {
                    col--;
                }
                this.spreadsheetPanel1.SetSelection(col, row);
            }
            else if (e.KeyCode == Keys.Right)
            {
                int col, row;
                this.spreadsheetPanel1.GetSelection(out col, out row);
                if (col != 25)
                {
                    col++;
                }
                this.spreadsheetPanel1.SetSelection(col, row);
            }


            this.SelectCell(this.spreadsheetPanel1);
        }

        /// <summary>
        /// Opens a dialog and calls the OpenFile dialog prompt using the file name choosen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFilePicker(object sender, EventArgs e)
        {
            FileDialog open = new OpenFileDialog();
            open.InitialDirectory = "c:\\";
            open.Filter = "Spreadsheet files (*.SS) |*.SS| All files (*.*)|*.*";

            if (open.ShowDialog() == DialogResult.OK)
            {

                if (OpenFile != null)
                {
                    OpenFile(this, new FileEventArgs(open.FileName));
                }

            }
        }

        /// <summary>
        /// Creates a new spreadsheet by firing the NewFile event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewSpreadhSheetButtonClick(object sender, EventArgs e)
        {
            if (NewFile != null)
            {
                NewFile(this, e);
            }
        }

        /// <summary>
        /// When pressed enter, the cells content and corresponding value are updated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellContentUpdate(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (SetContents != null)
                {
                    int row, col;
                    this.spreadsheetPanel1.GetSelection(out col, out row);
                    row++;
                    string cellName = ConvertColIntoLetter(col) + row;
                    SetContents(this, new SetContentsEventArgs(cellName, this.ContentChangeBox.Text));
                }
            }
        }

        /// <summary>
        /// Saves the spreadsheet using the current file path. If it does not exist, prompts to user to select a place to save.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveSpreadSheet(object sender, EventArgs e)
        {
            if (!path.Equals(""))
            {
                this.SaveFile(this, new FileEventArgs(path));
            }
            else
            {
                this.SaveAsSpreadSheet(this, e);
            }
        }

        /// <summary>
        /// Opens a file dialog prompt for the user to choose a location and file name for the spreadsheet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAsSpreadSheet(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Spreadsheet files (*.SS) |*.SS| All files (*.*)|*.*";
            save.InitialDirectory = "c:\\";
            save.FilterIndex = 1;

            if (save.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show(save.FileName);
                if (SaveFile != null)
                {
                    SaveFile(this, new FileEventArgs(save.FileName));
                }
            }

        }

        /// <summary>
        /// Closes the window and notifies the close file event handler that the file needs to be closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseWindow(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// When the window in question is x'ed out of. It triggers the close window prompt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XOutOfWindow(object sender, FormClosingEventArgs e)
        {
            if (CloseFile != null)
            {
                CloseFile(this, e);
            }
        }
    }
}
