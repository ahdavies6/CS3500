using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

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
        /// Returns the nth word from dictionary.txt.  If there is
        /// no nth word, responds with code 403. This is a demo;
        /// you can delete it.
        /// </summary>
        [WebGet(UriTemplate = "/word?index={n}")]
        string WordAtIndex(int n);

        /// <summary>
        /// Creates a user with request.Nickname as the Nickname. 
        /// 
        /// If Nickname is null, or is empty when trimmed, responds with status 403 (Forbidden). 
        /// 
        /// Otherwise, creates a new user with a unique UserToken and the trimmed Nickname.
        /// The returned UserToken should be used to identify the user in subsequent requests.
        /// Responds with status 201 (Created). 
        /// </summary>
        [WebInvoke(Method = "POST", UriTemplate = "/users")]
        UserTokenResponse RegisterUser(CreateUserRequest request);

        /// <summary>
        /// Joins a game for the given R
        // todo commments
        /// </summary>
        [WebInvoke(Method = "POST", UriTemplate = "/games")]
        GameIDResponse JoinGame(JoinRequest request);

        /// <summary>
        /// Canel Join request
        // todo commments
        /// </summary>
        [WebInvoke(Method = "PUT", UriTemplate = "/games")]
        void CancelJoinRequest(CancelJoinRequest request);

        /// <summary>
        /// Request to play word
        /// </summary>
        [WebInvoke(Method = "PUT", UriTemplate = "/games/{GameID}")]
        ScoreResponse PlayWord(PlayWord wordRequest, string GameID);

        /// <summary>
        /// Get the game status
        // todo commments
        /// </summary>
        [WebGet(UriTemplate = "/games/{GameID}?Brief={brief}")]
        Status GetGameStatus(string GameID, string brief);
    }
}
