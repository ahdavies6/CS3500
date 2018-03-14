using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleClient.Game
{
    /// <summary>
    /// Interface for the GameController to interact with
    /// </summary>
    interface IGameView
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
        // TODO: Fix the action to a proper delegate type
        event Action NextState;

        /// <summary>
        /// Method that creates a set of labels for the dice passed through by DiceConfigs
        /// </summary>
        /// <param name="DiceConfig"></param>
        void GenerateLabels(string DiceConfig);

    }
}
