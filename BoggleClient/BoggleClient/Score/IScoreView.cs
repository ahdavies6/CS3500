using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleClient.Score
{
    /// <summary>
    /// Interface allows ScoreController to interact with a view (GUI)
    /// </summary>
    interface IScoreView
    {
        /// <summary>
        /// The player's username
        /// </summary>
        string PlayerName { set; }

        /// <summary>
        /// The player's final score
        /// </summary>
        int PlayerScore { set; }

        /// <summary>
        /// The words the player submitted
        /// </summary>
        string[] PlayerWords { set; }
        
        /// <summary>
        /// The scores the player earned
        /// </summary>
        int[] PlayerScores { set; }

        /// <summary>
        /// The username of the opponent
        /// </summary>
        string OpponentName { set; }

        /// <summary>
        /// The opponent's final score
        /// </summary>
        int OpponentScore { set; }

        /// <summary>
        /// The words the opponent played
        /// </summary>
        string[] OpponentWords { set; }

        /// <summary>
        /// The scores the opponent earned
        /// </summary>
        int[] OpponentScores { set; }

        /// <summary>
        /// Restarts the application and returns it to the open screen
        /// </summary>
        event Action CancelPushed;
    }
}
