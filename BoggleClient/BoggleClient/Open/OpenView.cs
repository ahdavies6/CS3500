using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoggleClient
{
    public partial class OpenView : Form, Open.IOpenView
    {
        public OpenView()
        {
            InitializeComponent();
        }

        public event Action<string, string> ConnectToServer;
        public event Action CancelPushed;
        public event Action NextState;
    }
}
