using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoggleClient.Open
{
    public partial class OpenView : Form, IOpenView
    {
        public OpenView()
        {
            InitializeComponent();
        }

        // todo: I don't think this needs to be here, in light of NextState (and its EventArgs)
        /// <summary>
        /// Attempt to connect to the server
        /// </summary>
        public event Action<string, string> ConnectToServer;

        /// <summary>
        /// Event that occurs when the cancel button is pushed
        /// </summary>
        public event Action CancelPushed;

        /// <summary>
        /// Moves to the next state in the game (the actual boggle game)
        /// </summary>
        public event NextStateEventHandler NextState;
    }
}
