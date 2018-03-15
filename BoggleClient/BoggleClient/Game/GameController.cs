using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace BoggleClient.Game
{
    class GameController
    {
        /// <summary>
        /// String that keeps track of the Users unique ID
        /// </summary>
        private string UserID;

        /// <summary>
        /// HttpClient that connects with the boggle server
        /// </summary>
        private HttpClient client;

        /// <summary>
        /// Controls the view of the board with an interface
        /// </summary>
        private Game.IGameView view;

        /// <summary>
        /// Constructs a GameController for game. The UID passed is the ID of the player and the URL is the URL to 
        /// connect to the server
        /// </summary>
        public GameController(string userID, string URL, Game.IGameView view)
        {
            this.UserID = userID;
            this.view = view;
            this.client = GenerateHttpClient(URL);

            //this.view.AddWord += AddWordToGame;
        }

        /// <summary>
        /// Method that refreshes what is seen on the Boggle Board GUI
        /// </summary>
        public void Refresh()
        {

        }

        /// <summary>
        /// Given a string url, the method returns a constructed HttpClient
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private HttpClient GenerateHttpClient(string url)
        {
            return null;
        }

        /// <summary>
        /// Given a string word, attempts to add it to the boggle server
        /// </summary>
        /// <param name="word"></param>
        private void AddWordToGame(string word)
        {

        }
    }
}
