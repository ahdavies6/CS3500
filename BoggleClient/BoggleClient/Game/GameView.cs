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
    /// <summary>
    /// Windows form containing the boggle game client
    /// </summary>
    public partial class GameView : Form, IGameView
    {
        /// <summary>
        /// Opens the Form
        /// </summary>
        public GameView()
        {
            InitializeComponent();
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
            BoggleFace1.Text = diceString[0].ToString();
            BoggleFace2.Text = diceString[1].ToString();
            BoggleFace3.Text = diceString[2].ToString();
            BoggleFace4.Text = diceString[3].ToString();

            BoggleFace5.Text = diceString[4].ToString();
            BoggleFace6.Text = diceString[5].ToString();
            BoggleFace7.Text = diceString[6].ToString();
            BoggleFace8.Text = diceString[7].ToString();

            BoggleFace9.Text = diceString[8].ToString();
            BoggleFace10.Text = diceString[9].ToString();
            BoggleFace11.Text = diceString[10].ToString();
            BoggleFace12.Text = diceString[11].ToString();

            BoggleFace13.Text = diceString[12].ToString();
            BoggleFace14.Text = diceString[13].ToString();
            BoggleFace15.Text = diceString[14].ToString();
            BoggleFace16.Text = diceString[15].ToString();

            HashSet<Label> labels = new HashSet<Label> { BoggleFace1, BoggleFace2, BoggleFace3, BoggleFace4,
                BoggleFace5, BoggleFace6, BoggleFace7, BoggleFace8, BoggleFace9, BoggleFace10, BoggleFace11,
                BoggleFace12, BoggleFace13, BoggleFace14, BoggleFace15, BoggleFace16};

            foreach(Label l in labels)
            {
                if (l.Text.Equals("Q"))
                {
                    l.Text = "Qu";
                }
            }
        }

        /// <summary>
        /// The player's name
        /// </summary>
        public string PlayerName
        {
            set { PlayerNameLabel.Text = value; }
        }

        /// <summary>
        /// The player's score
        /// </summary>
        public int PlayerScore
        {
            set { PlayerScoreLabel.Text = value.ToString(); }
        }

        /// <summary>
        /// The opponent's name
        /// </summary>
        public string OpponentName
        {
            set { OpponentNameLabel.Text = value.ToString(); }
        }

        /// <summary>
        /// The opponent's score
        /// </summary>
        public int OpponentScore
        {
            set { OpponentScoreLabel.Text = value.ToString(); }
        }

        /// <summary>
        /// The time the game started at
        /// </summary>
        private int beginTime;

        /// <summary>
        /// The amount of time remaining in the game
        /// </summary>
        private int timeRemaining;

        /// <summary>
        /// Public wrapper property to timeRemaining; also displays time.
        /// </summary>
        public int TimeRemaining
        {
            get
            {
                return timeRemaining;
            }
            set
            {
                timeRemaining = value;

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

        /// <summary>
        /// Submits an addword request when the player presses enter in the textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WordTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // (char)13 is enter
            {
                AddWordEventArgs args = new AddWordEventArgs(WordTextbox.Text);
                AddWord?.Invoke(this, args);

                WordTextbox.ResetText();
            }
        }

        /// <summary>
        /// Cancels the active game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelGameButton_Click(object sender, EventArgs e)
        {
            CancelPushed?.Invoke();
        }

        /// <summary>
        /// Shows the user a help menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string messageText = "To submit a word, type it into the \"Word:\" textbox, and press enter. \r\n" +
                "To cancel the game, click the \"Cancel Game\" button. \r\n" +
                "Once the timer reaches zero, the game will terminate, and all of the words that you " +
                "and your opponent played will be displayed, along with the scores that they earned. \r\n" +
                "For more information on the rules of Boggle, visit: https://en.wikipedia.org/wiki/Boggle";
            MessageBox.Show(messageText);
        }
    }
}
