using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ServiceModel.Web;
using System.Net.Http;
using static System.Net.HttpStatusCode;
using System.Configuration;

namespace Boggle
{
    public class BoggleService : IBoggleService
    {
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
        /// Keeps track of any pending games, should only be one but kept as a dictionary in case requests get large 
        /// Dicationary key: string GameID
        /// Dicationary Value: BoggleGame 
        /// 
        /// Once a second player is found, the game is removed and then moved into the games dictionary
        /// </summary>
        private static Dictionary<string, BoggleGame> PendingGames = new Dictionary<string, BoggleGame>();

        /// <summary>
        /// Lock object for server threading.
        /// </summary>
        private static object sync = new object();

        /// <summary>
        /// Value that keeps track of the amount of games created when the server was created.
        /// Used to make GameIDs
        /// </summary>
        private static int NumberOfGames = 0;

        // The connection string to the DB
        private static string BoggleDB;

        static BoggleService()
        {
            // Saves the connection string for the database.  A connection string contains the
            // information necessary to connect with the database server.  When you create a
            // DB, there is generally a way to obtain the connection string.  From the Server
            // Explorer pane, obtain the properties of DB to see the connection string.

            // The connection string of my ToDoDB.mdf shows as
            //
            //    Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename="C:\Users\zachary\Source\CS 3500 S16\examples\ToDoList\ToDoListDB\App_Data\ToDoDB.mdf";Integrated Security=True
            //
            //Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\BoggleDB.mdf;Integrated Security=True

            // Unfortunately, this is absolute pathname on my computer, which means that it
            // won't work if the solution is moved.  Fortunately, it can be shorted to
            //
            //    Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename="|DataDirectory|\ToDoDB.mdf";Integrated Security=True
            //
            // You should shorten yours this way as well.
            //
            // Rather than build the connection string into the program, I store it in the Web.config
            // file where it can be easily found and changed.  You should do that too.
            BoggleDB = ConfigurationManager.ConnectionStrings["BoggleDB"].ConnectionString;
        }

        /// <summary>
        /// The most recent call to SetStatus determines the response code used when
        /// an http response is sent.
        /// </summary>
        private static void SetStatus(HttpStatusCode status)
        {
            WebOperationContext.Current.OutgoingResponse.StatusCode = status;
        }

        /// <summary>
        /// Registers new user
        /// </summary>
        public UserTokenResponse RegisterUser(CreateUserRequest request)
        {
            lock (sync)
            {
                if (request.Nickname is null)
                {
                    SetStatus(Forbidden);
                    return null;
                }

                string trimmedNickname = request.Nickname.Trim();
                if (trimmedNickname.Length == 0 || trimmedNickname.Length > 50)
                {
                    SetStatus(Forbidden);
                    return null;
                }

                string token = UserTokenGenerator();

                User user = new User(trimmedNickname, token);
                Users.Add(token, user);

                //response to the client
                UserTokenResponse response = new UserTokenResponse
                {
                    UserToken = token
                };
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


                if (!(request.TimeLimit >= 5 && request.TimeLimit <= 120))
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
                if (PendingGames.Count != 0)
                {
                    string currkey = null;
                    BoggleGame game = null;
                    foreach (string key in PendingGames.Keys)
                    {
                        currkey = key;
                        game = PendingGames[key];
                        break;
                    }

                    game.AddSecondPlayer(player, request.TimeLimit);
                    PendingGames.Remove(currkey);

                    Games.Add(game.GameID, game);

                    response.GameID = game.GameID;
                    SetStatus(Created);
                }
                else
                {
                    BoggleGame newGame = new BoggleGame(player, request.TimeLimit, GenerateGameID());

                    PendingGames.Add(request.UserToken, newGame);

                    response.GameID = newGame.GameID;
                    SetStatus(Accepted);
                }

                return response;
            }
        }

        /// <summary>
        /// Cancels an active join request from user userToken.
        /// 
        /// If UserToken is invalid or is not a player in the pending game, responds with status
        /// 403 (Forbidden).
        /// 
        /// Otherwise, removes UserToken from the pending game and responds with status 200 (OK).
        /// </summary>
        public void CancelJoinRequest(CancelJoinRequest request)
        {
            lock (sync)
            {
                if (request.UserToken is null || !PendingGames.ContainsKey(request.UserToken))
                {
                    SetStatus(Forbidden);
                }
                else
                {
                    PendingGames.Remove(request.UserToken);
                    NumberOfGames--;
                    SetStatus(OK);
                }
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
                string word = request.Word;
                string uid = request.UserToken;

                if (uid is null || (uid = uid.Trim()).Length == 0 || gameID is null || word is null || (word = word.Trim()).Length == 0 || word.Length > 30)
                {
                    SetStatus(Forbidden);
                    return null;
                }

                User curr;
                BoggleGame game;

                if (!Users.TryGetValue(uid, out curr) || !Games.TryGetValue(gameID, out game))
                {
                    SetStatus(Forbidden);
                    return null;
                }

                try
                {
                    ScoreResponse response = new ScoreResponse()
                    {
                        Score = game.PlayWord(curr, word)
                    };

                    SetStatus(OK);
                    return response;
                }
                catch (PlayerNotInGameException e)
                {
                    SetStatus(Forbidden);
                    return null;
                }
                catch (GameNotActiveException e)
                {
                    SetStatus(Conflict);
                    return null;
                }
            }
        }

