using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// Constant representing a pending game 
        /// </summary>
        public const int PENDING_GAME = 1;

        /// <summary>
        /// Constant representing an active game 
        /// </summary>
        public const int ACTIVE_GAME = 2;

        /// <summary>
        /// Constant representing a completed game 
        /// </summary>
        public const int COMPLETED_GAME = 3;

        /// <summary>
        /// The board model provided by Joe Zachary
        /// </summary>
        public BoggleBoard BoardModel;

        /// <summary>
        /// Property that returns the string for the board 
        /// </summary>
        public string Board { get { return BoardModel.ToString(); } }

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
        private Stopwatch timer;

        /// <summary>
        /// Property that represents how much time is left in the game
        /// </summary>
        public int TimeRemaining
        {
            get
            {
                if (GameStatus == ACTIVE_GAME)
                {
                    return 0;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Represents the game status.
        /// 1 = pending
        /// 2 = active
        /// 3 = completed 
        /// </summary>
        public int GameStatus { get; private set; }

        /// <summary>
        /// Adds the first player to the Boggle Game and makes the game. 
        /// </summary>
        public BoggleGame(string Nickname1, string UserToken1, int requestedTime)
        {
            this.Player1 = new Player(Nickname1, UserToken1);
            this.TimeLimit = requestedTime;
            this.GameStatus = PENDING_GAME;
        }

        /// <summary>
        /// Adds the second player into the game and starts the game. The Time limit of the game is set to the 
        /// overage of the two times requested by the players
        /// </summary>
        public void AddSecondPlayer(string Nickname2, string UserToken2, int requestedTime)
        {
            this.Player2 = new Player(Nickname2, UserToken2);
            this.TimeLimit = (TimeLimit + requestedTime) / 2;
            this.BoardModel = new BoggleBoard();
            this.timer = new Timer(TimeLimit * 1000);
            this.timer.Elapsed += EndGame;
            this.GameStatus = ACTIVE_GAME;
            this.timer.Start();
        }

        /// <summary>
        /// Method that is fired when the timer ends, signalling the end of the game
        /// </summary>
        // todo: should we use locks? Will this create some sort of race condition?
        private void EndGame(object sender, ElapsedEventArgs e)
        {
            this.GameStatus = COMPLETED_GAME;
            DateTime.UtcNow.TotalSeconds;
        }
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
        /// UserToken of the player 
        /// </summary>
        public string UserToken { get; private set; }

        /// <summary>
        /// Creates a new player struct with score set to 0, words and ScoreIncrements as empty lists, and Nickname as nickname
        /// </summary>
        /// <param name="nickname"></param>
        public Player(string nickname, string uToken)
        {
            Nickanme = nickname;
            UserToken = uToken;
            Score = 0;
            Words = new List<string>();
            ScoreIncrements = new List<int>();
        }
    }
}