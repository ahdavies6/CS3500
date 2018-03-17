using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace BoggleClient.Game
{
    /// <summary>
    /// Handles data and server interaction for the game phase of the boggle game client
    /// </summary>
    class GameController
    {
        /// <summary>
        /// Fired when the player connects with a server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void GamePhaseEventHandler(object sender, GamePhaseEventArgs e);

        /// <summary>
        /// Fired when the client is ready to move into the game phase
        /// </summary>
        public event GamePhaseEventHandler NextPhase;

        /// <summary>
        /// String that keeps track of the Users unique ID
        /// </summary>
        private string userID;

        /// <summary>
        /// HttpClient that connects with the boggle server
        /// </summary>
        private string URL;

        /// <summary>
        /// The game token ID
        /// </summary>
        private string gameID;

        /// <summary>
        /// The player's username
        /// </summary>
        private string nickname;

        /// <summary>
        /// Allows cancelling a request
        /// </summary>
        private CancellationTokenSource tokenSource;

        /// <summary>
        /// Controls the view of the board with an interface
        /// </summary>
        private IGameView view;

        /// <summary>
        /// Whether the game is still running on the server
        /// </summary>
        bool active;

        /// <summary>
        /// Constructs a GameController for game. The UID passed is the ID of the player and the URL is the URL to 
        /// connect to the server
        /// </summary>
        public GameController(string URL, string nickname, string userID, string gameID, IGameView view)
        {
            this.userID = userID;
            this.view = view;
            this.URL = URL;
            this.nickname = nickname;
            this.gameID = gameID;
            this.active = true;

            Refresh(true);
        }

        /// <summary>
        /// Cancels a request to the server
        /// </summary>
        public void Cancel()
        {
            tokenSource?.Cancel();
        }

        /// <summary>
        /// Method that refreshes what is seen on the Boggle Board GUI
        /// </summary>
        public async void Refresh(bool RefreshLabels)
        {
            if (active)
            {
                using (HttpClient client = GenerateHttpClient())
                {
                    try
                    {
                        //generate request
                        string uri = string.Format("BoggleService.svc/games/{0}", this.gameID);
                        tokenSource = new CancellationTokenSource();

                        HttpResponseMessage response = await client.GetAsync(uri, tokenSource.Token);

                        //update the display
                        if (response.IsSuccessStatusCode)
                        {
                            string responseResult = await response.Content.ReadAsStringAsync();
                            dynamic game = JsonConvert.DeserializeObject(responseResult);

                            if (((string)game.GameState).Equals("completed"))
                            {
                                NextPhase?.Invoke(this,
                                    new GamePhaseEventArgs(this.userID, this.gameID, this.nickname, this.URL));
                                this.active = false;
                            }
                            else if (!((string)game.GameState).Equals("pending"))
                            {
                                this.view.TimeRemaining = (int)game.TimeLeft;
                                
                                //If specified to refresh the labels
                                if (RefreshLabels)
                                {
                                    view.GenerateLabels((string)game.Board);
                                }

                                //set the scores
                                if (((string)game.Player1.Nickname).Equals(this.nickname))
                                {
                                    this.view.PlayerName = (string)game.Player1.Nickname;
                                    this.view.OpponentName = (string)game.Player2.Nickname;
                                    this.view.OpponentScore = (int)game.Player2.Score;
                                    this.view.PlayerScore = (int)game.Player1.Score;
                                }
                                else
                                {
                                    this.view.PlayerName = (string)game.Player2.Nickname;
                                    this.view.OpponentName = (string)game.Player1.Nickname;
                                    this.view.OpponentScore = (int)game.Player1.Score;
                                    this.view.PlayerScore = (int)game.Player2.Score;
                                }

                            }
                        }

                    }
                    catch (TaskCanceledException)
                    {

                    }
                }
            }
        }

        /// <summary>
        /// Returns a constructed HttpClient
        /// </summary>
        private HttpClient GenerateHttpClient()
        {
            HttpClient c = new HttpClient();
            c.BaseAddress = new Uri(URL);

            c.DefaultRequestHeaders.Accept.Clear();
            c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");

            return c;
        }

        /// <summary>
        /// Given a string word, attempts to add it to the boggle server
        /// </summary>
        /// <param name="word"></param>
        public async void AddWordToGame(object sender, AddWordEventArgs args)
        {
            string word = args.Word;
            using (HttpClient client = GenerateHttpClient())
            {
                try
                {
                    //Sending the request
                    string uri = String.Format("BoggleService.svc/games/{0}", gameID);
                    tokenSource = new CancellationTokenSource();

                    dynamic play = new ExpandoObject();
                    play.UserToken = this.userID;
                    play.Word = word;

                    StringContent content = new StringContent(JsonConvert.SerializeObject(play), Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync(uri, content, tokenSource.Token);

                    //deal with response
                    if (response.StatusCode == HttpStatusCode.Conflict)
                    {
                        NextPhase?.Invoke(this,
                            new GamePhaseEventArgs(this.userID, this.gameID, this.nickname, this.URL));
                    }

                }
                catch (TaskCanceledException)
                {

                }
            }
        }
    }

    /// <summary>
    /// Contains all the data necessary to enter the game phase
    /// </summary>
    class GamePhaseEventArgs : EventArgs
    {
        /// <summary>
        /// User token ID
        /// </summary>
        public string UserID { get; private set; }

        /// <summary>
        /// Game token ID
        /// </summary>
        public string GameID { get; private set; }

        /// <summary>
        /// Player's username
        /// </summary>
        public string Nickname { get; private set; }

        /// <summary>
        /// Server URL
        /// </summary>
        public string URL { get; private set; }

        /// <summary>
        /// Creates GamePhaseEventArgs with data necessary to begin and run a game
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="gameid"></param>
        /// <param name="nickname"></param>
        /// <param name="url"></param>
        public GamePhaseEventArgs(string uid, string gameid, string nickname, string url)
        {
            this.UserID = uid;
            this.GameID = gameid;
            this.Nickname = nickname;
            this.URL = url;
        }
    }
}
