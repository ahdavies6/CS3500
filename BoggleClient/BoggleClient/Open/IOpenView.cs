using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleClient.Open
{
    /// <summary>
    /// The data necessary to connect to the server
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ConnectEventHandler(object sender, ConnectEventArgs e);

    /// <summary>
    /// The data necessary to search for a game
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void SearchGameEventHandler(object sender, SearchGameEventArgs e);

    /// <summary>
    /// Interface for the OpenController to interact with
    /// </summary>
    public interface IOpenView
    {
        /// <summary>
        /// Whether the user is registering with the server
        /// </summary>
        bool Registering
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the user has been registered with the server
        /// </summary>
        bool Registered
        {
            get;
            set;
        }

        /// <summary>
        /// Attempt to connect to the server
        /// </summary>
        event ConnectEventHandler RegisterUser;

        /// <summary>
        /// Searches for a new game
        /// </summary>
        event SearchGameEventHandler SearchGame;

        /// <summary>
        /// Event that occurs when the cancel button is pushed
        /// </summary>
        event Action CancelRegister;

        /// <summary>
        /// Cancels searching for a game
        /// </summary>
        event Action CancelSearch;

        /// <summary>
        /// Refreshes which controls are accessible to the user
        /// </summary>
        void RefreshFieldAccess();

        /// <summary>
        /// Informs the user that their nickname or URL is invalid
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="URL"></param>
        void ShowNameRegistrationMsg(string nickname, string URL);
    }

    /// <summary>
    /// Contains the necessary data to connect to the server
    /// </summary>
    public class ConnectEventArgs : EventArgs
    {
        /// <summary>
        /// The server's URL
        /// </summary>
        public string URL
        {
            get;
            private set;
        }

        /// <summary>
        /// The player's username
        /// </summary>
        public string Nickname
        {
            get;
            private set;
        }

        /// <summary>
        /// The data necessary to connect to the server
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="nickname"></param>
        public ConnectEventArgs(string URL, string nickname)
        {
            this.URL = URL;
            this.Nickname = nickname;
        }
    }

    /// <summary>
    /// Contains the data to search for a game
    /// </summary>
    public class SearchGameEventArgs : EventArgs
    {
        /// <summary>
        /// The desired game length
        /// </summary>
        public int GameLength
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates an eventargs containg data to search for a game
        /// </summary>
        /// <param name="gameLength"></param>
        public SearchGameEventArgs(int gameLength)
        {
            this.GameLength = gameLength;
        }

        /// <summary>
        /// Creates an eventargs containing data to search for a game. Converts (gameLength) to an int, and returns -1 if gameLength
        /// cannot be converted, or is outside the required range.
        /// </summary>
        /// <param name="gameLength"></param>
        public SearchGameEventArgs(string gameLength)
        {
            if (Int32.TryParse(gameLength, out int length) && !(length < 5) && !(length > 120))
            {
                this.GameLength = length;
            }
            else
            {
                // equivalent to an error message
                this.GameLength = -1;
            }
        }
    }
}
