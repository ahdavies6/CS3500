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


        public Form1()
        {
            InitializeComponent();

        }

        event SaveFileEventHandler IView.SaveFile
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        public event SaveFileEventHandler SaveFileAs;
        public event EventHandler NewFile;
        public event OpenFileEventHandler OpenFile;
        public event EventHandler CloseFile;
        public event SetContentsEventHandler SetContents;
        public event EventHandler SaveFile;

        public IView GetNew()
        {
            Form1 newss = new Form1();
            return newss;
        }


        public void DisplayContents(string CellName, string CellValue)
        {
            int col = ConvertColToInt(CellName[0]);
            int row = int.Parse(CellName.Substring(1));
            this.spreadsheetPanel1.SetValue(col, row, CellValue);
        }


        public bool WarningPrompt()
        {
            DialogResult result = MessageBox.Show("There are unsaved changes! Do you want to save or cancel?", "Warning!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if(result == DialogResult.Yes)
            {
                this.SaveFile(this, new EventArgs());
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
                try
                {
                    if (OpenFile != null)
                    {
                        OpenFile(this, new OpenFileEventArgs(open.FileName));
                    }
                }
                catch(Exception ex) //CorruptedFileException e)
                {
                    MessageBox.Show("The file " + open.FileName + "Could not be opened.");
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
            if (this.SaveFile != null)
            {
                SaveFile(this, e);
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
                if (SaveFileAs != null)
                {
                    SaveFileAs(this, new SaveFileEventArgs(save.FileName));
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
