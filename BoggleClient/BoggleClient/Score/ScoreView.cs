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
    public partial class ScoreView : Form, Score.ICloseView
    {
        public ScoreView()
        {
            InitializeComponent();
        }

        public event Action CancelPushed;
        public event Action NewGame;
    }
}
