using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        private BoggleBoard BoardModel;

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
        private DateTime StartTime;

        public int TimeLeft
        {
            get
            {
                return (int) (StartTime - DateTime.UtcNow).TotalSeconds;
            }
        }

        /// <summary>
        /// Property that represents how much time is left in the game
        /// </summary>
        public int TimeRemaining
        {
            get
            {
                if (Status == ACTIVE_GAME)
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
        private int Status;

        /// <summary>
        /// Property that refreshes then gets the current games status
        /// 1 = pending
        /// 2 = active
        /// 3 = completed 
        /// </summary>
        public int GameStatus { get { Refresh(); return Status; } }

        /// <summary>
        /// Adds the first player to the Boggle Game and makes the game. 
        /// </summary>
        public BoggleGame(string Nickname1, string UserToken1, int requestedTime)
        {
            this.Player1 = new Player(Nickname1, UserToken1);
            this.TimeLimit = requestedTime;
            this.Status = PENDING_GAME;
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
            this.StartTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Adds a word to the game and scores it. 
        /// Returns the score if adding the word was successful. Returns -1 if the game is not active anymore.
        /// 
        /// Throws PlayerNotInGameException() if the userToken is not part of the game
        /// Thowos GameNotActiveException() if the game is not active
        /// 
        /// Assumes that word is a valid word less than 30 characters and not null
        /// </summary>
        public int AddWord(string userToken, string word)
        {
            //Refreshes the status of the game
            Refresh();

            //Checks if the game is still active
            if (this.Status != ACTIVE_GAME)
            {
                throw new GameNotActiveException();
            }

            Player currentPlayer;

            if (Player1.UserToken.Equals(userToken))
            {
                currentPlayer = Player1;
            }
            else if (Player2.UserToken.Equals(userToken))
            {
                currentPlayer = Player2;
            }
            else
            {
                throw new PlayerNotInGameException();
            }

            int score = ScoreWord(word, currentPlayer);

            currentPlayer.Score += score;
            currentPlayer.Words.Add(word);
            currentPlayer.ScoreIncrements.Add(score);

            return score;

        }

        //Todo move this into a data structure?
        /// <summary>
        /// Checks if the param word is a valid word within dictionary.txt
        /// 
        /// Method is case insensitive
        /// </summary>
        private bool IsValidWord(string word)
        {
            word = word.ToUpper();
            using (StreamReader file = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "dicationary.txt"))
            {
                string currLine;
                while ((currLine = file.ReadLine()) != null)
                {
                    if (word.Equals(currLine))
                    {
                        return true;
                    }
                }
            }

            //Case no dictionary word matches with the word param
            return false;
        }

        /// <summary>
        /// Given a word, it scores it given that it is a valid word. Follows these guidlines from PS8:
        /// 
        /// If a string has fewer than three characters, it scores zero points.
        /// Otherwise, if a string has a duplicate that occurs earlier in the list, it scores zero points.
        /// Otherwise, if a string is legal(it appears in the dictionary and occurs on the board), 
        ///     it receives a score that depends on its length.Three- and four-letter words are worth one point, 
        ///     five-letter words are worth two points, six-letter words are worth three points, 
        ///     seven-letter words are worth five points, and longer words are worth 11 points.
        /// Otherwise, the string scores negative one point.
        /// 
        /// Method is case insensitive
        /// </summary>
        private int ScoreWord(string word, Player currentPlayer)
        {
            if (word.Length < 3 || currentPlayer.Words.Contains(word.ToLower()))
            {
                return 0;
            }
            else if (currentPlayer.Words.Contains(word))
            {
                return 0;
            }
            else if (this.BoardModel.CanBeFormed(word) && IsValidWord(word))
            {
                int leng = word.Length;

                if (leng == 3 || leng == 4)
                {
                    return 1;
                }
                else if (leng == 5 || leng == 6)
                {
                    return leng - 3;
                }
                else if (leng == 7)
                {
                    return 5;
                }
                else
                {
                    return 11;
                }
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Private method that signals the end of the game
        /// </summary>
        private void EndGame()
        {
            this.Status = COMPLETED_GAME;
        }

        /// <summary>
        /// Helper method that refreshes the status of the game (from active to completed when time is right)
        /// </summary>
        private void Refresh()
        {
            double timeLeft = (DateTime.UtcNow - this.StartTime).TotalSeconds;
            if (timeLeft < 0)
            {
                EndGame();
            }

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

    /// <summary>
    /// Exception indicating that the player is not in the game
    /// </summary>
    public class PlayerNotInGameException : Exception
    {

        /// <summary>
        /// Constructor that merely makes the exception
        /// </summary>
        public PlayerNotInGameException() : base()
        {

        }
    }


    /// <summary>
    /// Exception that signals that the game is not active
    /// </summary>
    public class GameNotActiveException : Exception
    {
        /// <summary>
        /// Constructs a GameNotActiveException
        /// </summary>
        public GameNotActiveException() : base()
        {

        }
    }

}