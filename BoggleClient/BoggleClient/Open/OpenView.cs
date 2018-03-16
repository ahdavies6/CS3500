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
    // todo: add doc comments
    public partial class OpenView : Form, IOpenView
    {
        /// <summary>
        /// Opens the Form
        /// </summary>
        public OpenView()
        {
            InitializeComponent();

            //int scorePanelSizeX = (int)(this.ClientSize.Width);
            //int scorePanelSizeY = (int)(this.ClientSize.Height * 0.8);
            //ScorePanel.Size = new Size(scorePanelSizeX, scorePanelSizeY);
            //ScorePanel.Location = Context.TLPointCenterDynamic(ScorePanel);

            int mainPanelSizeX = (int)(this.ClientSize.Width);
            int mainPanelSizeY = (int)(this.ClientSize.Height * 0.9);
            MainPanel.Size = new Size(mainPanelSizeX, mainPanelSizeY);
            MainPanel.Location = Context.TLPointCenterDynamic(MainPanel);
        }

        // todo: after merging, refactor to RegisterUser
        /// <summary>
        /// Attempt to connect to the server
        /// </summary>
        public event ConnectEventHandler ConnectToServer;

        public event SearchGameEventHandler SearchGame;

        // todo: after merging, refactor to CancelRegister
        /// <summary>
        /// Event that occurs when the cancel button is pushed
        /// </summary>
        public event Action CancelPushed;

        public event Action CancelSearch;

        /// <summary>
        /// Moves to the next state in the game (the actual boggle game)
        /// </summary>
        //public event NextStateEventHandler NextState;

        private void ServerRegisterButton_Click(object sender, EventArgs e)
        {
            ConnectEventArgs args = new ConnectEventArgs(ServerTextbox.Text, NameTextbox.Text);
            ConnectToServer?.Invoke(this, args);
        }

        private void SearchGamesButton_Click(object sender, EventArgs e)
        {
            //NextStateEventArgs args = new NextStateEventArgs(DurationTextbox.Text);

            //if (args.GameLength != -1)
            //{
            //    NextState?.Invoke(this, args);
            //}
            //NextStateEventArgs args = new NextStateEventArgs(DurationTextbox.Text);

            SearchGameEventArgs args = new SearchGameEventArgs(DurationTextbox.Text);

            if (args.GameLength != -1)
            {
                SearchGame?.Invoke(this, args);
            }
            else
            {
                // todo: implement some kind of error?
            }

        }

        private void CancelRegisterButton_Click(object sender, EventArgs e)
        {
            CancelPushed?.Invoke();
        }

        private void CancelSearchButton_Click(object sender, EventArgs e)
        {
            CancelSearch?.Invoke();
        }
    }
}
