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
    /// <summary>
    /// Windows form to join a boggle game
    /// </summary>
    public partial class OpenView : Form, IOpenView
    {
        /// <summary>
        /// Whether the client is registering a new user
        /// </summary>
        public bool Registering
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the user has been registered
        /// </summary>
        public bool Registered
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the client is searching for a game
        /// </summary>
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
        }

        /// <summary>
        /// Attempt to connect to the server
        /// </summary>
        public event ConnectEventHandler RegisterUser;

        /// <summary>
        /// Attemt to search for a game
        /// </summary>
        public event SearchGameEventHandler SearchGame;

        /// <summary>
        /// Fired when the user cancels registering their username
        /// </summary>
        public event Action CancelRegister;

        /// <summary>
        /// Fired when the user cancels searching for a game
        /// </summary>
        public event Action CancelSearch;

        /// <summary>
        /// Refreshes which textboxes and buttons should be accessible to the user
        /// </summary>
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

        /// <summary>
        /// Registers the player with the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerRegisterButton_Click(object sender, EventArgs e)
        {
            Registered = false;
            Registering = true;
            RefreshFieldAccess();

            ConnectEventArgs args = new ConnectEventArgs(ServerTextbox.Text, NameTextbox.Text);
            RegisterUser?.Invoke(this, args);
        }

        /// <summary>
        /// Begins searching for a game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Cancels username registration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelRegisterButton_Click(object sender, EventArgs e)
        {
            Registered = false;
            Registering = false;
            RefreshFieldAccess();

            CancelRegister?.Invoke();
        }

        /// <summary>
        /// Cancels searching for a game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelSearchButton_Click(object sender, EventArgs e)
        {
            Searching = false;
            RefreshFieldAccess();

            CancelSearch?.Invoke();
        }

        /// <summary>
        /// Called when the user's name is illegal
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="URL"></param>
        public void ShowNameRegistrationMsg(string nickname, string URL)
        {
            MessageBox.Show(string.Format("Sorry! The server at: {0} did not allow registration of the name: {1}. Please try again!", URL, nickname));
        }

        /// <summary>
        /// Shows a help message box when the user clicks the help button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
