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
    //TODO doc comments
    class GameController
    {

        public event Action<GamePhaseArgs> NextPhase;

        /// <summary>
        /// String that keeps track of the Users unique ID
        /// </summary>
        private string UserID;

        /// <summary>
        /// HttpClient that connects with the boggle server
        /// </summary>
        // TODO doc comments
        private string URL;

        private string userID;

        private string gameID;

        private string nickname;

        private CancellationTokenSource tokenSource;

        /// <summary>
        /// Controls the view of the board with an interface
        /// </summary>
        private Game.IGameView view;

        /// <summary>
        /// Constructs a GameController for game. The UID passed is the ID of the player and the URL is the URL to 
        /// connect to the server
        /// </summary>
        public GameController(string URL, string nickname, string userID, string gameID, Game.IGameView view)
        {
            this.UserID = userID;
            this.view = view;
            this.URL = URL;
            this.nickname = nickname;
            this.gameID = gameID;
            this.view.AddWord += AddWordToGame;
            this.view.CancelPushed += Cancel;
        }

        private void Cancel()
        {
            tokenSource?.Cancel();
        }

        /// <summary>
        /// Method that refreshes what is seen on the Boggle Board GUI
        /// </summary>
        public async void Refresh()
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
                            NextPhase?.Invoke(new GamePhaseArgs(this.userID, this.gameID, this.nickname, this.URL));
                        }
                        else
                        {
                            this.view.TimeRemaining = (int)game.TimeLeft;

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
                catch (TaskCanceledException ex)
                {

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
        private async void AddWordToGame(AddWordEventArgs args)
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
                        NextPhase?.Invoke(new GamePhaseArgs(this.userID, this.gameID, this.nickname, this.URL));
                    }

                }
                catch (TaskCanceledException ex)
                {

                }
            }

        }
    }

    class GamePhaseArgs : EventArgs
    {
        public string UserID { get; private set; }

        public string GameID { get; private set; }

        public string Nickname { get; private set; }

        public string URL { get; private set; }

        public GamePhaseArgs(string uid, string gameid, string nickname, string url)
        {
            this.UserID = uid;
            this.GameID = gameid;
            this.Nickname = nickname;
            this.URL = url;
        }
    }
}
