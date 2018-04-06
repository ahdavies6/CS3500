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
        /// Dictionary of valid words
        /// </summary>
        private static HashSet<string> Dict;

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

            Dict = new HashSet<string>();

            using (StreamReader file = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "dictionary.txt"))
            {
                string currLine;
                while ((currLine = file.ReadLine()) != null)
                {
                    Dict.Add(currLine.ToLower());
                }
            };
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
        /// Creates a user with nickname. 
        /// 
        /// If nickname is null, or is empty when trimmed, responds with status 403 (Forbidden). 
        /// 
        /// Otherwise, creates a new user with a unique UserToken and the trimmed nickname.
        /// The returned UserToken should be used to identify the user in subsequent requests.
        /// Responds with status 201 (Created). 
        /// </summary>
        public UserTokenResponse RegisterUser(CreateUserRequest request)
        {

            if (request is null || request.Nickname is null || (request.Nickname = request.Nickname.Trim()).Length == 0)
            {
                SetStatus(Forbidden);
                return null;
            }

            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand cmd = new SqlCommand("insert into Users(UserID, Nickname) values(@UserID, @Nickname)", conn, trans))
                    {
                        string uid = Guid.NewGuid().ToString();

                        cmd.Parameters.AddWithValue("@UserID", uid);
                        cmd.Parameters.AddWithValue("@Nickname", request.Nickname);

                        //try to do the insert 
                        if (cmd.ExecuteNonQuery() != 1)
                        {
                            throw new Exception("Query failed unexpectedly");
                        }

                        //Commit
                        trans.Commit();

                        //Returning values
                        SetStatus(Created);
                        return new UserTokenResponse()
                        {
                            UserToken = uid
                        };
                    }
                }
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

            if (request is null || request.UserToken is null || (request.UserToken = request.UserToken.Trim()).Length == 0)
            {
                SetStatus(Forbidden);
                return;
            }

            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand cmd = new SqlCommand("delete from Games where Player1 = @Player1Token", conn, trans))
                    {
                        cmd.Parameters.AddWithValue("@Player1Token", request.UserToken);

                        if (cmd.ExecuteNonQuery() == 0)
                        {
                            SetStatus(Forbidden);
                        }
                        else
                        {
                            SetStatus(OK);
                        }

                        trans.Commit();
                    }
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

            //null and length checks
            if (gameID is null || request is null || request.UserToken is null || (request.UserToken = request.UserToken.Trim()).Length == 0
                || request.Word is null || (request.Word = request.Word.Trim()).Length == 0 || request.Word.Length > 30)
            {
                SetStatus(Forbidden);
            }

            //values used when doing the query then insert into words
            string player1, player2, board;
            int timeLimit;
            DateTime? startTime;

            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand cmd = new SqlCommand("select GameID, Player1, Player2, Board, TimeLimit, StartTime where GameID = @GameID", conn, trans))
                    {
                        cmd.Parameters.AddWithValue("@GameID", gameID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            //Game doesnt exist
                            if (!reader.HasRows)
                            {
                                SetStatus(Forbidden);
                                reader.Close();
                                trans.Commit();
                                return null;
                            }

                            //only one row 
                            reader.Read();
                            player1 = (string)reader["Player1"];
                            player2 = reader["Player2"]?.ToString();
                            board = (string)reader["Board"];
                            timeLimit = (int)reader["TimeLimit"];
                            startTime = (DateTime?)reader["StartTime"];
                        }
                    }

                    //any indicator that the game is pending
                    if (player2 is null || startTime is null || board is null || startTime is null)
                    {
                        SetStatus(Conflict);
                        trans.Commit();
                        return null;
                    }

                    //Checks if the player is in the game
                    if (!(player1.Equals(request.UserToken) || player2.Equals(request.UserToken)))
                    {
                        SetStatus(Forbidden);
                        trans.Commit();
                        return null;
                    }

                    //check if the game is compeleted
                    if((startTime?.AddSeconds(timeLimit) - DateTime.UtcNow)?.TotalMilliseconds < 0)
                    {
                        SetStatus(Conflict);
                        trans.Commit();
                        return null;
                    }

                    int score = ScoreWord(new BoggleBoard(board), request.Word);

                    //Check if the word has already been played by this player
                    using (SqlCommand cmd = new SqlCommand("select Id from Words where Word = @Word and GameID = @GameID and Player = @Player", conn, trans))
                    {
                        cmd.Parameters.AddWithValue("@Word", request.Word);
                        cmd.Parameters.AddWithValue("@GameID", gameID);
                        cmd.Parameters.AddWithValue("@Player", request.UserToken);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                score = 0;
                            }
                        }
                    }

                    //add the new word
                    using (SqlCommand cmd = new SqlCommand("insert into Words (Word, GameID, Player, Score) values(@Word, @GameID, @Player, @Score", conn, trans))
                    {
                        cmd.Parameters.AddWithValue("@Word", request.Word);
                        cmd.Parameters.AddWithValue("@GameID", gameID);
                        cmd.Parameters.AddWithValue("@Player", request.UserToken);
                        cmd.Parameters.AddWithValue("@Score", score);

                        if (cmd.ExecuteNonQuery() != 1)
                        {
                            throw new Exception("Query failed unexpectedly");
                        }

                        trans.Commit();

                        SetStatus(OK);
                        return new ScoreResponse() { Score = score };

                    }

                }
            }
        }

        /// <summary>
        /// Given a word and a boggle board, scores it as if it was being played for the first time
        /// </summary>
        /// <param name="bg"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        private int ScoreWord(BoggleBoard bg, string word)
        {
            if (word.Length < 3)
            {
                return 0;
            }
            else if (bg.CanBeFormed(word) && Dict.Contains(word.ToLower()))
            {
                int leng = word.Length;

                if (leng == 3 || leng == 4)
                {
                    return 1;
                }
                else if (leng == 5 || leng == 6)
                {
                    return leng - 3;
                }
                else if (leng == 7)
                {
                    return 5;
                }
                else
                {
                    return 11;
                }
            }
            else
            {
                return -1;
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