        /// <summary>
        /// Generates a new, unique user token that is a Guid
        /// </summary>
        private string UserTokenGenerator()
        {
            return Guid.NewGuid().ToString();
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
        public FullStatusResponse GetGameStatus(string GameID, string brief)
        {
            lock (sync)
            {
                //format brief 
                if (brief is null) brief = "";
                else brief = brief.ToLower();

                //check pending games
                //this should never be large by design
                foreach (string key in PendingGames.Keys)
                {
                    if (PendingGames[key].GameID.Equals(GameID))
                    {
                        FullStatusResponse response = new FullStatusResponse
                        {
                            GameState = "pending"
                        };

                        SetStatus(OK);
                        return response;
                    }
                }

                if (Games.ContainsKey(GameID))
                {
                    BoggleGame game = Games[GameID];
                    FullStatusResponse response = new FullStatusResponse();

                    // active and brief 
                    if (game.GameState == GameStatus.Active && brief.Equals("yes"))
                    {
                        response.GameState = "active";
                        response.TimeLeft = game.TimeLeft;
                        response.Player1 = new SerialPlayer()
                        {
                            Score = game.Player1.Score
                        };
                        response.Player2 = new SerialPlayer()
                        {
                            Score = game.Player2.Score
                        };

                    }
                    //active not brief
                    else if (game.GameState == GameStatus.Active && !brief.Equals("yes"))
                    {
                        response.GameState = "active";
                        response.Board = game.Board.ToString();
                        response.TimeLimit = game.TimeLimit;
                        response.TimeLeft = game.TimeLeft;
                        response.Player1 = new SerialPlayer()
                        {
                            Nickname = game.Player1.User.Nickname,
                            Score = game.Player1.Score
                        };
                        response.Player2 = new SerialPlayer()
                        {
                            Nickname = game.Player2.User.Nickname,
                            Score = game.Player2.Score
                        };
                    }
                    //completed and brief
                    else if (game.GameState == GameStatus.Completed && brief.Equals("yes"))
                    {
                        response.GameState = "completed";
                        response.TimeLeft = 0;
                        response.Player1 = new SerialPlayer()
                        {
                            Score = game.Player1.Score
                        };
                        response.Player2 = new SerialPlayer()
                        {
                            Score = game.Player2.Score
                        };

                    }
                    //completed not brief
                    else if (game.GameState == GameStatus.Completed && !brief.Equals("yes"))
                    {
                        response.GameState = "completed";
                        response.Board = game.Board.ToString();
                        response.TimeLimit = game.TimeLimit;
                        response.TimeLeft = 0;
                        response.Player1 = new SerialPlayer()
                        {
                            Nickname = game.Player1.User.Nickname,
                            Score = game.Player1.Score,
                            WordsPlayed = new List<WordEntry>()
                        };
                        response.Player2 = new SerialPlayer()
                        {
                            Nickname = game.Player2.User.Nickname,
                            Score = game.Player2.Score,
                            WordsPlayed = new List<WordEntry>()
                        };

                        //add all words of 1
                        for (int i = 0; i < game.Player1.Words.Count; i++)
                        {
                            response.Player1.WordsPlayed.Add(new WordEntry()
                            {
                                Word = game.Player1.Words[i],
                                Score = game.Player1.WordScores[i]
                            });
                        }

                        //add all words of 2
                        for (int i = 0; i < game.Player2.Words.Count; i++)
                        {
                            response.Player2.WordsPlayed.Add(new WordEntry()
                            {
                                Word = game.Player2.Words[i],
                                Score = game.Player2.WordScores[i]
                            });
                        }
                    }

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
    }

    /// <summary>
    /// Singleton class allows checking whether a word is valid
    /// </summary>
    public class Words
    {
        /// <summary>
        /// Singleton self
        /// </summary>
        private static Words me;

        /// <summary>
        /// All the valid words
        /// </summary>
        private static ISet<string> words;

        /// <summary>
        /// Singleton constructor
        /// 
        /// Passes in all the words from dictionary.txt to words field
        /// </summary>
        private Words()
        {
            if (me == null)
            {
                words = new HashSet<string>();

                using (StreamReader file = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "dictionary.txt"))
                {
                    string currLine;
                    while ((currLine = file.ReadLine()) != null)
                    {
                        words.Add(currLine.ToUpper());
                    }
                }
            }
        }

        /// <summary>
        /// Checks whether word is a valid dictionary word
        /// </summary>
        public static bool IsValidWord(string word)
        {
            if (me == null)
            {
                me = new Words();
            }

            return words.Contains(word.ToUpper());
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
