using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ServiceModel.Web;
using System.Net.Http;
using static System.Net.HttpStatusCode;

namespace Boggle
{
    public class BoggleService : IBoggleService
    {
        /// <summary>
        /// Keeps track of any pending games, should only be one but kept as a dictionary in case requests get large 
        /// Dicationary key: string UserToken
        /// Dicationary Value: BoggleGame 
        /// 
        /// Once a second player is found, the game is removed and then moved into the games dictionary
        /// </summary>
        private static Dictionary<string, BoggleGame> PendingGames = new Dictionary<string, BoggleGame>();

        /// <summary>
        /// Keeps track of all users
        /// Key: UserToken
        /// Value: Nickname
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

        /// <summary>
        /// Creates a user with nickname. 
        /// 
        /// If nickname is null, or is empty when trimmed, responds with status 403 (Forbidden). 
        /// 
        /// Otherwise, creates a new user with a unique UserToken and the trimmed nickname.
        /// The returned UserToken should be used to identify the user in subsequent requests.
        /// Responds with status 201 (Created). 
        /// </summary>
        public void CancelJoinRequest(CancelJoinRequest request)
        {
            lock (sync)
            {
                PendingGames.Remove(request.UserToken);
                SetStatus(OK);
            }
        }

        /// <summary>
        /// Returns the status of game with ID gameID
        /// </summary>
        public Status GetGameStatus(string gameID, string brief)
        {
            //if (nickname == null || nickname.Trim() == null)
            //{
            //    return HttpStatusCode.Forbidden;
            //}
            //else
            //{
            //    User newUser = new User(nickname.Trim(), GenerateNewToken());
            //    users.Add(newUser);
            //    return HttpStatusCode.Created;
            //}

            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates a new, unique UserToken.
        /// </summary>
        /// <returns></returns>
        private string GenerateNewToken()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Attempts to join a game with user userToken and timeLimit
        /// 
        /// If UserToken is invalid, TimeLimit is less than 5, or TimeLimit is over 120, responds
        /// with status 403 (Forbidden).
        /// 
        /// Otherwise, if UserToken is already a player in the pending game, responds with status
        /// 409 (Conflict).
        /// 
        /// Otherwise, if there is already one player in the pending game, adds UserToken as the
        /// second player. The pending game becomes active and a new pending game with no players
        /// is created. The active game's time limit is the integer average of the time limits
        /// requested by the two players. Returns the new active game's GameID (which should be the
        /// same as the old pending game's GameID). Responds with status 201 (Created).
        /// 
        /// Otherwise, adds UserToken as the first player of the pending game, and the TimeLimit as
        /// the pending game's requested time limit. Returns the pending game's GameID. Responds with
        /// status 202 (Accepted).
        /// </summary>
        public GameIDResponse JoinGame(JoinRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Plays word to game gameID
        /// </summary>
        /// <param name="wordRequest"></param>
        /// <param name="gameID"></param>
        /// <returns></returns>
        public ScoreResponse PlayWord(PlayWord wordRequest, string gameID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Registers new user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public UserTokenResponse RegisterUser(CreateUserRequest request)
        {
            lock (sync)
            {
                if (request.Nickname is null)
                {
                    SetStatus(Forbidden);
                    return null; // todo: see if this works right
                }

                string trimmedNickname = request.Nickname.Trim();
                if (trimmedNickname.Length == 0 || trimmedNickname.Length > 50)
                {
                    SetStatus(Forbidden);
                    return null;
                    // todo: what else needs to be done here?
                }

                string token = UserTokenGenerator();

                // Get a unique token
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
        /// Generates a new, unique user token
        /// </summary>
        private string UserTokenGenerator()
        {
            Random rand = new Random();
            string token = "";

            do
            {
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
            } while (Users.ContainsKey(token));

            return token;
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
        /// Plays word under userToken in game GameID.
        /// 
        /// If Word is null or empty when trimmed, or if GameID or UserToken is missing or invalid,
        /// or if UserToken is not a player in the game identified by GameID, responds with
        /// response code 403 (Forbidden).
        /// 
        /// Otherwise, if the game state is anything other than "active", responds with response code
        /// 409 (Conflict).
        /// 
        /// Otherwise, records the trimmed Word as being played by UserToken in the game identified by
        /// GameID. Returns the score for Word in the context of the game (e.g. if Word has been played
        /// before the score is zero). Responds with status 200 (OK).
        /// Note: The word is not case sensitive.
        /// </summary>
        public HttpStatusCode PlayWord(string userToken, string word, string GameID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the game status of game GameID
        /// 
        /// If GameID is invalid, responds with status 403 (Forbidden).
        /// 
        /// Otherwise, returns information about the game named by GameID as illustrated below. Note that
        /// the information returned depends on whether "Brief=yes" was included as a parameter as well as
        /// on the state of the game. Responds with status code 200 (OK). Note: The Board and Words are
        /// not case sensitive.
        /// </summary>
        public HttpStatusCode GetGameStatus(string GameID, bool brief)
        {
            throw new NotImplementedException();
        }
    }

    public class User
    {
        /// <summary>
        /// The user's nickname
        /// </summary>
        public string Nickname { get; private set; }

        /// <summary>
        /// The user's token
        /// </summary>
        public string UserToken { get; private set; }

        /// <summary>
        /// Creates a new RegisteredUser with nickname and userToken
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="userToken"></param>
        public User(string nickname, string userToken)
        {
            Nickname = nickname;
            UserToken = userToken;
        }
    }
}
