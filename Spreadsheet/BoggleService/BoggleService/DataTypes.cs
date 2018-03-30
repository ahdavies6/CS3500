using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Collection of classes that represent the variety of request bodies that can be sent to the server
/// </summary>
namespace Boggle
{
    //Todo is there a more elegant solution than having a class type with just instance variable?

    /// <summary>
    /// Class the represents the request to make a user
    /// </summary>
    public class CreateUserRequest
    {
        /// <summary>
        /// Nickname passed to a create user request
        /// </summary>
        public string Nickname { get; set; }
    }

    /// <summary>
    /// Class that represents a join request
    /// </summary>
    public class JoinRequest
    {
        /// <summary>
        /// UserToken of the person who wants to join a game
        /// </summary>
        public string UserToken { get; set; }

        /// <summary>
        /// Time limit the user wants
        /// </summary>
        public string TimeLimit { get; set; }
    }

    /// <summary>
    /// Represents the body of a cancel request
    /// </summary>
    public class CancelJoinRequest
    {
        /// <summary>
        /// UserToken of the person who wants to cancel a join request
        /// </summary>
        public string UserToken { get; set; }
    }

    /// <summary>
    /// Class to hold the params for the request to play a word
    /// </summary>
    public class PlayWord
    {
        /// <summary>
        /// UserToken of the person putting in the play word request
        /// </summary>
        public string UserToken { get; set; }

        /// <summary>
        /// Word being played
        /// </summary>
        public string Word { get; set; }
    }

    #region excess - to delete if the expando object works for returning a json
    /// <summary>
    /// Class that holds a UserToken string for Json serialization and deserialization
    /// </summary>
    public class UserTokenClass
    {

        /// <summary>
        /// Token of the user
        /// </summary>
        public string UserToken { get; set; }

    }


    /// <summary>
    /// Holds the score when responding to playing a word 
    /// </summary>
    public class ScoreResponse
    {
        /// <summary>
        /// Int that represents a score sent back from the server
        /// </summary>
        public int Score { get; set; }
    }

    /// <summary>
    /// Represents just the game state, used for a pending game status 
    /// </summary>
    public class GameStateClass
    {
        /// <summary>
        /// Represents the game state
        /// </summary>
        public string GameState { get; set; }
    }

    /// <summary>
    /// Serialized when game status is asked for and is meant to be brief
    /// </summary>
    public class GameStatusBrief
    {
        /// <summary>
        /// Represents the state of the game
        /// </summary>
        public string GameState { get; set; }

        /// <summary>
        /// Time left in the game
        /// </summary>
        public int TimeLeft { get; set; }

        /// <summary>
        /// Player1 1's score
        /// </summary>
        public PlayerBrief Player1 { get; set; }

        /// <summary>
        /// Player1 2's score
        /// </summary>
        public PlayerBrief Player2 { get; set; }

        /// <summary>
        /// Player class for brief game status
        /// </summary>
        public class PlayerBrief
        {
            /// <summary>
            /// Score the current player 
            /// </summary>
            string Score { get; set; }
        }

    }

    #endregion
}