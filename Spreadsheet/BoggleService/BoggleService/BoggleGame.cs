using System;
using System.Collections.Generic;
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
        private Timer timer;

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
        /// Creates a new player struct with score set to 0, words and ScoreIncrements as empty lists, and Nickname as nickname
        /// </summary>
        /// <param name="nickname"></param>
        public Player(string nickname)
        {
            Nickname = nickname;
            Score = 0;
            Words = new List<string>();
            WordScores = new List<int>();
        }

        public void AddWord(string word, int wordScore)
        {
            Words.Add(word);
            WordScores.Add(wordScore);
            Score += wordScore;
        }
    }
}