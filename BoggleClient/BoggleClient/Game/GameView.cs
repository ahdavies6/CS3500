using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoggleClient.Game
{
    public partial class GameView : Form, IGameView
    {
        public GameView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event that gets fired when a new word is added
        /// </summary>
        public event Action<string> AddWord;

        /// <summary>
        /// Event that gets fired if the cancel button is clicked
        /// </summary>
        public event Action CancelPushed;

        /// <summary>
        /// Moves the game to the score state
        /// </summary>
        public event NextStateEventHandler NextState;

        /// <summary>
        /// Method that creates a set of labels for the dice passed through by DiceConfigs
        /// </summary>
        public void GenerateLabels(string DiceConfig)
        {
            throw new NotImplementedException();
        }
    }
}
