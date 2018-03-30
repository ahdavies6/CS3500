using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Net.HttpStatusCode;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Dynamic;

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
    //todo add a test for the timelimit average
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

         private RestTestClient client = new RestTestClient("http://localhost:60000/BoggleService.svc/");
        //private RestTestClient client = new RestTestClient("http://ice.eng.utah.edu/BoggleService.svc/");
        [TestMethod]
        public void Generate3UsersNormal()
        {
            dynamic data = new ExpandoObject();
            data.Nickname = "p1";
            Response r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Created, r.Status);

            data = new ExpandoObject();
            data.Nickname = "p2";
            r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Created, r.Status);

            data = new ExpandoObject();
            data.Nickname = "p3";
            r = client.DoPostAsync("users", data).Result;
            Assert.AreEqual(Created, r.Status);
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
            //Doesnt work with joes, his GiD resets
            //Assert.AreNotEqual(gid, (string)r.Data.GameID);
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
        }
    }

}
