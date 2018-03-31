using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Net.HttpStatusCode;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Dynamic;
using System.Threading;
using Boggle;
using System.IO;
using System.Collections.Generic;

namespace Boggle
{
    /// <summary>
    /// Provides a way to start and stop the IIS web server from within the test
    /// cases.  If something prevents the test cases from stopping the web server,
    /// subsequent tests may not work properly until the stray process is killed
    /// manually.
    /// </summary>
    public static class IISAgent
    {
        // Reference to the running process
        private static Process process = null;

        /// <summary>
        /// Starts IIS
        /// </summary>
        public static void Start(string arguments)
        {
            if (process == null)
            {
                ProcessStartInfo info = new ProcessStartInfo(Properties.Resources.IIS_EXECUTABLE, arguments);
                info.WindowStyle = ProcessWindowStyle.Minimized;
                info.UseShellExecute = false;
                process = Process.Start(info);
            }
        }

        /// <summary>
        ///  Stops IIS
        /// </summary>
        public static void Stop()
        {
            if (process != null)
            {
                process.Kill();
            }
        }
    }

    [TestClass]
    public class BoggleTests
    {
        /// <summary>
        /// This is automatically run prior to all the tests to start the server
        /// </summary>
        [ClassInitialize()]
        public static void StartIIS(TestContext testContext)
        {
            IISAgent.Start(@"/site:""BoggleService"" /apppool:""Clr4IntegratedAppPool"" /config:""..\..\..\.vs\config\applicationhost.config""");
        }

        /// <summary>
        /// This is automatically run when all tests have completed to stop the server
        /// </summary>
        [ClassCleanup()]
        public static void StopIIS()
        {
            IISAgent.Stop();
        }

        /// <summary>
        /// The client which will be used in all of the tests
        /// </summary>
        private RestTestClient client = new RestTestClient("http://localhost:60000/BoggleService.svc/");

        [TestMethod]
        public void Generate3UsersNormal()
        {
            dynamic data = new ExpandoObject();
            data.Nickname = "p1";
            Response r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Created, r.Status);
            Assert.IsNotNull(r.Data.UserToken);

            data = new ExpandoObject();
            data.Nickname = "p2";
            r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Created, r.Status);
            Assert.IsNotNull(r.Data.UserToken);


