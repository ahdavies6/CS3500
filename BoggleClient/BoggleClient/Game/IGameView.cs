using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleClient.Game
{
    // todo: missing doc comments

    /// <summary>
    /// Event handler for NextState event, which contains all the data (within e) to construct a ScoreView.
    /// </summary>
    public delegate void NextStateEventHandler(object sender, NextStateEventArgs e);

    public delegate void AddWordEventHandler(object sender, AddWordEventArgs e);

    /// <summary>
    /// Interface for the GameController to interact with
    /// </summary>
    public interface IGameView
    {
        // todo: implement setting of these into GameController, in some form
        string PlayerName { set; }
        int PlayerScore { set; }
        string OpponentName { set; }
        int OpponentScore { set; }
        int TimeRemaining { set; }

        /// <summary>
        /// Event that gets fired when a new word is added
        /// </summary>
        event AddWordEventHandler AddWord;

        /// <summary>
        /// Event that gets fired if the cancel button is clicked
        /// </summary>
        event Action CancelPushed;

        /// <summary>
        /// Moves the game to the score state
        /// </summary>
        event NextStateEventHandler NextState;

        /// <summary>
        /// Method that creates a set of labels for the dice passed through by DiceConfigs
        /// </summary>
        void GenerateLabels(string DiceConfig);
    }

    public class AddWordEventArgs : EventArgs
    {
        public string Word
        {
            get;
            private set;
        }

        public AddWordEventArgs(string word)
        {
            this.Word = word;
        }
    }

    // todo: implement this after ScoreView is finished
    /// <summary>
    /// Contains the event data for NextState event, including all the data necessary to construct
    /// a ScoreView.
    /// </summary>
    public class NextStateEventArgs : EventArgs
    {
        public NextStateEventArgs() { }
    }
}
