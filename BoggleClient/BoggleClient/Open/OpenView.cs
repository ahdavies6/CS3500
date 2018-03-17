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
        // todo: delete whichever of the first two is unnecessary
        public bool Registering
        {
            get;
            set;
        }
        public bool Registered
        {
            get;
            set;
        }
        private bool Searching;

        /// <summary>
        /// Opens the Form
        /// </summary>
        public OpenView()
        {
            InitializeComponent();

            int mainPanelSizeX = (int)(this.ClientSize.Width);
            int mainPanelSizeY = (int)(this.ClientSize.Height * 0.9);
            MainPanel.Size = new Size(mainPanelSizeX, mainPanelSizeY);
            MainPanel.Location = Context.TLPointCenterDynamic(MainPanel);

            Registering = false;
            Registered = false;
            Searching = false;
            RefreshFieldAccess();

            // todo: URGENT REMOVE THIS
            ServerTextbox.Text = "http://ice.users.coe.utah.edu/";
        }

        /// <summary>
        /// Attempt to connect to the server
        /// </summary>
        public event ConnectEventHandler RegisterUser;

        public event SearchGameEventHandler SearchGame;

        // todo: after merging, refactor to CancelRegister
        /// <summary>
        /// Event that occurs when the cancel button is pushed
        /// </summary>
        public event Action CancelRegister;

        public event Action CancelSearch;

        public void RefreshFieldAccess()
        {
            ServerTextbox.Enabled = !Registering && !Searching;
            NameTextbox.Enabled = !Registering && !Searching;
            ServerRegisterButton.Enabled = !Registering && !Searching;
            CancelRegisterButton.Enabled = Registering;

            DurationTextbox.Enabled = Registered && !Searching;
            SearchGamesButton.Enabled = Registered && !Searching;
            CancelSearchButton.Enabled = Registered && Searching;
        }

        private void ServerRegisterButton_Click(object sender, EventArgs e)
        {
            Registered = false;
            Registering = true;
            RefreshFieldAccess();

            ConnectEventArgs args = new ConnectEventArgs(ServerTextbox.Text, NameTextbox.Text);
            RegisterUser?.Invoke(this, args);
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
                Searching = true;
                RefreshFieldAccess();

                SearchGame?.Invoke(this, args);
            }
            else
            {
                MessageBox.Show("Please pick a value between 5 and 120 for the duration!");
            }
        }

        private void CancelRegisterButton_Click(object sender, EventArgs e)
        {
            Registered = false;
            Registering = false;
            RefreshFieldAccess();

            CancelRegister?.Invoke();
        }

        private void CancelSearchButton_Click(object sender, EventArgs e)
        {
            Searching = false;
            RefreshFieldAccess();

            CancelSearch?.Invoke();
        }

        public void ShowNameRegistrationMsg(string nickname, string URL)
        {
            MessageBox.Show(string.Format("Sorry! The server at: {0} did not allow registration of the name: {1}. Please try again!", URL, nickname));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string messageText = "To register a new username (or connect with an existing one), " +
                "enter the server URL into the \"Server Address\" textbox, enter your desired username " +
                "into the \"Player Name\" textbox, and click \"Register with Server\". \r\n" +
                "To cancel this registration (if registering takes too long, or you wish to change " +
                "your username), click \"Cancel Registration. \r\n" +
                "Once your username has been accepted by the server, type the desired game duration " +
                "(in seconds) into the \"Game Duration\" textbox, and click \"Search for Game\". \r\n" +
                "If you wish to cancel the search for your game (if the wait is taking too long, " +
                "or you wish to adjust your game duration), click \"Stop Searching\".";
            MessageBox.Show(messageText);
        }
    }
}
