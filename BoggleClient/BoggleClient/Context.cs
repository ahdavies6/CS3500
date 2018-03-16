using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BoggleClient.Game;
using BoggleClient.Open;
using BoggleClient.Score;
using System.Drawing;
using System.Threading;

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
            //OpenView view = new OpenView();
            //StartOpen(view);
            StartOpen();
        }

        /// <summary>
        /// Helper method returns where the Top-Left location Point should be for Control
        /// (within its Parent container).
        /// </summary>
        public static Point TLPointCenterDynamic(Control child)
        {
            try
            {
                Control parent = child.Parent;

                int x = (parent.ClientSize.Width - child.ClientSize.Width) / 2;
                int y = (parent.ClientSize.Height - child.ClientSize.Height) / 2;

                return new Point(x, y);
            }
            catch
            {
                return new Point(0, 0);
            }
        }

        //private void StartOpen(OpenView view)
        private void StartOpen()
        {
            OpenView view = new OpenView();
            view.Show();
            OpenController controller = new OpenController(view);

            view.RegisterUser += controller.Register;
            view.SearchGame += controller.Search;
            view.CancelRegister += controller.Cancel;
            view.CancelSearch += controller.Cancel;

            view.FormClosed += (sender, e) => ExitThread();

            // remove deprecated:
            //view.NextState += (sender, e) => StartGame(e.UserToken, e.URL, e.Nickname);

            // todo: implement an event in Controller that is fired when a game is found and (roughly)
            // follows this spec: "event ...EventArgs GameFound", where EventArgs contains:
            //     URL, Nickname, UserID, GameLength, GameID
            controller.NextPhase += (sender, e) =>
            {
                if (e.URL != null && e.Nickname != null)
                {
                    StartGame(e.URL, e.Nickname, e.UserID, e.GameLength, e.GameID);
                    view.Hide();
                }
            };
        }

        //private void StartGame(object o)
        //{
        //    if (o is OpenViewEventArgs)
        //    {
        //        OpenViewEventArgs e = (OpenViewEventArgs)o;
        //        StartGame(e.URL, e.Nickname, e.UserID, e.GameLength, e.GameID);
        //    }
        //}

        private void StartGame(string URL, string nickname, string userID, int gameLength, string gameID)
        {
            GameView view = new GameView();
            view.Show();
            // todo: where to work with gameLength?
            GameController controller = new GameController(URL, nickname, userID, gameID, view);

            view.AddWord += (sender, e) => controller.AddWordToGame(sender, e);
            view.CancelPushed += StartOpen;
            controller.NextPhase += (sender, e) => StartScore(e.GameID);
            view.FormClosed += (sender, e) => ExitThread();

            System.Timers.Timer timer = new System.Timers.Timer(1000);
            // todo: should GameController.Refresh have some parameters?
            timer.Start();
            timer.Elapsed += (sender, e) => view.Invoke(new Action(controller.Refresh));
            timer.AutoReset = true;
            controller.Refresh();
        }

        // todo: pick from these two constructors:
        //private void StartScore(string playerName, int playerScore, string[] playerWords, int[] playerScores, 
        //    string opponentName, int opponentScore, string[] opponentWords, int[] opponentScores)
        private void StartScore(string gameID)
        {
            ScoreView view = new ScoreView();
            // todo: remove nulls
            string URL = "http://ice.users.coe.utah.edu/";
            ScoreController controller = new ScoreController(view, null, gameID, URL);

            view.CancelPushed += StartOpen;
            view.FormClosed += (sender, e) => ExitThread();

            view.Show();
        }

        protected override void OnMainFormClosed(object sender, EventArgs e)
        {
            //context.Start();
            //Application.Run(context);
        }
    }
}
