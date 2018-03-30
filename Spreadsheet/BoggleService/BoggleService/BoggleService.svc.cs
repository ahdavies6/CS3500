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
        private static Dictionary<string, User> Users = new Dictionary<string, User>();

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
        /// Generates a new, unique UserToken.
        /// </summary>
        /// <returns></returns>
        private string GenerateNewToken()
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

                User user = new User(token, trimmedNickname);
                Users.Add(token, user);

                //response to the client
                UserTokenResponse response = new UserTokenResponse();
                response.UserToken = token;
                SetStatus(Created);
                return response;
            }
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
            lock (sync)
            {
                User player;
                if (Users.ContainsKey(request.UserToken))
                {
                    player = Users[request.UserToken];
                }
                else
                {
                    SetStatus(Forbidden);
                    return null;
                }

                int timeLimit;
                if (Int32.TryParse(request.TimeLimit, out timeLimit))
                {
                    if (!(timeLimit >= 5 && timeLimit <= 120))
                    {
                        SetStatus(Forbidden);
                        return null;
                    }
                }
                else
                {
                    SetStatus(Forbidden);
                    return null;
                }

                if (PendingGames.ContainsKey(request.UserToken))
                {
                    SetStatus(Conflict);
                    return null;
                }

                GameIDResponse response = new GameIDResponse();
                if (PendingGames.Count > 0)
                {
                    //BoggleGame game = PendingGames.GetEnumerator().Current.Value;
                    var games = PendingGames.GetEnumerator();
                    games.MoveNext();
                    BoggleGame game = games.Current.Value;

                    game.AddSecondPlayer(player, timeLimit);
                    PendingGames.Remove(request.UserToken);

                    Games.Add(game.GameID, game);

                    response.GameID = game.GameID;
                    SetStatus(Created);
                }
                else
                {
                    BoggleGame newGame = new BoggleGame(player, timeLimit, GenerateGameID());

                    PendingGames.Add(request.UserToken, newGame);

                    response.GameID = newGame.GameID;
                    SetStatus(Accepted);
                }

                return response;
            }
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
        /// Plays word to game gameID
        /// 
        /// If Word is null or empty or longer than 30 characters when trimmed, or if GameID
        /// or UserToken is invalid, or if UserToken is not a player in the game identified by GameID,
        /// responds with response code 403 (Forbidden).
        /// 
        /// Otherwise, if the game state is anything other than "active", responds with response
        /// code 409 (Conflict).
        /// 
        /// Otherwise, records the trimmed Word as being played by UserToken in the game identified by
        /// GameID. Returns the score for Word in the context of the game (e.g. if Word has been played before
        /// the score is zero). Responds with status 200 (OK). Note: The word is not case sensitive.
        /// </summary>
        public ScoreResponse PlayWord(PlayWord request, string gameID)
        {
            lock (sync)
            {
                string trimmedWord = request.Word.Trim();

                if (trimmedWord == null || trimmedWord == "" || trimmedWord.Length > 30)
                {
                    SetStatus(Forbidden);
                    return null;
                }

                if (!(Games.ContainsKey(gameID)))
                {
                    SetStatus(Forbidden);
                    return null;
                }

                User player = Games[gameID].GetUser(Users[request.UserToken]);

                if (player == null)
                {
                    SetStatus(Forbidden);
                    return null;
                }

                BoggleGame game = Games[gameID];

                if (game.Status != GameStatus.Active)
                {
                    SetStatus(Conflict);
                    return null;
                }

                ScoreResponse response = new ScoreResponse();
                response.Score = game.PlayWord(player, request.Word);
                SetStatus(OK);

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
        /// Get the game status of game GameID
        /// 
        /// If GameID is invalid, responds with status 403 (Forbidden).
        /// 
        /// Otherwise, returns information about the game named by GameID as illustrated below. Note that
        /// the information returned depends on whether "Brief=yes" was included as a parameter as well as
        /// on the state of the game. Responds with status code 200 (OK). Note: The Board and Words are
        /// not case sensitive.
        /// </summary>
        public IStatus GetGameStatus(string GameID, string brief)
        {
            if (PendingGames.ContainsKey(GameID))
            {
                StateResponse response = new StateResponse();
                response.GameState = GameStatus.Pending;

                SetStatus(OK);
                return response;
            }
            else if (Games.ContainsKey(GameID))
            {
                BoggleGame game = Games[GameID];

                FullStatusResponse response = new FullStatusResponse();

                // non-player game data
                response.GameState = game.Status;
                response.Board = game.Board.ToString();
                response.TimeLimit = game.TimeLimit;
                response.TimeLeft = game.TimeLeft;

                // player1 game data
                SerialPlayer player1 = new SerialPlayer();
                player1.Nickname = game.Player1.User.Nickname;
                player1.Score = game.Player1.Score;

                HashSet<WordEntry> wordsPlayed1 = new HashSet<WordEntry>();
                for (int i = 0; i < game.Player1.Words.Count; i++)
                {
                    WordEntry wordEntry = new WordEntry();
                    wordEntry.Word = game.Player1.Words[i];
                    wordEntry.Score = game.Player1.WordScores[i];

                    wordsPlayed1.Add(wordEntry);
                }
                player1.WordsPlayed = wordsPlayed1;

                // player 2 game data
                SerialPlayer player2 = new SerialPlayer();
                player2.Nickname = game.Player2.User.Nickname;
                player2.Score = game.Player2.Score;

                HashSet<WordEntry> wordsPlayed2 = new HashSet<WordEntry>();
                for (int i = 0; i < game.Player2.Words.Count; i++)
                {
                    WordEntry wordEntry = new WordEntry();
                    wordEntry.Word = game.Player2.Words[i];
                    wordEntry.Score = game.Player2.WordScores[i];

                    wordsPlayed2.Add(wordEntry);
                }
                player2.WordsPlayed = wordsPlayed2;

                SetStatus(OK);
                return response;
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
