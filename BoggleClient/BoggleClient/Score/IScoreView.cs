using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleClient.Score
{
    // todo: add anything else? (inc doc comments)

    interface IScoreView
    {
        /// <summary>
        /// Restarts the application and returns it to the open screen
        /// </summary>
        event Action CancelPushed;
    }
}
