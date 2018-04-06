using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

/// <summary>
/// Collection of classes that represent the variety of request bodies that can be sent to the server
/// </summary>
namespace Boggle
{
    #region Request Structures
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
        public int TimeLimit { get; set; }
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

    #endregion

    #region Response structures

    /// <summary>
    /// Sends back the UserToken
    /// </summary>
    public class UserTokenResponse
    {
        public string UserToken { get; set; }
    }

    /// <summary>
    /// Sends back the score produced by a play word request
    /// </summary>
    public class ScoreResponse
    {
        public int Score { get; set; }
    }

    /// <summary>
    /// Class that sends back the GameID of the game
    /// </summary>
    public class GameIDResponse
    {
        public int GameID { get; set; }
    }

    /// <summary>
    /// An interface to label what data structures can come from a Status response
    /// </summary>
    public class IStatus
    {
        // instantiators define things here
    }

    /// <summary>
    /// Class for when status just needs to send "pending"
    /// </summary>
    [DataContract]
    public class StateResponse : IStatus
    {
        [DataMember]
        public string GameState { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Board { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int TimeLimit { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? TimeLeft { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public SerialPlayer Player1 { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public SerialPlayer Player2 { get; set; }
    }

    /// <summary>
    /// Class that represents the response from the server when get status is called
    /// </summary>
    [DataContract]
    public class FullStatusResponse 
    {
        [DataMember]
        public string GameState { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Board { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int TimeLimit { get; set; }
        
        [DataMember]
        public int TimeLeft { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public SerialPlayer Player1 { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public SerialPlayer Player2 { get; set; }
    }

    /// <summary>
    /// Class that represents each player in the game when sending a status
    /// </summary>
    [DataContract]
    public class SerialPlayer
    {
        [DataMember(EmitDefaultValue = false)]
        public string Nickname { get; set; }

        [DataMember]
        public int Score { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public IList<WordEntry> WordsPlayed { get; set; }
    }

    /// <summary>
    /// Represents a list of words that are played in the game when sent back as a status
    /// </summary>
    public class WordEntry
    {
        public string Word { get; set; }

        public int Score { get; set; }
    }
    #endregion
}