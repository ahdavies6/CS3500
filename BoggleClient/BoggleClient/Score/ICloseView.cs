using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleClient.Score
{
    interface ICloseView
    {
        /// <summary>
        /// Restarts the application and puts it back into the open screen
        /// </summary>
        // TODO: Fix the action to the proper parameters / delegate type
        event Action CancelPushed;

        /// <summary>
        /// Extra event that we could use for making a new game 
        /// 
        /// NOTE: We would have to change action to the proper delegate type
        /// </summary>
        event Action NewGame;
    }
}
