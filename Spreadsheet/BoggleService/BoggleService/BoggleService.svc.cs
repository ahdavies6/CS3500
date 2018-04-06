using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ServiceModel.Web;
using System.Net.Http;
using static System.Net.HttpStatusCode;
using System.Configuration;
using System.Data.SqlClient;

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

        /// <summary>
        /// The connection string to the DB
        /// </summary>
        private static string BoggleDB;

        /// <summary>
        /// String that indicates where the dictionary.txt is
        /// </summary>
        private static string DictionaryLocation;

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

            DictionaryLocation = AppDomain.CurrentDomain.BaseDirectory + "dictionary.txt";
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
        /// second player. The pending game becomes active. The active game's time limit is the integer
        /// average of the time limits requested by the two players. Returns the new active game's
        /// GameID (which should be the same as the old pending game's GameID). Responds with
        /// status 201 (Created).
        /// 
        /// Otherwise, adds UserToken as the first player of a new pending game, and the TimeLimit as
        /// the pending game's requested time limit. Returns the pending game's GameID. Responds with
        /// status 202 (Accepted).
        /// </summary>
        public GameIDResponse JoinGame(JoinRequest request)
        {
            int timeLimit = request.TimeLimit;

            // make sure TimeLimit is within the expected bounds
            if (!(timeLimit >= 5 && timeLimit <= 120))
            {
                SetStatus(Forbidden);
                return null;
            }

            // the UserID that'll be used throughout this method
            string userID = request.UserToken.Trim();

            // make sure UserToken is within the expected bounds
            if (userID == null || userID.Length == 0 || userID.Length > 36)
            {
                SetStatus(Forbidden);
                return null;
            }

            // todo: check whether userToken is in users. if so, set forbidden and return null
            // open connection to database
            using (SqlConnection connection = new SqlConnection(BoggleDB))
            {
                connection.Open();

                // execute all commands within a single transaction
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    string gameID = "";

                    // find out whether there's a pending game
                    using (SqlCommand command = new SqlCommand("select GameID from Games where Player2 is null",
                        connection, transaction))
                    {
                        // the reader that will determine whether there's a pending game, and if so, what its gameID is
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                gameID = (string)reader["GameID"];
                                string player1 = (string)reader["Player1"];

                                // averages the time between player1's requested time and this user's requested time
                                int player1RequestedTime = (int)reader["TimeLimit"];
                                timeLimit = (timeLimit + player1RequestedTime) / 2;

                                // player is already searching for a game
                                if (player1 == userID)
                                {
                                    SetStatus(Conflict);
                                    return null;
                                }
                            }
                        }
                    }

                    // gameID would have been reassigned if the previous command block found a pending game
                    if (gameID == "") // create a new pending game
                    {
                        gameID = CreateNewGame(userID, timeLimit);

                        if (gameID != "ERROR") // if creating the new game in the database was a success
                        {
                            SetStatus(Accepted);
                        }
                        else // adding the new game failed, which means that userID isn't registered
                        {
                            SetStatus(Forbidden);
                        }
                    }
                    else // join the pending game
                    {
                        // recall that gameID and timeLimit were modified by the previous command block
                        if (JoinPendingGame(userID, gameID, timeLimit)) // success
                        {
                            SetStatus(Created);
                        }
                        else // joining the game failed, which means that userID isn't registered
                        {
                            SetStatus(Forbidden);
                        }
                    }

                    return new GameIDResponse { GameID = gameID };
                }
            }
        }

        /// <summary>
        /// Creates a new pending game with userID; returns the primary key GameID for the new pending game
        /// </summary>
        private string CreateNewGame(string userID, int requestedTime)
        {
            // open connection to database
            using (SqlConnection connection = new SqlConnection(BoggleDB))
            {
                connection.Open();

                // execute all commands within a single transaction
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    // todo: make sure this command (especially the part(s) in parenthesis) does what you think it does
                    using (SqlCommand command = new SqlCommand(
                        //"insert into Games (Player1) output inserted.GameID values(@Player1)", connection, transaction))
                        "insert into Games Player1 values(@Player1, @TimeLimit)", connection, transaction))
                    {
                        command.Parameters.AddWithValue("@Player1", userID);
                        command.Parameters.AddWithValue("@TimeLimit", requestedTime);

                        // execute command, and get back the primary key (GameID)
                        // todo: make sure that this doesn't throw an exception if .ToString() doesn't work
                        string gameID = command.ExecuteScalar().ToString();

                        // todo: change "null" to whatever ExecuteScalar returns if the command failed
                        // (which well may be a null string)
                        if (gameID != null)
                        {
                            return gameID;
                        }
                        else // the command failed, which should only happen if userID isn't registered
                        {
                            return "ERROR";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds userID to the pending game gameID; returns whether this worked or not
        /// </summary>
        private bool JoinPendingGame(string userID, string gameID, int timeLimit)
        {
            // open connection to database
            using (SqlConnection connection = new SqlConnection(BoggleDB))
            {
                connection.Open();

                // execute all commands within a single transaction
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    // todo: make sure this command (especially the part in parenthesis) does what you think it does
                    //Games(Player2, Board, TimeLimit, StartTime)
                    using (SqlCommand command = new SqlCommand("update Games set (Player2, Board, TimeLimit, StartTime)=" +
                        "(@Player2, @Board, @TimeLimit, @StartTime) where GameID = @GameID", connection, transaction))
                    {
                        command.Parameters.AddWithValue("@Player2", userID);
                        command.Parameters.AddWithValue("@Board", BoggleBoard.GenerateBoggleBoard());
                        command.Parameters.AddWithValue("@TimeLimit", timeLimit);
                        command.Parameters.AddWithValue("@StartTime", timeLimit);
                        command.Parameters.AddWithValue("@GameID", gameID);

                        int numRowsModified = command.ExecuteNonQuery();

                        if (numRowsModified != 0) // the command was a success
                        {
                            return true;
                        }
                        else // the command failed, which should only happen when the userID isn't registered
                        {
                            return false;
                        }
                    }
                }
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
