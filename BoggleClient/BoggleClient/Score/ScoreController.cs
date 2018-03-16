using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BoggleClient.Score
{
    class ScoreController
    {

        private IScoreView view;


        private string GameID;

        private string URL;

        private CancellationTokenSource tokenSource;

        public ScoreController(IScoreView view, string GameID, string URL)
        {
            this.view = view;
            this.GameID = GameID;
            this.URL = URL;
            this.view.CancelPushed += Cancel;

            ShowScores();
        }


        /// <summary>
        /// Given a string url, the method returns a constructed HttpClient
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private HttpClient GenerateHttpClient()
        {
            HttpClient c = new HttpClient();
            c.BaseAddress = new Uri(URL);

            c.DefaultRequestHeaders.Accept.Clear();
            c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");

            return c;
        }

        private async void ShowScores()
        {
            using (HttpClient client = GenerateHttpClient())
            {
                try
                {
                    //get response
                    string uri = string.Format("BoggleService.svc/games/{0}", this.GameID);
                    this.tokenSource = new CancellationTokenSource();
                    HttpResponseMessage response = await client.GetAsync(uri, this.tokenSource.Token);

                    //deal with response
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        dynamic gamestatus = JsonConvert.DeserializeObject(result);

                        this.view.PlayerName = (string) gamestatus.Player1.Nickname;
                        this.view.OpponentName = (string)gamestatus.Player2.Nickname;
                        this.view.PlayerScore = (int)gamestatus.Player1.Score;
                        this.view.OpponentScore = (int)gamestatus.Player2.Score;

                        //get the words played
                        //player 1
                        List<string> wordsp1 = new List<string>();
                        List<int> scoresp1 = new List<int>();
                        foreach(dynamic item in gamestatus.Player1.WordsPlayed)
                        {
                            wordsp1.Add((string)item.Word);
                            scoresp1.Add((int) item.Score);
                        }
                        this.view.PlayerScores = scoresp1.ToArray();
                        this.view.PlayerWords = wordsp1.ToArray();

                        //player 2
                        List<string> wordsp2 = new List<string>();
                        List<int> scoresp2 = new List<int>();
                        foreach (dynamic item in gamestatus.Player2.WordsPlayed)
                        {
                            wordsp2.Add((string)item.Word);
                            scoresp2.Add((int)item.Score);
                        }
                        this.view.OpponentScores = scoresp2.ToArray();
                        this.view.OpponentWords = wordsp2.ToArray();
                    }
                }
                catch (TaskCanceledException ex)
                {
                }
            }
        }

        private void Cancel()
        {
            tokenSource?.Cancel();
        }

    }
}
