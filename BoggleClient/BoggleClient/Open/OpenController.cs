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
    public delegate void OpenViewEventHandler(object sender, OpenViewEventArgs e);

    class OpenController
    {
        public event OpenViewEventHandler NextPhase;

        private IOpenView view;

        private CancellationTokenSource TokenSource;

        private string URL;

        private string GameID;

        private string Nickname;

        private string UserID;

        private int gameLength;

        private bool Registered;

        public OpenController(IOpenView view)
        {
            this.view = view;
            this.Registered = false;
        }

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

        public void Cancel()
        {
            this.TokenSource?.Cancel();
        }

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

    public class BadRequestException : Exception
    {

    }

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
