using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BoggleClient.Game;
using BoggleClient.Open;
using BoggleClient.Score;

namespace BoggleClient
{
    // todo: add missing doc comments

    /// <summary>
    /// Singleton-pattern "meta-controller" that activates different Views (Forms) and Controllers in
    /// the client, per user interaction.
    /// </summary>
    class Context : ApplicationContext
    {
        /// <summary>
        /// Singleton pattern field exists independently of its instance.
        /// </summary>
        private static Context context;

        private Context() { }

        public static Context GetContext()
        {
            if (context == null)
            {
                context = new Context();
            }
            return context;
        }

        public void Start()
        {
            StartOpen();
        }

        // todo: revisit after implementing OpenController and GameController
        private void StartOpen()
        {
            OpenView view = new OpenView();
            // pass OpenController's constructor the openView
            OpenController controller = new OpenController();

            view.FormClosed += (sender, e) => ExitThread();
            view.CancelPushed += Start;
            // pass nickname in here too once it's a parameter for GameController's constructor
            view.NextState += (sender, e) => StartGame(e.UserID, e.URL, e.Nickname);

            view.Show();
        }

        // todo: revisit after implementing GameController and ScoreController
        private void StartGame(string userID, string URL, string nickname)
        {
            GameView view = new GameView();
            // change to this:
            //GameController controller = new GameController(userID, URL, view, nickname);
            GameController controller = new GameController(userID, URL, view);

            view.FormClosed += (sender, e) => ExitThread();
            view.CancelPushed += Start;
            // after ScoreController implementation, StartScore should have some parameters, right?
            view.NextState += (sender, e) => StartScore();

            System.Timers.Timer timer = new System.Timers.Timer(1000);
            // should GameController.Refresh have some parameters?
            timer.Elapsed += (sender, e) => controller.Refresh();

            view.Show();
        }

        // todo: revisit after implementing ScoreController
        private void StartScore()
        {
            ScoreView view = new ScoreView();
            // pass ScoreController the view
            // pass ScoreController more params (e.g. gameID or userID)?
            ScoreController controller = new ScoreController();

            view.FormClosed += (sender, e) => ExitThread();
            view.CancelPushed += Start;

            view.Show();
        }
    }
}
