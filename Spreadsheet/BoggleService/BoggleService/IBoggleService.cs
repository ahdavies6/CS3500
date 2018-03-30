using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Net;

namespace Boggle
{
    [ServiceContract]
    public interface IBoggleService
    {
        /// <summary>
        /// Sends back index.html as the response body.
        /// </summary>
        [WebGet(UriTemplate = "/api")]
        Stream API();

        /// <summary>
        /// Creates a user with nickname. 
        /// 
        /// If nickname is null, or is empty when trimmed, responds with status 403 (Forbidden). 
        /// 
        /// Otherwise, creates a new user with a unique UserToken and the trimmed nickname.
        /// The returned UserToken should be used to identify the user in subsequent requests.
        /// Responds with status 201 (Created). 
        /// </summary>
        [WebInvoke(Method = "POST", UriTemplate = "/users")]
        UserTokenResponse RegisterUser(CreateUserRequest request);

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
        [WebInvoke(Method = "POST", UriTemplate = "/games")]
        GameIDResponse JoinGame(JoinRequest request);

        /// <summary>
        /// Cancels an active join request from user userToken.
        /// 
        /// If UserToken is invalid or is not a player in the pending game, responds with status
        /// 403 (Forbidden).
        /// 
        /// Otherwise, removes UserToken from the pending game and responds with status 200 (OK).
        /// </summary>
        [WebInvoke(Method = "PUT", UriTemplate = "/games")]
        void CancelJoinRequest(CancelJoinRequest request);

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
        [WebInvoke(Method = "PUT", UriTemplate = "/games/{GameID}")]
        ScoreResponse PlayWord(PlayWord wordRequest, string GameID);

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
        [WebInvoke(Method = "GET", UriTemplate = "/games/{GameID}?Brief={brief}")]
        IStatus GetGameStatus(string GameID, string brief);

        // original:
        //[WebGet(UriTemplate = "/games/{GameID}?Brief={brief}")]
        // minimalist:
        //[WebGet(UriTemplate = "/games/{GameID}")]
        // redux:
        //[WebInvoke(Method = "GET", UriTemplate = "/games/{GameID}?Brief={brief}")]
    }
}
