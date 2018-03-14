using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleClient.Open
{
    interface IOpenView
    {
        /// <summary>
        /// Attempt to connect to the server
        /// </summary>
        event Action<string, string> ConnectToServer;

        /// <summary>
        /// Event that occurs when the cancel button is pushed
        /// </summary>
        event Action CancelPushed;

        /// <summary>
        /// Moves to the next state in the game (the actual boggle game
        /// </summary>
        // TODO: Change action to a proper delegate type
        event Action NextState;
    }
}
