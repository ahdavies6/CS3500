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
        /// All the users that have been registered
        /// </summary>
        HashSet<User> users;

        /// <summary>
        /// All of the games the server has handled, either active or completed
        /// </summary>
        HashSet<BoggleGame> games;

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

        /// <summary>
        /// Creates a user with nickname. 
        /// 
        /// If nickname is null, or is empty when trimmed, responds with status 403 (Forbidden). 
        /// 
        /// Otherwise, creates a new user with a unique UserToken and the trimmed nickname.
        /// The returned UserToken should be used to identify the user in subsequent requests.
        /// Responds with status 201 (Created). 
        /// </summary>
        public HttpStatusCode RegisterUser(string nickname)
        {
            if (nickname == null || nickname.Trim() == null)
            {
                return HttpStatusCode.Forbidden;
            }
            else
            {
                User newUser = new User(nickname.Trim(), GenerateNewToken());
                users.Add(newUser);
                return HttpStatusCode.Created;
            }
        }

        // todo: implement this!
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
        public HttpStatusCode JoinGame(string userToken, int timeLimit)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Cancels an active join request from user userToken.
        /// 
        /// If UserToken is invalid or is not a player in the pending game, responds with status
        /// 403 (Forbidden).
        /// 
        /// Otherwise, removes UserToken from the pending game and responds with status 200 (OK).
        /// </summary>
        public HttpStatusCode CancelJoinRequest(string userToken)
        {
            throw new NotImplementedException();
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
            using (StreamReader file = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "dictionary.txt"))
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
