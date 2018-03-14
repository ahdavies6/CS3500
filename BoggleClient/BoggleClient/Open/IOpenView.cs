using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleClient.Open
{
    // todo: missing doc comments

    /// <summary>
    /// Event handler for NextState event, which contains all the data (within e) to construct a GameView.
    /// </summary>
    public delegate void NextStateEventHandler(object sender, NextStateEventArgs e);

    public delegate void ConnectEventHandler(object sender, ConnectEventArgs e);

    /// <summary>
    /// Interface for the OpenController to interact with
    /// </summary>
    public interface IOpenView
    {
        /// <summary>
        /// Attempt to connect to the server
        /// </summary>
        event ConnectEventHandler ConnectToServer;

        /// <summary>
        /// Event that occurs when the cancel button is pushed
        /// </summary>
        event Action CancelPushed;

        /// <summary>
        /// Moves to the next state in the game (the actual boggle game)
        /// </summary>
        event NextStateEventHandler NextState;
    }

    // todo: revisit (check) after implementing OpenController
    // is this necessary, considering NextState?
    public class ConnectEventArgs : EventArgs
    {
        public string UserToken
        {
            get;
            private set;
        }

        public int GameLength
        {
            get;
            private set;
        }

        public ConnectEventArgs(string userID, int gameLength)
        {
            this.UserToken = userID;
            this.GameLength = gameLength;
        }
    }

    /// <summary>
    /// Contains the event data for NextState event, including everything necessary to construct
    /// a GameView.
    /// </summary>
    public class NextStateEventArgs : EventArgs
    {
        public string UserID
        {
            get;
            private set;
        }

        public string URL
        {
            get;
            private set;
        }

        public string Nickname
        {
            get;
            private set;
        }

        public NextStateEventArgs(string userID, string URL, string nickname)
        {
            this.UserID = userID;
            this.URL = URL;
            this.Nickname = nickname;
        }
    }
}
