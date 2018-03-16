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
    // todo: doc comments
    public partial class GameView : Form, IGameView
    {
        /// <summary>
        /// Opens the Form
        /// </summary>
        public GameView()
        {
            InitializeComponent();

            RemainingDataLabel.Parent = RemainingBar;
            RemainingDataLabel.Location = Context.TLPointCenterDynamic(RemainingDataLabel);
        }

        /// <summary>
        /// Event that gets fired when a new word is added
        /// </summary>
        public event AddWordEventHandler AddWord;

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
        public void GenerateLabels(string diceString)
        {
            TopLeftLabel.Text = diceString[0].ToString();
            TopMiddleLabel.Text = diceString[1].ToString();
            TopRightLabel.Text = diceString[2].ToString();

            MiddleLeftLabel.Text = diceString[3].ToString();
            MiddleMiddleLabel.Text = diceString[4].ToString();
            MiddleRightLabel.Text = diceString[5].ToString();

            BottomLeftLabel.Text = diceString[6].ToString();
            BottomMiddleLabel.Text = diceString[7].ToString();
            BottomRightLabel.Text = diceString[8].ToString();
        }

        // todo: get GameController to call all of these
        #region Data We Need From GameController

        public string PlayerName
        {
            set { PlayerNameLabel.Text = value; }
        }

        public int PlayerScore
        {
            set { PlayerScoreLabel.Text = value.ToString(); }
        }

        public string OpponentName
        {
            set { OpponentNameLabel.Text = value.ToString(); }
        }

        public int OpponentScore
        {
            set { OpponentScoreLabel.Text = value.ToString(); }
        }

        private int beginTime;

        public int TimeRemaining
        {
            set
            {
                if (beginTime == default(int))
                {
                    beginTime = value;
                }

                int minutes = value / 60;
                int seconds = value % 60;
                RemainingDataLabel.Text = minutes.ToString() + ":" + seconds.ToString();

                RemainingBar.Value = (value / beginTime) * 100;
            }
        }
        
        #endregion

        private void WordTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // (char)13 is enter
            {
                AddWordEventArgs args = new AddWordEventArgs(WordTextbox.Text);
                AddWord?.Invoke(this, args);

                WordTextbox.ResetText();
            }
        }

        private void CancelGameButton_Click(object sender, EventArgs e)
        {
            CancelPushed?.Invoke();
        }
    }
}
