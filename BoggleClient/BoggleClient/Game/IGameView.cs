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
    /// Interface for the GameController to interact with
    /// </summary>
    public interface IGameView
    {
        /// <summary>
        /// Event that gets fired when a new word is added
        /// </summary>
        event Action<string> AddWord;

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
