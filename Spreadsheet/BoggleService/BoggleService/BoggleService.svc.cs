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
        /// <summary>
        /// Keeps track of any pending games, should only be one but kept as a dictionary in case requests get large 
        /// Dicationary key: string UserID
        /// Dicationary Value: BoggleGame 
        /// 
        /// Once a second player is found, the game is removed and then moved into the games dictionary
        /// </summary>
        private static Dictionary<string, BoggleGame> PendingGames = new Dictionary<string, BoggleGame>();

        /// <summary>
        /// Keeps track of all users
        /// Key: UserID
        /// Value; Nickname
        /// </summary>
        private static Dictionary<string, string> Users = new Dictionary<string, string>();

        /// <summary>
        /// Dictionary to represent the games in use
        /// Key: GameID (gotten from when the game is created). It is a property in BoggleGame
        /// Value: BoggleGame game 
        /// 
        /// Contains both active and completed games but NOT pending games
        /// </summary>
        private static Dictionary<string, BoggleGame> Games = new Dictionary<string, BoggleGame>();
        
        /// <summary>
        /// Lock object for server threading.
        /// </summary>
        private static object sync = new object();

        /// <summary>
        /// Value that keeps track of the amount of games created when the server was created.
        /// Used to make GameIDs
        /// </summary>
        private static int NumberOfGames = 0;

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

        /// <summary>
        /// Increments the number of games and creates a unique GameID
        /// </summary>
        private string GenerateGameID()
        {
            NumberOfGames++;
            return "G" + NumberOfGames;
        }

        /// <summary>
        /// Helper method that creates a randomly generated UserToken of the form
        /// xxxx-xxxx-xxxx-xxxx
        /// </summary>
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
