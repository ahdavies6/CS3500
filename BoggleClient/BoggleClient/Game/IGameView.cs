using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleClient.Game
{
    /// <summary>
    /// Event handler for NextState event, which contains all the data (within e) to construct a ScoreView.
    /// </summary>
    public delegate void NextStateEventHandler(object sender, NextStateEventArgs e);

    /// <summary>
    /// Data necessary to add a word
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void AddWordEventHandler(object sender, AddWordEventArgs e);

    /// <summary>
    /// Interface for the GameController to interact with
    /// </summary>
    public interface IGameView
    {
        /// <summary>
        /// The player's name
        /// </summary>
        string PlayerName { set; }

        /// <summary>
        /// The player's score
        /// </summary>
        int PlayerScore { set; }

        /// <summary>
        /// The opponent's name
        /// </summary>
        string OpponentName { set; }

        /// <summary>
        /// The opponent's score
        /// </summary>
        int OpponentScore { set; }

        /// <summary>
        /// The time remaining in the game
        /// </summary>
        int TimeRemaining { get; set; }

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

    /// <summary>
    /// Data necessary to add a word in the game
    /// </summary>
    public class AddWordEventArgs : EventArgs
    {
        /// <summary>
        /// Word to be added
        /// </summary>
        public string Word
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates AddWordEventArgs containing the word to be added
        /// </summary>
        /// <param name="word"></param>
        public AddWordEventArgs(string word)
        {
            this.Word = word;
        }
    }

    /// <summary>
    /// Contains the event data for NextState event, including all the data necessary to construct
    /// a ScoreView.
    /// </summary>
    public class NextStateEventArgs : EventArgs
    {
        /// <summary>
        /// Empty eventargs
        /// </summary>
        public NextStateEventArgs() { }
    }
}
