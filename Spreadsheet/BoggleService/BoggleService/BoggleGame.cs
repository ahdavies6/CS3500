using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Timers;
using System.Web;

namespace Boggle
{
    // todo: remove all the commented out code in here

    /// <summary>
    /// Class that represents a single BoggleGame
    /// </summary>
    public class BoggleGame
    {
        // todo: remove deprecated
        ///// <summary>
        ///// Represents all of the valid words possible
        ///// 
        ///// NOTE: You might have to remove this if the memory overhead is too large and it crashes, this is something to test
        ///// against the tests i wrote
        ///// </summary>
        //public static ISet<string> DictionaryWords;

        /// <summary>
        /// The board model provided by Joe Zachary
        /// </summary>
        public BoggleBoard Board;

        /// <summary>
        /// Player object that represents the first player
        /// </summary>
        public Player Player1 { get; private set; }

        /// <summary>
        /// Player object that represents the second player
        /// </summary>
        public Player Player2 { get; private set; }

        /// <summary>
        /// GameID of this game
        /// </summary>
        public string GameID { get; private set; }

        /// <summary>
        /// Represents the game status
        /// </summary>
        public GameStatus Status;

        /// <summary>
        /// The Time limit of the game. Does not change after the initial set
        /// </summary>
        public int TimeLimit { get; private set; }

        /*
         * See notes in AddSecondPlayer
        /// <summary>
        /// Will refresh the game each second
        /// </summary>
        private Timer timer;

        /// <summary>
        /// Keeps track of the time since the game began
        /// </summary>
        private Stopwatch stopwatch;*/


        /// <summary>
        /// keeps track of when the game started
        /// </summary>
        private DateTime Start;

