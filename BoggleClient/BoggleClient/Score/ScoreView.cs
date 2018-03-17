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

        // todo: get ScoreController to call all of these
        #region Data We Need From ScoreController

        public string PlayerName
        {
            set { AbovePlayerWordsLabel.Text = value; }
        }

        public int PlayerScore
        {
            set { AbovePlayerScoresLabel.Text = value.ToString(); }
        }

        private string ArrayToString(string[] array)
        {
            string result = "";

            foreach (string piece in array)
            {
                result = result + piece + "\r\n";
            }

            return result;
        }

        private string ArrayToString(int[] array)
        {
            string result = "";

            foreach (int piece in array)
            {
                result = result + piece.ToString() + "\r\n";
            }

            return result;
        }


        public string[] PlayerWords
        {
            set { PlayerWordsDataLabel.Text = ArrayToString(value); }
        }

        public int[] PlayerScores
        {
            set { PlayerScoresDataLabel.Text = ArrayToString(value); }
        }

        public string OpponentName
        {
            set { AboveOpponentWordsLabel.Text = value; }
        }

        public int OpponentScore
        {
            set { AboveOpponentScoresLabel.Text = value.ToString(); }
        }

        public string[] OpponentWords
        {
            set { OpponentWordsDataLabel.Text = ArrayToString(value); }
        }

        public int[] OpponentScores
        {
            set { OpponentScoresDataLabel.Text = ArrayToString(value); }
        }

        #endregion

        private void ReturnButton_Click(object sender, EventArgs e)
        {
            CancelPushed?.Invoke();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Click \"Return to Main Menu\" to return to the main menu.");
        }
    }
}
