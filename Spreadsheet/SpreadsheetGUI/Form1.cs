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

        public event EventHandler NewFile;
        public event OpenFileEventHandler OpenFile;
        public event EventHandler SaveFile;
        public event EventHandler CloseFile;
        public event SetContentsEventHandler SetContents;

        public IView GetNew()
        {
            throw new NotImplementedException();
        }


        public void DisplayContents(string CellName, string CellValue)
        {
            int col = ConvertColToInt(CellName[0]);
            int row = int.Parse(CellName.Substring(1));
            this.spreadsheetPanel1.SetValue(col, row, CellValue);
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

    }
}
