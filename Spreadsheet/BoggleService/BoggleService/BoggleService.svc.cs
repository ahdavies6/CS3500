using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ServiceModel.Web;
using static System.Net.HttpStatusCode;

namespace Boggle
{
    public class BoggleService : IBoggleService
    {

        private static Dictionary<string, BoggleGame> PendingGames = new Dictionary<string, BoggleGame>();
        private static Dictionary<string, string> Users = new Dictionary<string, string>();
        private static Dictionary<string, BoggleGame> Games = new Dictionary<string, BoggleGame>();
        private static object sync = new object();

        /// <summary>
        /// The most recent call to SetStatus determines the response code used when
        /// an http response is sent.
        /// </summary>
        /// <param name="status"></param>
        private static void SetStatus(HttpStatusCode status)
        {
            WebOperationContext.Current.OutgoingResponse.StatusCode = status;
        }

        /// <summary>
        /// Returns a Stream version of index.html.
        /// </summary>
        /// <returns></returns>
        public Stream API()
        {
            SetStatus(OK);
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            return File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "index.html");
        }

        public void CancelJoinRequest(CancelJoinRequest request)
        {
            lock (sync)
            {
                PendingGames.Remove(request.UserToken);
                SetStatus(OK);
            }
        }

        public Status GetGameStatus(string GameID, string brief)
        {
            throw new NotImplementedException();
        }

        public GameIDResponse JoinGame(JoinRequest request)
        {
            throw new ArgumentException();
        }

        public ScoreResponse PlayWord(PlayWord wordRequest, string GameID)
        {
            throw new NotImplementedException();
        }

        public UserTokenResponse RegisterUser(CreateUserRequest request)
        {
            lock (sync)
            {
                if (request.Nickname is null)
                {
                    SetStatus(Forbidden);
                    return null; //valid or nah?
                                 //todo
                }

                string trimmedNickname = request.Nickname.Trim();
                if (trimmedNickname.Length == 0 || trimmedNickname.Length > 50)
                {
                    SetStatus(Forbidden);
                    return null;
                    //todo?
                }

                string token = UserTokenGenerator();

                //Get a unique token
                while (Users.ContainsKey(token))
                {
                    token = UserTokenGenerator();
                }

                Users.Add(token, trimmedNickname);

                //response to the client
                UserTokenResponse response = new UserTokenResponse();
                response.UserToken = token;
                SetStatus(Created);
                return response;
            }
        }

        private string UserTokenGenerator()
        {
            Random rand = new Random();

            string token = "";

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    token = token + rand.Next(10);
                }

                //Every iteration but the last 
                if (i != 3)
                {
                    token = token + "-";
                }
            }

            return token;
        }



        /// <summary>
        /// Demo.  You can delete this.
        /// </summary>
        public string WordAtIndex(int n)
        {
            if (n < 0)
            {
                SetStatus(Forbidden);
                return null;
            }

            string line;
            using (StreamReader file = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "dictionary.txt"))
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (n == 0) break;
                    n--;
                }
            }

            if (n == 0)
            {
                SetStatus(OK);
                return line;
            }
            else
            {
                SetStatus(Forbidden);
                return null;
            }
        }
    }
}
