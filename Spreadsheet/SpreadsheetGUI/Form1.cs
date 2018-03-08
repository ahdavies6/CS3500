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
    }
}
