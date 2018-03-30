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
    /// Returns the score of word, should it be playable with the BoggleGame's BoggleBoard.
    /// </summary>
    /// <param name="word"></param>
    /// <returns></returns>
    public delegate int GetWordScore(string word);

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
        public BoggleBoard Board;

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
        /// Represents the game status
        /// </summary>
        public string GameStatus { get; private set; }

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
        /// GameID of this game
        /// </summary>
        public string GameID { get; private set; }

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
        public BoggleGame(string Nickname1, string UserToken1, int requestedTime, string GameID)
        {
            this.Player1 = new Player(Nickname1, UserToken1);
            this.TimeLimit = requestedTime;
            this.Status = PENDING_GAME;
            this.GameID = GameID;

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
        /// Returns the score of word, should it be playable with the BoggleGame's BoggleBoard.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private GetWordScore getWordScore;

        /// <summary>
        /// Creates a new BoggleGame.
        /// </summary>
        /// <param name="letters"></param>
        /// <param name="p1Name"></param>
        /// <param name="p2Name"></param>
        /// <param name="timeLimit"></param>
        public BoggleGame(string letters, string p1Name, string p2Name, int timeLimit, GetWordScore getWordScore)
        {
            Board = new BoggleBoard(letters);
            Player1 = new Player(p1Name);
            Player2 = new Player(p2Name);
            TimeLimit = timeLimit;
            GameStatus = "active";

            timer = new Timer(timeLimit * 1000);
            timer.Elapsed += Completed;
            this.getWordScore = getWordScore;
        }

        /// <summary>
        /// Plays word under player.
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="word"></param>
        public void PlayWord(string playerName, string word)
        {
            Player player;
            int wordScore;

            if (playerName == Player1.Nickname)
            {
                player = Player1;
            }
            else if (playerName == Player2.Nickname)
            {
                player = Player2;
            }
            else // there is no Player named "playerName" in this game
            {
                throw new ArgumentOutOfRangeException(playerName);
            }

            if (Board.CanBeFormed(word))
            {
                wordScore = getWordScore(word);
            }
            else
            {
                wordScore = -1;
            }

            player.AddWord(word, wordScore);
        }

        /// <summary>
        /// Called when the game has expired
        /// </summary>
        private void Refresh()
        {
            double timeLeft = (DateTime.UtcNow - this.StartTime).TotalSeconds;
            if (timeLeft < 0)
            {
                EndGame();
            }

        }
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Completed(object sender, ElapsedEventArgs e)
        {
            timer = null;
            GameStatus = "completed";
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
        public string Nickname { get; private set; }

        /// <summary>
        /// Represents the Score the player
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        /// Represents all the words played
        /// </summary>
        public IList<string> Words { get; private set; }

        /// <summary>
        /// Represents the score per each word
        /// </summary>
        public IList<int> WordScores { get; private set; }

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
            WordScores = new List<int>();
        }

        /// <summary>
        /// Adds word and wordScore to this player's data.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="wordScore"></param>
        public void AddWord(string word, int wordScore)
        {
            Words.Add(word);
            WordScores.Add(wordScore);
            Score += wordScore;
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