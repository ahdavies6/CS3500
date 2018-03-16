using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleClient.Open
{
    // todo: missing doc comments

    public delegate void ConnectEventHandler(object sender, ConnectEventArgs e);

    public delegate void SearchGameEventHandler(object sender, SearchGameEventArgs e);

    // todo: remove deprecated?
    ///// <summary>
    ///// Event handler for NextState event, which contains all the data (within e) to construct a GameView.
    ///// </summary>
    //public delegate void NextStateEventHandler(object sender, NextStateEventArgs e);

    /// <summary>
    /// Interface for the OpenController to interact with
    /// </summary>
    public interface IOpenView
    {
        bool Registering
        {
            get;
            set;
        }

        bool Registered
        {
            get;
            set;
        }

        // todo: after merging, refactor as RegisterUser
        /// <summary>
        /// Attempt to connect to the server
        /// </summary>
        event ConnectEventHandler RegisterUser;

        event SearchGameEventHandler SearchGame;

        // todo: after merging, refactor as CancelRegister
        /// <summary>
        /// Event that occurs when the cancel button is pushed
        /// </summary>
        event Action CancelRegister;

        event Action CancelSearch;

        void RefreshFieldAccess();
        void ShowNameRegistrationMsg(string nickname, string URL);

        /// <summary>
        /// Moves to the next state in the game (the actual boggle game)
        /// </summary>
        //event Action NextState;
    }

    public class ConnectEventArgs : EventArgs
    {
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

        //public int GameLength
        //{
        //    get;
        //    private set;
        //}

        //public ConnectEventArgs(string URL, string userToken, int gameLength)
        public ConnectEventArgs(string URL, string nickname)
        {
            this.URL = URL;
            this.Nickname = nickname;
        }
    }

    // todo: remove deprecated?
    ///// <summary>
    ///// Contains the event data for NextState event, including everything necessary to construct
    ///// a GameView.
    ///// </summary>
    //public class NextStateEventArgs : EventArgs
    //{
    //    // UserToken should be stored by controller
    //    //public string UserToken
    //    //{
    //    //    get;
    //    //    private set;
    //    //}

    //    // URL should be stored by controller
    //    //public string URL
    //    //{
    //    //    get;
    //    //    private set;
    //    //}

    //    // Nickname should be stored by Controller
    //    //public string Nickname
    //    //{
    //    //    get;
    //    //    private set;
    //    //}

    //    public int GameLength
    //    {
    //        get;
    //        private set;
    //    }

    //    //public NextStateEventArgs(string userToken, string URL, string nickname)
    //    public NextStateEventArgs(int gameLength)
    //    {
    //        this.GameLength = gameLength;
    //    }

    //    public NextStateEventArgs(string gameLength)
    //    {
    //        if (Int32.TryParse(gameLength, out int length) && length > 5 && length < 120)
    //        {
    //            this.GameLength = length;
    //        }
    //        else
    //        {
    //            // equivalent to an error message
    //            this.GameLength = -1;
    //        }
    //    }
    //}

    public class SearchGameEventArgs : EventArgs
    {
        //public string UserToken
        //{
        //    get;
        //    private set;
        //}

        public int GameLength
        {
            get;
            private set;
        }

        //public SearchGameEventArgs(string userToken, int gameLength)
        public SearchGameEventArgs(int gameLength)
        {
            this.GameLength = gameLength;
        }

        public SearchGameEventArgs(string gameLength)
        {
            // todo: decide whether to handle length checking here or in Context
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
