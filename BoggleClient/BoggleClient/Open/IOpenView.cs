using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleClient.Open
{
    /// <summary>
    /// Event handler for NextState event, which contains all the data (within e) to construct a GameView.
    /// </summary>
    public delegate void NextStateEventHandler(object sender, NextStateEventArgs e);

    /// <summary>
    /// Interface for the OpenController to interact with
    /// </summary>
    public interface IOpenView
    {
        // todo: I don't think this needs to be here, in light of NextState (and its EventArgs)
        /// <summary>
        /// Attempt to connect to the server
        /// </summary>
        event Action<string, string> ConnectToServer;

        /// <summary>
        /// Event that occurs when the cancel button is pushed
        /// </summary>
        event Action CancelPushed;

        /// <summary>
        /// Moves to the next state in the game (the actual boggle game)
        /// </summary>
        event NextStateEventHandler NextState;
    }

    // todo: finish implementing this after GameView is finished (inc doc comments)
    /// <summary>
    /// Contains the event data for NextState event, including all the data necessary to construct
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

        public Game.IGameView View
        {
            get;
            private set;
        }

        public NextStateEventArgs(string userID, string URL, Game.IGameView view)
        {
            this.UserID = userID;
            this.URL = URL;
            this.View = view;
        }
    }
}