        /// <summary>
        /// How much time is left in the game
        /// </summary>
        public int TimeLeft
        {
            get
            {
                //int timeLeft = TimeLimit - (int)stopwatch.ElapsedMilliseconds;

                int timeLeft = (int)(Start.AddSeconds(TimeLimit) - DateTime.UtcNow).TotalSeconds;

                if (timeLeft > 0)
                {
                    return timeLeft;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Initializes a new game with one player
        /// </summary>
        public BoggleGame(User player, int requestedTime, string gameID)
        {
            Board = new BoggleBoard();
            Player1 = new Player(player, requestedTime);
            GameID = gameID;

            Status = GameStatus.Pending;
        }

        /// <summary>
        /// Adds the second player into the game and starts the game. The Time limit of the game is set to the 
        /// overage of the two times requested by the players
        /// </summary>
        public void AddSecondPlayer(User player, int requestedTime)
        {
            Player2 = new Player(player, requestedTime);
            TimeLimit = (Player1.RequestedTime + Player2.RequestedTime) / 2;

            // todo: remove deprecated
            ////I also added this hashset that hold all the dictionary words, it should make looking up words faster 
            ////but there might be too much memory used 
            ////Thats why i kept it static 
            //if (DictionaryWords == null)
            //{
            //    DictionaryWords = new HashSet<string>();
            //    GenerateDictionary();
            //}

            //see note below
            Start = DateTime.UtcNow;

            //I removed this since having it run every second might be too much overhead? We can just have it refresh before 
            // any operation is done to check if the status is still active. 
            //using stopwatch and timer spawns 3 threads just for timing and updating status, this should update just the same
            //but without the overhead, especially when more games are added
            //using DateTime is how joe does it in piazza so i just kept that version
            //if this logic doesnt make senes text me and ill try to explain better
            //timer = new Timer(1000);
            //timer.Elapsed += (object sender, ElapsedEventArgs e) => { Refresh(); };
            //stopwatch = new Stopwatch();

            Status = GameStatus.Active;
        }

        // todo: remove deprecated
        ///// <summary>
        ///// Helper method that generates the static dictionary
        ///// </summary>
        //private void GenerateDictionary()
        //{
        //    using (StreamReader file = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "dicationary.txt"))
        //    {
        //        string currLine;
        //        while ((currLine = file.ReadLine()) != null)
        //        {
        //            DictionaryWords.Add(currLine.ToUpper());
        //        }
        //    }
        //}

        /// <summary>
        /// If this game contains user player, returns them
        /// 
        /// Otherwise, returns null
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public User GetUser(User player)
        {
            if (Player1.User == player)
            {
                return Player1.User;
            }
            else if (Player2.User == player)
            {
                return Player2.User;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// If game is still active, adds a word to the game (under player playerName), scores it, adds it (and
        /// its score) to player's data, and returns the score.
        /// 
        /// If game is no longer active, throws GameNotActiveException.
        /// </summary>
        private int PlayWord(Player player, string word)
        {
            Refresh();

            if (Status != GameStatus.Active)
            {
                throw new GameNotActiveException();
            }

            int wordScore;

            if (Board.CanBeFormed(word))
            {
                wordScore = ScoreWord(word, player);
            }
            else
            {
                wordScore = -1;
            }

            player.AddWord(word, wordScore);
            return wordScore;
        }


        /// <summary>
        /// If game is still active, adds a word to the game (under user), scores it, adds it (and
        /// its score) to player's data, and returns the score.
        /// 
        /// If game is no longer active, throws GameNotActiveException.
        /// If user is not in this game, throws PlayerNotInGameException.
        /// </summary>
        public int PlayWord(User user, string word)
        {

            int wordScore;

            if (Player1.User == user)
            {
                wordScore = PlayWord(Player1, word);
            }
            else if (Player2.User == user)
            {
                wordScore = PlayWord(Player2, word);
            }
            else
            {
                throw new PlayerNotInGameException();
            }

            return wordScore;
        }

        ///// <summary>
        ///// If game is still active, adds a word to the game (under user with ID userToken), scores it,
        ///// adds it (and its score) to player's data, and returns the score.
        ///// 
        ///// If game is no longer active, throws GameNotActiveException.
        ///// If user is not in this game, throws PlayerNotInGameException.
        ///// </summary>
        //public int PlayWord(string userToken, string word)
        //{
        //    int wordScore;

        //    if (Player1.User.UserToken == userToken)
        //    {
        //        wordScore = PlayWord(Player1, word);
        //    }
        //    else if (Player2.User.UserToken == userToken)
        //    {
        //        wordScore = PlayWord(Player2, word);
        //    }
        //    else
        //    {
        //        throw new PlayerNotInGameException();
        //    }

        //    return wordScore;
        //}

        /// <summary>
        /// Given a word, it scores it given that it is a valid word. Follows these guidlines from PS8:
        /// 
        /// If a string has fewer than three characters, it scores zero points.
        /// Otherwise, if a string has a duplicate that occurs earlier in the list, it scores zero points.
        /// Otherwise, if a string is legal (it appears in the dictionary and occurs on the board), 
        /// it receives a score that depends on its length. Three- and four-letter words are worth one point, 
        /// five-letter words are worth two points, six-letter words are worth three points, 
        /// seven-letter words are worth five points, and longer words are worth 11 points.
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
            else if (this.Board.CanBeFormed(word) && Words.IsValidWord(word))
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

        // todo: remove deprecated
        ///// <summary>
        ///// Checks if the param word is a valid word within dictionary.txt
        ///// 
        ///// Method is case insensitive
        ///// </summary>
        //private bool IsValidWord(string word)
        //{
        //    return DictionaryWords.Contains(word.ToUpper());
        //    /*
        //    using (StreamReader file = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "dicationary.txt"))
        //    {
        //        string currLine;
        //        while ((currLine = file.ReadLine()) != null)
        //        {
        //            if (word.Equals(currLine))
        //            {
        //                return true;
        //            }
        //        }
        //    }

        //    //Case no dictionary word matches with the word param
        //    return false;*/
        //}

        /// <summary>
        /// Ends the game once the time has run out
        /// </summary>
        private void Refresh()
        {
            if (TimeLeft < 0)
            {
                Status = GameStatus.Completed;

                //timer = null;
                //stopwatch = null;
            }
        }
    }

    /// <summary>
    /// Represents the state of the game, be it pending, active, or completed
    /// </summary>
    public enum GameStatus
    {
        Pending, Active, Completed
    }

    /// <summary>
    /// Player structure that keeps track of certain player elements
    /// </summary>
    public class Player
    {
        /// <summary>
        /// The user who is this player
        /// </summary>
        public User User { get; private set; }

        /// <summary>
        /// How much time the player requested
        /// </summary>
        public int RequestedTime { get; private set; }

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
        /// Creates a new player struct with score set to 0, words and ScoreIncrements as empty lists, and Nickname as nickname
        /// </summary>
        /// <param name="nickname"></param>
        public Player(User me, int requestedTime)
        {
            User = me;
            RequestedTime = requestedTime;

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