            data = new ExpandoObject();
            data.Nickname = "p3";
            r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Created, r.Status);
            Assert.IsNotNull(r.Data.UserToken);
        }

        [TestMethod]
        public void TestForbiddenNames()
        {
            dynamic data = new ExpandoObject();
            data.Nickname = null;
            Response r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Forbidden, r.Status);

            data = new ExpandoObject();
            data.Nickname = "";
            r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Forbidden, r.Status);

            data = new ExpandoObject();
            data.Nickname = "     ";
            r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Forbidden, r.Status);
        }

        [TestMethod]
        public void JoinGameValidTest()
        {
            dynamic data = new ExpandoObject();
            data.Nickname = "p1";
            Response r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Created, r.Status);
            string p1token = (string)r.Data.UserToken;

            data = new ExpandoObject();
            data.Nickname = "p2";
            r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Created, r.Status);
            string p2token = (string)r.Data.UserToken;

            //add p1
            data = new ExpandoObject();
            data.UserToken = p1token;
            data.TimeLimit = 25;
            r = client.DoPostAsync("games", data).Result;
            Assert.AreEqual(Accepted, r.Status);
            string gid = (string)r.Data.GameID;

            //add p2
            data = new ExpandoObject();
            data.UserToken = p2token;
            data.TimeLimit = 25;
            r = client.DoPostAsync("games", data).Result;
            Assert.AreEqual(Created, r.Status);
            Assert.AreEqual(gid, (string)r.Data.GameID);
        }

        [TestMethod]
        public void InvalidJoinRequestTest()
        {
            dynamic data = new ExpandoObject();
            data.Nickname = "p1";
            Response r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Created, r.Status);
            string p1token = (string)r.Data.UserToken;

            data = new ExpandoObject();
            data.Nickname = "p2";
            r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Created, r.Status);
            string p2token = (string)r.Data.UserToken;

            //TimeLimit < 5
            data = new ExpandoObject();
            data.UserToken = p1token;
            data.TimeLimit = 4;
            r = client.DoPostAsync("games", data).Result;
            Assert.AreEqual(Forbidden, r.Status);

            //TimeLimit > 120
            data = new ExpandoObject();
            data.UserToken = p1token;
            data.TimeLimit = 121;
            r = client.DoPostAsync("games", data).Result;
            Assert.AreEqual(Forbidden, r.Status);
        }

        [TestMethod]
        public void JoinGameCoverage1()
        {
            dynamic data = new ExpandoObject();
            data.Nickname = "p1";
            Response r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Created, r.Status);
            string p1token = (string)r.Data.UserToken;

            data = new ExpandoObject();
            data.UserToken = p1token;
            data.TimeLimit = 60;
            r = client.DoPostAsync("games", data).Result;
            Assert.AreEqual(Accepted, r.Status);

            r = client.DoPostAsync("games", data).Result;
            Assert.AreEqual(Conflict, r.Status);
        }

        [TestMethod]
        public void JoinGameCoverage2()
        {
            dynamic data = new ExpandoObject();
            data.UserToken = "problem";
            data.TimeLimit = 60;
            Response r = client.DoPostAsync("games", data).Result;
            Assert.AreEqual(Forbidden, r.Status);
        }

        [TestMethod]
        public void CancelJoinValidTest()
        {
            dynamic data = new ExpandoObject();
            data.Nickname = "p1";
            Response r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Created, r.Status);
            string p1token = (string)r.Data.UserToken;

            data = new ExpandoObject();
            data.Nickname = "p2";
            r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Created, r.Status);
            string p2token = (string)r.Data.UserToken;

            //p1 request to join
            data = new ExpandoObject();
            data.UserToken = p1token;
            data.TimeLimit = 25;
            r = client.DoPostAsync("games", data).Result;
            Assert.AreEqual(Accepted, r.Status);
            string gid = (string)r.Data.GameID;

            //p1 canceling request
            data = new ExpandoObject();
            data.UserToken = p1token;
            r = client.DoPutAsync("games", data).Result;
            Assert.AreEqual(OK, r.Status);

            //p2 requesting a new game
            data = new ExpandoObject();
            data.UserToken = p2token;
            data.TimeLimit = 25;
            r = client.DoPostAsync("games", data).Result;
            Assert.AreEqual(Accepted, r.Status);
            Assert.AreEqual(gid, (string)r.Data.GameID);
            gid = (string)r.Data.GameID;

            //p1 joining the new game
            data = new ExpandoObject();
            data.UserToken = p1token;
            data.TimeLimit = 25;
            r = client.DoPostAsync("games", data).Result;
            Assert.AreEqual(Created, r.Status);
            Assert.AreEqual(gid, (string)r.Data.GameID);
        }

        [TestMethod]
        public void CancelJoinRequestInvalid()
        {

            dynamic data = new ExpandoObject();
            data.Nickname = "p1";
            Response r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Created, r.Status);
            string p1token = (string)r.Data.UserToken;

            data = new ExpandoObject();
            data.Nickname = "p2";
            r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Created, r.Status);
            string p2token = (string)r.Data.UserToken;

            //p1 request to join
            data = new ExpandoObject();
            data.UserToken = p1token;
            data.TimeLimit = 25;
            r = client.DoPostAsync("games", data).Result;
            Assert.AreEqual(Accepted, r.Status);
            string gid = (string)r.Data.GameID;

            //p1 canceling request - chopped of user token
            data = new ExpandoObject();
            data.UserToken = p1token[0];
            r = client.DoPutAsync("games", data).Result;
            Assert.AreEqual(Forbidden, r.Status);

            //p2 is not a player in any game
            data = new ExpandoObject();
            data.UserToken = p2token;
            r = client.DoPutAsync("games", data).Result;
            Assert.AreEqual(Forbidden, r.Status);

            //Resets server so p1 has cancelled request
            //p1 canceling request
            data = new ExpandoObject();
            data.UserToken = p1token;
            r = client.DoPutAsync("games", data).Result;
            Assert.AreEqual(OK, r.Status);
        }

        /// <summary>
        /// Helper method finds valid words, given a game board
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public Dictionary<int, LinkedList<string>> GetValidWords(BoggleBoard board, int desiredNumWords)
        {
            Dictionary<int, LinkedList<string>> validWords = new Dictionary<int, LinkedList<string>>();
            int timesChecked = 0;

            using (StreamReader reader = new StreamReader("dictionary.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null && timesChecked < desiredNumWords)
                {
                    if (board.CanBeFormed(line))
                    {
                        if (!validWords.ContainsKey(line.Length))
                        {
                            validWords.Add(line.Length, new LinkedList<string>());
                        }

                        validWords[line.Length].AddLast(line);
                        desiredNumWords++;
                    }
                }
            }

            return validWords;
        }

        [TestMethod]
        public void PlayWordValid()
        {
            dynamic data = new ExpandoObject();
            data.Nickname = "p1";
            Response r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Created, r.Status);
            string p1token = (string)r.Data.UserToken;

            data = new ExpandoObject();
            data.Nickname = "p2";
            r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Created, r.Status);
            string p2token = (string)r.Data.UserToken;

            //add p1
            data = new ExpandoObject();
            data.UserToken = p1token;
            data.TimeLimit = 15;
            r = client.DoPostAsync("games", data).Result;
            Assert.AreEqual(Accepted, r.Status);
            string gid = (string)r.Data.GameID;

            //add p2
            data = new ExpandoObject();
            data.UserToken = p2token;
            data.TimeLimit = 15;
            r = client.DoPostAsync("games", data).Result;
            Assert.AreEqual(Created, r.Status);
            Assert.AreEqual(gid, (string)r.Data.GameID);

            //Get the board to form words
            r = client.DoGetAsync("games/" + gid).Result;
            Assert.AreEqual(OK, r.Status);
            BoggleBoard board = new BoggleBoard((string)r.Data.Board);
            Dictionary<int, LinkedList<string>> validWords = GetValidWords(board, 100);

            //test each scoring method
            foreach (int key in validWords.Keys)
            {
                data = new ExpandoObject();
                data.UserToken = p1token;
                data.Word = validWords[key].First.Value;
                r = client.DoPutAsync("games/" + gid, data).Result;
                Assert.AreEqual(OK, r.Status);
                if (key < 3)
                {
                    Assert.AreEqual(0, (int)r.Data.Score);
                }
                else if (key == 3 || key == 4)
                {
                    Assert.AreEqual(1, (int)r.Data.Score);
                }
                else if (key == 5 || key == 6)
                {
                    Assert.AreEqual(key - 3, (int)r.Data.Score);
                }
                else if (key == 7)
                {
                    Assert.AreEqual(5, (int)r.Data.Score);
                }
                else if (key > 7)
                {
                    Assert.AreEqual(11, (int)r.Data.Score);
                }

            }

            //Test an -1 score word
            data = new ExpandoObject();
            data.UserToken = p1token;
            data.Word = "invalidword0";
            r = client.DoPutAsync("games/" + gid, data).Result;
            Assert.AreEqual(OK, r.Status);
            Assert.AreEqual(-1, (int)r.Data.Score);
        }

        [TestMethod]
        public void TestPlayWordMultiple()
        {
            for (int i = 0; i < 4; i++)
            {
                PlayWordValid();
            }
        }

        [TestMethod]
        public void TestPlayWordErrors()
        {
            dynamic data = new ExpandoObject();
            data.Nickname = "p1";
            Response r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Created, r.Status);
            string p1token = (string)r.Data.UserToken;

            data = new ExpandoObject();
            data.Nickname = "p2";
            r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Created, r.Status);
            string p2token = (string)r.Data.UserToken;

            //add p1
            data = new ExpandoObject();
            data.UserToken = p1token;
            data.TimeLimit = 10;
            r = client.DoPostAsync("games", data).Result;
            Assert.AreEqual(Accepted, r.Status);
            string gid = (string)r.Data.GameID;

            //add p2
            data = new ExpandoObject();
            data.UserToken = p2token;
            data.TimeLimit = 10;
            r = client.DoPostAsync("games", data).Result;
            Assert.AreEqual(Created, r.Status);
            Assert.AreEqual(gid, (string)r.Data.GameID);

            //Null word
            data = new ExpandoObject();
            data.UserToken = p1token;
            data.Word = null;
            r = client.DoPutAsync("games/" + gid, data).Result;
            Assert.AreEqual(Forbidden, r.Status);

            //trimmed empty word
            data = new ExpandoObject();
            data.UserToken = p1token;
            data.Word = "    ";
            r = client.DoPutAsync("games/" + gid, data).Result;
            Assert.AreEqual(Forbidden, r.Status);

            // empty word
            data = new ExpandoObject();
            data.UserToken = p1token;
            data.Word = "    ";
            r = client.DoPutAsync("games/" + gid, data).Result;
            Assert.AreEqual(Forbidden, r.Status);

            //Someone not in the game
            data = new ExpandoObject();
            data.UserToken = "I_am_not_a_player";
            data.Word = "word";
            r = client.DoPutAsync("games/" + gid, data).Result;
            Assert.AreEqual(Forbidden, r.Status);

            Thread.Sleep(10000);

            //Expired game
            data = new ExpandoObject();
            data.UserToken = p1token;
            data.Word = "word";
            r = client.DoPutAsync("games/" + gid, data).Result;
            Assert.AreEqual(Conflict, r.Status);
        }

        [TestMethod]
        public void PlayWordCoverage()
        {
            dynamic data = new ExpandoObject();
            data.Nickname = "p1";
            Response r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Created, r.Status);
            string p1token = (string)r.Data.UserToken;

            data = new ExpandoObject();
            data.Nickname = "p2";
            r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Created, r.Status);
            string p2token = (string)r.Data.UserToken;

            data = new ExpandoObject();
            data.Nickname = "p3";
            r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Created, r.Status);
            string p3token = (string)r.Data.UserToken;

            data = new ExpandoObject();
            data.UserToken = p1token;
            data.TimeLimit = 10;
            r = client.DoPostAsync("games", data).Result;
            Assert.AreEqual(Accepted, r.Status);
            string gid = (string)r.Data.GameID;

            data = new ExpandoObject();
            data.UserToken = p2token;
            data.TimeLimit = 10;
            r = client.DoPostAsync("games", data).Result;
            Assert.AreEqual(Created, r.Status);
            Assert.AreEqual(gid, (string)r.Data.GameID);

            r = client.DoGetAsync("games/" + gid).Result;
            Assert.AreEqual(OK, r.Status);
            BoggleBoard board = new BoggleBoard((string)r.Data.Board);
            Dictionary<int, LinkedList<string>> validWords = GetValidWords(board, 1);

            foreach (int key in validWords.Keys)
            {
                data = new ExpandoObject();
                data.UserToken = p3token;
                data.Word = validWords[key].First.Value;
                r = client.DoPutAsync("games/" + gid, data).Result;
                Assert.AreEqual(Forbidden, r.Status);
            }
        }

        [TestMethod]
        public void TestGetStatus()
        {
            dynamic data = new ExpandoObject();
            data.Nickname = "p1";
            Response r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Created, r.Status);
            string p1token = (string)r.Data.UserToken;

            data = new ExpandoObject();
            data.Nickname = "p2";
            r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Created, r.Status);
            string p2token = (string)r.Data.UserToken;

            //add p1
            data = new ExpandoObject();
            data.UserToken = p1token;
            data.TimeLimit = 15;
            r = client.DoPostAsync("games", data).Result;
            Assert.AreEqual(Accepted, r.Status);
            string gid = (string)r.Data.GameID;

            //pending get status
            r = client.DoGetAsync("games/{0}?Brief=no", gid).Result;
            Assert.AreEqual(OK, r.Status);
            Assert.AreEqual("pending", (string)r.Data.GameState);

            //add p2
            data = new ExpandoObject();
            data.UserToken = p2token;
            data.TimeLimit = 15;
            r = client.DoPostAsync("games", data).Result;
            Assert.AreEqual(Created, r.Status);
            Assert.AreEqual(gid, (string)r.Data.GameID);

            //Get the board to form words
            r = client.DoGetAsync("games/" + gid).Result;
            Assert.AreEqual(OK, r.Status);
            BoggleBoard board = new BoggleBoard((string)r.Data.Board);
            Dictionary<int, LinkedList<string>> validWords = GetValidWords(board, 1000);

            int totalScore = 0;
            IList<string> wordsadded = new List<string>();
            IList<int> scores = new List<int>();

            //test each scoring method
            foreach (KeyValuePair<int, LinkedList<string>> pair in validWords)
            {
                int length = pair.Key;
                LinkedList<string> words = pair.Value;

                data = new ExpandoObject();
                data.UserToken = p1token;
                data.Word = words.First.Value;
                r = client.DoPutAsync("games/" + gid, data).Result;
                Assert.AreEqual(OK, r.Status);
                wordsadded.Add(validWords[length].First.Value);

                if (length == 3 || length == 4)
                {
                    totalScore++;
                    scores.Add(1);
                }
                else if (length == 5 || length == 6)
                {
                    totalScore += length - 3;
                    scores.Add(length - 3);

                }
                else if (length == 7)
                {
                    totalScore += 5;
                    scores.Add(5);

                }
                else if (length > 7)
                {
                    totalScore += 11;
                    scores.Add(11);

                }
                else
                {
                    scores.Add(0);
                }
            }

            //Test an -1 score word
            data = new ExpandoObject();
            data.UserToken = p1token;
            data.Word = "invalidword0";
            r = client.DoPutAsync("games/" + gid, data).Result;
            Assert.AreEqual(OK, r.Status);
            Assert.AreEqual(-1, (int)r.Data.Score);
            totalScore--;
            wordsadded.Add("invalidword0");
            scores.Add(-1);

            //testing a full status when active and brief=yes
            r = client.DoGetAsync("games/" + gid + "?Brief=yes").Result;
            Assert.AreEqual(OK, r.Status);
            Assert.AreEqual("active", (string)r.Data.GameState);
            Assert.AreEqual(totalScore, (int)r.Data.Player1.Score);
            Assert.AreEqual(0, (int)r.Data.Player2.Score);

            //testing a full status when active and brief=no
            r = client.DoGetAsync("games/" + gid).Result;
            Assert.AreEqual(OK, r.Status);
            Assert.AreEqual("active", (string)r.Data.GameState);
            Assert.AreEqual(15, (int)r.Data.TimeLimit);
            Assert.AreEqual(totalScore, (int)r.Data.Player1.Score);
            Assert.AreEqual(0, (int)r.Data.Player2.Score);
            Assert.AreEqual("p1", (string)r.Data.Player1.Nickname);
            Assert.AreEqual("p2", (string)r.Data.Player2.Nickname);

            Thread.Sleep(15000);

            //testing a full status when comppleted and brief=yes
            r = client.DoGetAsync("games/" + gid + "?Brief=yes").Result;
            Assert.AreEqual(OK, r.Status);
            Assert.AreEqual("completed", (string)r.Data.GameState);
            Assert.AreEqual(totalScore, (int)r.Data.Player1.Score);
            Assert.AreEqual(0, (int)r.Data.Player2.Score);

            //testing a full status when completed and brief=no
            r = client.DoGetAsync("games/" + gid).Result;
            Assert.AreEqual(OK, r.Status);
            Assert.AreEqual("completed", (string)r.Data.GameState);
            Assert.AreNotEqual(0, (int)r.Data.Player1.Score);
        }
    }
}
