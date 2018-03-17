using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoggleClient.Score
{
    public partial class ScoreView : Form, IScoreView
    {
        /// <summary>
        /// Opens the Form
        /// </summary>
        public ScoreView()
        {
            InitializeComponent();

            int scorePanelSizeX = (int)(this.ClientSize.Width);
            int scorePanelSizeY = (int)(this.ClientSize.Height * 0.8);
            ScorePanel.Size = new Size(scorePanelSizeX, scorePanelSizeY);
            ScorePanel.Location = Context.TLPointCenterDynamic(ScorePanel);
        }

        /// <summary>
        /// Restarts the application and returns it to the open screen
        /// </summary>
        public event Action CancelPushed;

        #region ScoreController Handled Data

        /// <summary>
        /// Player's username
        /// </summary>
        public string PlayerName
        {
            set { AbovePlayerWordsLabel.Text = value; }
        }

        /// <summary>
        /// Player's final score
        /// </summary>
        public int PlayerScore
        {
            set { AbovePlayerScoresLabel.Text = value.ToString(); }
        }

        /// <summary>
        /// Converts an array string to a single string
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private string ArrayToString(string[] array)
        {
            string result = "";

            foreach (string piece in array)
            {
                result = result + piece + "\r\n";
            }

            return result;
        }

        /// <summary>
        /// Converts an integer array to a string array
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private string ArrayToString(int[] array)
        {
            string result = "";

            foreach (int piece in array)
            {
                result = result + piece.ToString() + "\r\n";
            }

            return result;
        }

        /// <summary>
        /// The words the player submitted
        /// </summary>
        public string[] PlayerWords
        {
            set { PlayerWordsDataLabel.Text = ArrayToString(value); }
        }

        /// <summary>
        /// The scores the player earned
        /// </summary>
        public int[] PlayerScores
        {
            set { PlayerScoresDataLabel.Text = ArrayToString(value); }
        }

        /// <summary>
        /// The opponent's name
        /// </summary>
        public string OpponentName
        {
            set { AboveOpponentWordsLabel.Text = value; }
        }

        /// <summary>
        /// The opponent's score
        /// </summary>
        public int OpponentScore
        {
            set { AboveOpponentScoresLabel.Text = value.ToString(); }
        }

        /// <summary>
        /// The words the opponent played
        /// </summary>
        public string[] OpponentWords
        {
            set { OpponentWordsDataLabel.Text = ArrayToString(value); }
        }

        /// <summary>
        /// The scores the opponent earned
        /// </summary>
        public int[] OpponentScores
        {
            set { OpponentScoresDataLabel.Text = ArrayToString(value); }
        }

        #endregion

        /// <summary>
        /// Cancels the active game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReturnButton_Click(object sender, EventArgs e)
        {
            CancelPushed?.Invoke();
        }

        /// <summary>
        /// Displays a help message for the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Click \"Return to Main Menu\" to return to the main menu.");
        }
    }
}
