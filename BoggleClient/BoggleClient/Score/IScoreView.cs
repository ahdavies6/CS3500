using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleClient.Score
{
    interface IScoreView
    {
        /// <summary>
        /// Restarts the application and returns it to the open screen
        /// </summary>
        // TODO: Fix the action to the proper parameters / delegate type
        // Hold up... I don't understand the above. Why doesn't action work?
        event Action CancelPushed;

        /// <summary>
        /// Extra event that we could use for making a new game 
        /// 
        /// NOTE: We would have to change action to the proper delegate type
        /// </summary>
        // todo: implement/figure out the note above
        // todo: decide whether to remove this or not
        event Action NewGame;
    }
}
