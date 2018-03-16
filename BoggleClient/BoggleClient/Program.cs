using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoggleClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // todo: uncomment all of this as soon as the Controllers have been implemented
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            Context context = Context.GetContext();
            context.Start();
            Application.Run(context);

            // todo: delete this once the Controllers have been implemented
            //MainOpen();
            //MainGame();
            //MainScore();
        }

        // todo: delete region as soon as the Controllers have been implemented
        #region OpenForms

        static void MainOpen()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Open.OpenView view = new Open.OpenView();
            Application.Run(view);
        }

        static void MainGame()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Game.GameView view = new Game.GameView();
            Application.Run(view);
        }

        static void MainScore()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Score.ScoreView view = new Score.ScoreView();
            Application.Run(view);
        }

        #endregion
    }
}
