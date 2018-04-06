using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ServiceModel.Web;
using System.Net.Http;
using static System.Net.HttpStatusCode;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace Boggle
{
    // todo: remove all deprecated code once database has been fully implemented
    /// <summary>
    /// Boggle game server that keeps track of user data, game data, and serves it up as necessary
    /// for a Boggle game client to receive. Also allows users to interact with the server as necessary
    /// to create "accounts" and play the game against other users.
    /// </summary>
    public class BoggleService : IBoggleService
    {
        ///// <summary>
        ///// Keeps track of all users
        ///// Key: UserToken
        ///// Value: Nickname
        ///// </summary>
        //private static Dictionary<string, User> Users = new Dictionary<string, User>();

        ///// <summary>
        ///// Dictionary to represent the games in use
        ///// Key: GameID (gotten from when the game is created). It is a property in BoggleGame
        ///// Value: BoggleGame game 
        ///// 
        ///// Contains both active and completed games but NOT pending games
        ///// </summary>
        //private static Dictionary<string, BoggleGame> Games = new Dictionary<string, BoggleGame>();

        ///// <summary>
        ///// Keeps track of any pending games, should only be one but kept as a dictionary in case requests get large 
        ///// Dicationary key: string GameID
        ///// Dicationary Value: BoggleGame 
        ///// 
        ///// Once a second player is found, the game is removed and then moved into the games dictionary
        ///// </summary>
        //private static Dictionary<string, BoggleGame> PendingGames = new Dictionary<string, BoggleGame>();

        ///// <summary>
        ///// Lock object for server threading.
        ///// </summary>
        //private static object sync = new object();

        ///// <summary>
        ///// Value that keeps track of the amount of games created when the server was created.
        ///// Used to make GameIDs
        ///// </summary>
        //private static int NumberOfGames = 0;

        /// <summary>
        /// The connection string to the database that contains all of the server's data
        /// </summary>
        private static string BoggleDB;

        /// <summary>
        /// Dictionary of valid words
        /// </summary>
        private static HashSet<string> Dict;

        /// <summary>
        /// Saves the database's connection string and the dictionary's filepath
        /// </summary>
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

            //null checks 
            if (request is null || request.UserToken is null)
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

            // open connection to database
            using (SqlConnection connection = new SqlConnection(BoggleDB))
            {
                connection.Open();

                // execute all commands within a single transaction
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    // todo: make sure GameID starts at 1, not 0
                    // (otherwise, this should be changed, because this might actually end up as 0 when creating a game)
                    int gameID = -1;

                    // find out whether there's a pending game
                    using (SqlCommand command = new SqlCommand("select GameID, Player1, TimeLimit from Games where Player2 is null",
                        connection, transaction))
                    {
                        // the reader that will determine whether there's a pending game, and if so, what its gameID is
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                gameID = (int)reader["GameID"];
                                string player1 = (string)reader["Player1"];

                                // averages the time between player1's requested time and this user's requested time
                                int player1RequestedTime = (int)reader["TimeLimit"];
                                timeLimit = (timeLimit + player1RequestedTime) / 2;

                                // player is already searching for a game
                                if (player1 == userID)
                                {
                                    SetStatus(Conflict);
                                    reader.Close();
                                    transaction.Commit();
                                    return null;
                                }
                            }
                        }
                    }

                    if (gameID == -1)
                    {
                        using (SqlCommand command = new SqlCommand("insert into Games (Player1, TimeLimit) output inserted.GameID values(@Player1, @TimeLimit)", connection, transaction))
                        {
                            command.Parameters.AddWithValue("@Player1", userID);
                            command.Parameters.AddWithValue("@TimeLimit", timeLimit);

                            SqlParameter outputparam = new SqlParameter("@GameID", SqlDbType.Int)
                            {
                                Direction = ParameterDirection.Output
                            };

                            command.Parameters.Add(outputparam);

                            // execute command, and get back the primary key (GameID)
                            try
                            {
                                gameID = (int)command.ExecuteScalar();

                            }
                            catch (SqlException)
                            {
                                SetStatus(Forbidden);
                                transaction.Commit();
                                return null;
                            }
                            SetStatus(Accepted);
                        }
                    }
                    else
                    {
                        using (SqlCommand command = new SqlCommand("update Games set Player2 = @Player2, Board = @Board, TimeLimit = @TimeLimit, StartTime = @StartTime where GameID = @GameID", connection, transaction))
                        {
                            command.Parameters.AddWithValue("@Player2", userID);
                            command.Parameters.AddWithValue("@Board", BoggleBoard.GenerateBoggleBoard());
                            command.Parameters.AddWithValue("@TimeLimit", timeLimit);
                            command.Parameters.AddWithValue("@StartTime", DateTime.UtcNow);
                            command.Parameters.AddWithValue("@GameID", gameID);

                            try
                            {
                                if (command.ExecuteNonQuery() != 0)
                                {
                                    //throw new Exception("SQL query failed");
                                }
                            }
                            catch (SqlException)
                            {
                                SetStatus(Forbidden);
                                transaction.Commit();
                                return null;
                            }

                            SetStatus(Created);

                        }

                    }

                    transaction.Commit();
                    return new GameIDResponse { GameID = gameID };
                }
            }
        }

        /// <summary>
        /// Helper method for JoinGame.
        /// Creates a new pending game with userID, and returns the primary key GameID for the
        /// new pending game.
        /// </summary>
        private int CreateNewGame(string userID, int requestedTime)
        {
            // open connection to database
            using (SqlConnection connection = new SqlConnection(BoggleDB))
            {
                connection.Open();

                // execute all commands within a single transaction
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    // todo: make sure this command (especially the part(s) in parenthesis) does what you think it does
                    return 0;
                }
            }
        }

        /// <summary>
        /// Helper method for JoinGame.
        /// Adds userID to the pending game gameID, and returns whether this worked or not.
        /// </summary>
        private bool JoinPendingGame(string userID, int gameID, int timeLimit)
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
                    return true;
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

            if (request is null || request.UserToken is null || (request.UserToken = request.UserToken.Trim()).Length == 0)
            {
                SetStatus(Forbidden);
                return;
            }

            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                conn.Open();
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
                return null;
            }

            //values used when doing the query then insert into words
            string player1, player2, board;
            int timeLimit;
            DateTime? startTime;

            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand cmd = new SqlCommand("select GameID, Player1, Player2, Board, TimeLimit, StartTime from Games where GameID = @GameID", conn, trans))
                    {
                        cmd.Parameters.AddWithValue("@GameID", gameID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            //Game doesnt exist
                            if (!reader.HasRows)
                            {
                                SetStatus(Forbidden);
                                reader.Close();
                                //trans.Commit();
                                return null;
                            }

                            //only one row 
                            reader.Read();

                            //test if pending
                            if (DBNull.Value.Equals(reader["Player2"]))
                            {
                                SetStatus(Conflict);
                                reader.Close();
                                trans.Commit();
                                return null;
                            }

                            player1 = (string)reader["Player1"];
                            player2 = (string)reader["Player2"];

                            board = reader["Board"]?.ToString();
                            timeLimit = (int)reader["TimeLimit"];
                            startTime = reader.GetDateTime(5);
                        }
                    }


                    //Checks if the player is in the game
                    if (!(player1.Equals(request.UserToken) || player2.Equals(request.UserToken)))
                    {
                        SetStatus(Forbidden);
                        trans.Commit();
                        return null;
                    }

                    //check if the game is compeleted
                    if ((startTime?.AddSeconds(timeLimit) - DateTime.UtcNow)?.TotalMilliseconds < 0)
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
                    using (SqlCommand cmd = new SqlCommand("insert into Words (Word, GameID, Player, Score) values(@Word, @GameID, @Player, @Score)", conn, trans))
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
        /// Scores the passed word in the passed boggle board. It assumes the word has not been played before
        /// 
        /// Case insensitive
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
        /// Returns the game status of game GameID.
        /// 
        /// If GameID is invalid, responds with status 403 (Forbidden).
        /// Otherwise, returns information about the game named by GameID as illustrated below. Note that
        /// the information returned depends on whether brief was included as a parameter as well as
        /// on the state of the game. Responds with status code 200 (OK). Note: The Board and Words are
        /// not case sensitive.
        /// </summary>
        public FullStatusResponse GetGameStatus(string gameID, string brief)
        {

            bool briefbool = brief.ToLower().Equals("yes");

            int id;
            if (!int.TryParse(gameID, out id))
            {
                SetStatus(Forbidden);
            }

            // open connection to database
            using (SqlConnection connection = new SqlConnection(BoggleDB))
            {
                connection.Open();

                // execute all commands within a single transaction
                using (SqlTransaction transaction = connection.BeginTransaction())
                {

                    string p1id = "";
                    string p2id = "";
                    bool completed = false;
                    FullStatusResponse response = new FullStatusResponse();

                    // figures out the GameState and records it
                    using (SqlCommand command = new SqlCommand("select * from Games where GameID = @GameID", connection, transaction))
                    {

                        command.Parameters.AddWithValue("@GameID", id);

                        // read the information the command returned
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows) // gameID wasn't in the database
                            {
                                SetStatus(Forbidden);
                                return null;
                            }

                            while (reader.Read())
                            {
                                // todo: see if this actually returns "null" for an empty column
                                // if the game only has one player, it's pending
                                if (DBNull.Value.Equals(reader["Player2"]))
                                {
                                    response.GameState = "pending";

                                    SetStatus(OK);
                                    return response;
                                }

                                // add the stuff for brief & active/inactive (it's the same stuff)

                                response.Player1 = new SerialPlayer();
                                response.Player2 = new SerialPlayer();

                                DateTime start = (reader["StartTime"] as DateTime?).GetValueOrDefault();
                                int limit = (reader["TimeLimit"] as int?).GetValueOrDefault();
                                string board = (string)reader["Board"];
                                completed = false;
                                int timeleft = (int)(start.AddSeconds(limit) - DateTime.UtcNow).TotalSeconds;
                                if (timeleft > 0)
                                {
                                    completed = false;
                                    response.GameState = "active";
                                    response.TimeLeft = timeleft;
                                }
                                else
                                {
                                    completed = true;
                                    response.TimeLeft = 0;
                                    response.GameState = "completed";
                                }

                                if (!briefbool)
                                {
                                    response.Board = board;
                                }

                                p1id = (string)reader["Player1"];
                                p2id = (string)reader["Player2"];

                            }
                        }
                    }

                    //Get p1 words
                    using (SqlCommand cmd = new SqlCommand("select * from Words where GameID = @GameID and Player = @Player", connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Player", p1id);
                        cmd.Parameters.AddWithValue("@GameID", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            IList<WordEntry> p1words = new List<WordEntry>();
                            int p1score = 0;

                            while (reader.HasRows)
                            {
                                reader.Read();
                                int s = (int)reader["Score"];
                                p1words.Add(new WordEntry()
                                {
                                    Word = (string)reader["Word"],
                                    Score = s
                                });
                                p1score += s;
                            }

                            if (briefbool)
                            {
                                response.Player1.Score = p1score;
                            }
                            else if (completed == true)
                            {
                                response.Player1.WordsPlayed = p1words;
                            }
                            else if (completed == false)
                            {
                                response.Player1.Score = p1score;
                            }

                        }
                    }

                    //get p2 words
                    using (SqlCommand cmd = new SqlCommand("select * from Words where GameID = @GameID and Player = @Player", connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Player", p2id);
                        cmd.Parameters.AddWithValue("@GameID", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            IList<WordEntry> p2words = new List<WordEntry>();
                            int p2score = 0;

                            while (reader.HasRows)
                            {
                                reader.Read();
                                int s = (int)reader["Score"];
                                p2words.Add(new WordEntry()
                                {
                                    Word = (string)reader["Word"],
                                    Score = s
                                });
                                p2score += s;
                            }

                            if (briefbool)
                            {
                                response.Player2.Score = p2score;
                            }
                            else if (completed == true)
                            {
                                response.Player2.WordsPlayed = p2words;
                            }
                            else if (completed == false)
                            {
                                response.Player2.Score = p2score;
                            }

                        }
                    }


                    //get p1 nickname
                    if (!briefbool)
                    {
                        using (SqlCommand cmd = new SqlCommand("select * from Users where UserID = @id", connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@id", p1id);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {

                                while (reader.Read())
                                {

                                    response.Player1.Nickname = (string)reader["Nickname"];
                                }

                            }
                        }
                    }

                    //get p2 nickname
                    if (!briefbool)
                    {
                        using (SqlCommand cmd = new SqlCommand("select * from Users where UserID = @id", connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@id", p2id);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {

                                while (reader.Read())
                                {
                                    response.Player2.Nickname = (string)reader["Nickname"];
                                }

                            }
                        }
                    }

                    SetStatus(OK);
                    transaction.Commit();
                    return response;
                }
            }
        }

        /*
                               // brief active & inactive data

                               string p1ID = (string)reader["Player1"];
                               // todo: make sure that nested database searching like this doesn't cause problems
                               IList<WordEntry> p1Words = GetPlayerScores(gameID, p1ID, out int p1Score);
                               SerialPlayer player1 = new SerialPlayer { Score = p1Score };
                               response.Player1 = player1;

                               string p2ID = (string)reader["Player2"];
                               // todo: make sure that nested database searching like this doesn't cause problems
                               IList<WordEntry> p2Words = GetPlayerScores(gameID, p2ID, out int p2Score);
                               SerialPlayer player2 = new SerialPlayer { Score = p2Score };
                               response.Player2 = player2;

                               // brief active & inactive response
                               if (briefbool)
                               {
                                   response.Player1 = player1;
                                   response.Player2 = player2;

                                   SetStatus(OK);
                                   return response;
                               }

                               // not brief, active data

                               response.Board = (string)reader["Board"];
                               response.TimeLimit = (int)reader["TimeLimit"];

                               // todo: make sure that nested database searching like these don't cause problems
                               response.Player1.Nickname = GetPlayerNickname(p1ID);
                               response.Player2.Nickname = GetPlayerNickname(p2ID);

                               // not brief, active response
                               if (response.GameState == "active")
                               {
                                   SetStatus(OK);
                                   return response;
                               }

                               // not brief, completed data
                               response.Player1.WordsPlayed = p1Words;
                               response.Player2.WordsPlayed = p2Words;

                               // not brief, completed response
                               SetStatus(OK);
                               transaction.Commit();
                               return response;
                           }*/

        /// <summary>
        /// Private helper method returns an IList of word entries (which are words paired with
        /// what they scored) given a player and a game. Out int score is the total score for
        /// that player, that game.
        /// </summary>
        private IList<WordEntry> GetPlayerScores(string gameID, string userID, out int score)
        {
            // open connection to database
            using (SqlConnection connection = new SqlConnection(BoggleDB))
            {
                connection.Open();

                // execute all commands within a single transaction
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    // get all of the word data for a given player
                    using (SqlCommand command = new SqlCommand("select * from Words where (GameID, Player) " +
                        "= (@GameID, @UserID)", connection, transaction))
                    {
                        command.Parameters.AddWithValue("@GameID", gameID);
                        command.Parameters.AddWithValue("@UserID", userID);

                        IList<WordEntry> words = new List<WordEntry>();
                        score = 0;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                WordEntry word = new WordEntry
                                {
                                    Word = (string)reader["Word"],
                                    Score = (int)reader["Score"]
                                };

                                words.Add(word);
                                score += (int)reader["Score"];
                            }
                        }

                        return words;
                    }
                }
            }
        }

        /// <summary>
        /// Private helper method returns a player's Nickname, given their UserID
        /// </summary>
        private string GetPlayerNickname(string userID)
        {
            // open connection to database
            using (SqlConnection connection = new SqlConnection(BoggleDB))
            {
                connection.Open();

                // execute all commands within a single transaction
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    // get the PlayerID
                    using (SqlCommand command = new SqlCommand("select Nickname from Users where UserID = @UserID",
                        connection, transaction))
                    {
                        command.Parameters.AddWithValue("@UserID", userID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            string nickname = null;

                            while (reader.Read())
                            {
                                nickname = (string)reader["Nickname"];
                            }

                            return nickname;
                        }
                    }
                }
            }
        }

    }

}
