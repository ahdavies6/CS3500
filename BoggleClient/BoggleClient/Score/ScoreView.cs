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
        }

        /// <summary>
        /// Restarts the application and returns it to the open screen
        /// </summary>
        public event Action CancelPushed;
    }
}
