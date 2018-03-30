using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;

namespace Boggle
{
    /// <summary>
    /// Class that represents a single BoggleGame
    /// </summary>
    public class BoggleGame
    {

        /// <summary>
        /// The board model provided by Joe Zachary
        /// </summary>
        private BoggleBoard BoardModel;

        /// <summary>
        /// Player object that represents the first player
        /// </summary>
        public Player Player1 { get; private set; }

        /// <summary>
        /// Player object that represents the second player
        /// </summary>
        public Player Player2 { get; private set; }

        /// <summary>
        /// The Time limit of the game. Does not change after the initial set
        /// </summary>
        public int TimeLimit { get; private set; }

        /// <summary>
        /// Timer object that keeps track of how long the game has. Fires the end game method when completed
        /// </summary>
        private Timer timer;

        /// <summary>
        /// Represents the game status
        /// </summary>
        public string GameStatus { get; private set; }
    }

    /// <summary>
    /// Player structure that keeps track of certain player elements
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Represents the nickname of the player
        /// </summary>
        public string Nickanme { get; set; }

        /// <summary>
        /// Represents the Score the player
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Represents all the words played
        /// </summary>
        public IList<string> Words { get; set; }

        /// <summary>
        /// Represents the score after each word
        /// </summary>
        public IList<int> ScoreIncrements { get; set; }

        /// <summary>
        /// Creates a new player struct with score set to 0, words and ScoreIncrements as empty lists, and Nickname as nickname
        /// </summary>
        /// <param name="nickname"></param>
        public Player(string nickname)
        {
            Nickanme = nickname;
            Score = 0;
            Words = new List<string>();
            ScoreIncrements = new List<int>();
        }
    }
}