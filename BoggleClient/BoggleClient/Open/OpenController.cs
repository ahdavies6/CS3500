using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BoggleClient.Open
{
    /// <summary>
    /// Contains data necessary to begin a new game
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void OpenViewEventHandler(object sender, OpenViewEventArgs e);

    /// <summary>
    /// Registers the user with a server and finds a game
    /// </summary>
    class OpenController
    {
        /// <summary>
        /// Fired when the user is ready to begin the game phase
        /// </summary>
        public event OpenViewEventHandler NextPhase;

        /// <summary>
        /// The GUI the user interacts with
        /// </summary>
        private IOpenView view;

        /// <summary>
        /// Used to cancel an active server request
        /// </summary>
        private CancellationTokenSource TokenSource;

        /// <summary>
        /// The URL of the game server
        /// </summary>
        private string URL;

        /// <summary>
        /// The game ID token
        /// </summary>
        private string GameID;

        /// <summary>
        /// The user's username
        /// </summary>
        private string Nickname;

        /// <summary>
        /// The user's token ID
        /// </summary>
        private string UserID;

        /// <summary>
        /// The desired length of the game
        /// </summary>
        private int gameLength;

        /// <summary>
        /// Whether the user has been registered by the server
        /// </summary>
        private bool Registered;

        /// <summary>
        /// Opens a new controller controlling (view)
        /// </summary>
        /// <param name="view"></param>
        public OpenController(IOpenView view)
        {
            this.view = view;
            this.Registered = false;
        }

        /// <summary>
        /// Searches for a game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void Search(object sender, SearchGameEventArgs e)
        {
            if (Registered)
            {
                try
                {
                    using (HttpClient Client = GenerateHttpClient(URL))
                    {
                        try
                        {
                            //request
                            string uri = string.Format("BoggleService.svc/games");
                            dynamic body = new ExpandoObject();
                            body.UserToken = UserID;
                            body.TimeLimit = e.GameLength;
                            TokenSource = new CancellationTokenSource();
                            StringContent content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

                            HttpResponseMessage response = await Client.PostAsync(uri, content, TokenSource.Token);


                            //dealing with response
                            if (response.IsSuccessStatusCode)
                            {
                                string result = await response.Content.ReadAsStringAsync();
                                dynamic game = JsonConvert.DeserializeObject(result);
                                this.GameID = game.GameID;
                            }

                            if (response.StatusCode == HttpStatusCode.Accepted)
                            {
                                bool foundGame = false;

                                uri = string.Format("BoggleService.svc/games/{0}?Brief=yes", this.GameID);

                                while (!foundGame)
                                {
                                    Thread.Sleep(250);

                                    HttpResponseMessage msg = await Client.GetAsync(uri, TokenSource.Token);
                                    if (msg.IsSuccessStatusCode)
                                    {
                                        string result = await msg.Content.ReadAsStringAsync();
                                        dynamic game = JsonConvert.DeserializeObject(result);
                                        if (((string)game.GameState).Equals("active"))
                                        {
                                            foundGame = true;
                                            gameLength = game.TimeLeft;
                                        }
                                    }
                                }
                            }

                            if(response.StatusCode == HttpStatusCode.Conflict)
                            {
                                //Removes from the old game and adds to a new game
                                uri = string.Format("BoggleService.svc/games");
                                body = new ExpandoObject();
                                body.UserToken = UserID;
                                TokenSource = new CancellationTokenSource();
                                content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

                                 response = await Client.PutAsync(uri, content, TokenSource.Token);
                                this.Search(sender, e);
                                return;
                            }

                            NextPhase?.Invoke(this, new OpenViewEventArgs(this.UserID, this.GameID,
                                this.Nickname, this.URL, this.gameLength));

                        }
                        catch (TaskCanceledException)
                        {

                        }
                    }
                }
                catch (BadRequestException)
                {
                    view.Registered = false;
                    view.Registering = false;
                    view.RefreshFieldAccess();
                }
            }
        }

        /// <summary>
        /// Cancels registering username or searching for a game
        /// </summary>
        public void Cancel()
        {
            this.TokenSource?.Cancel();
        }

        /// <summary>
        /// Registers the user with the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void Register(object sender, ConnectEventArgs e)
        {
            this.URL = e.URL;
            this.Nickname = e.Nickname;
            try
            {
                using (HttpClient Client = GenerateHttpClient(e.URL))
                {
                    try
                    {
                        //request
                        string uri = string.Format("BoggleService.svc/users");
                        dynamic body = new ExpandoObject();
                        body.Nickname = e.Nickname;
                        TokenSource = new CancellationTokenSource();
                        StringContent content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await Client.PostAsync(uri, content, TokenSource.Token);


                        //dealing with response
                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            dynamic register = JsonConvert.DeserializeObject(result);
                            this.UserID = (string)register.UserToken;
                            this.Registered = true;

                            view.Registering = false;
                            view.Registered = true;
                            view.RefreshFieldAccess();
                        }
                        else
                        {
                            view.Registering = false;
                            view.Registered = false;
                            view.RefreshFieldAccess();
                            view.ShowNameRegistrationMsg(this.Nickname, this.URL);
                        }

                    }
                    catch (TaskCanceledException ex)
                    {

                    }
                }
            }
            catch (BadRequestException)
            {
                view.Registered = false;
                view.Registering = false;
                view.RefreshFieldAccess();
            }
        }

        /// <summary>
        /// Returns an HttpClient to interact with the server
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private HttpClient GenerateHttpClient(string url)
        {
            try
            {
                HttpClient c = new HttpClient();
                c.BaseAddress = new Uri(url);

                c.DefaultRequestHeaders.Accept.Clear();
                c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");

                return c;
            }
            catch (UriFormatException)
            {
                throw new BadRequestException();
            }
        }
    }

    /// <summary>
    /// Thrown when the user attempts to contact an invalid server
    /// </summary>
    public class BadRequestException : Exception
    {

    }

    /// <summary>
    /// The data necessary to begin a game
    /// </summary>
    public class OpenViewEventArgs : EventArgs
    {
        public string UserID { get; private set; }

        public string GameID { get; private set; }

        public string Nickname { get; private set; }

        public string URL { get; private set; }

        public int GameLength { get; private set; }

        public OpenViewEventArgs(string userid, string gameid, string nickname, string url, int gameLength)
        {
            this.UserID = userid;
            this.GameID = gameid;
            this.Nickname = nickname;
            this.URL = url;
            this.GameLength = gameLength;
        }

    }
}
