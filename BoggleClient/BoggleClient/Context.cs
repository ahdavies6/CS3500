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

            FormClosedEventHandler exitDel = delegate (object sender, FormClosedEventArgs e) { ExitThread(); };
            view.FormClosed += exitDel;
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
                    view.FormClosed -= exitDel;
                    view.Close();
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
            FormClosedEventHandler exitDel = delegate (object sender, FormClosedEventArgs e) { ExitThread(); };
            view.CancelPushed += () => {
                StartOpen();
                view.FormClosed -= exitDel;
                view.Close();
                controller.Cancel();
            };
            view.FormClosed += exitDel;

            controller.NextPhase += (sender, e) =>
            {
                StartScore(e.GameID, e.URL);
                view.FormClosed -= exitDel;
                view.Close();
            };

            System.Timers.Timer timer = new System.Timers.Timer(1000);
            // todo: should GameController.Refresh have some parameters?
            controller.NextPhase += (sender, e) => timer.Stop();
            view.CancelPushed += () => timer.Stop();
            view.FormClosed += (sender, e) => timer.Stop();
            timer.Start();
            timer.Elapsed += (sender, e) => view.Invoke(new Action(controller.Refresh));
            timer.AutoReset = true;
            controller.Refresh();
        }

        // todo: pick from these two constructors:
        //private void StartScore(string playerName, int playerScore, string[] playerWords, int[] playerScores, 
        //    string opponentName, int opponentScore, string[] opponentWords, int[] opponentScores)
        private void StartScore(string gameID, string URL)
        {
            ScoreView view = new ScoreView();
            ScoreController controller = new ScoreController(view, gameID, URL);
            FormClosedEventHandler exitDel = delegate(object sender, FormClosedEventArgs e) { ExitThread(); };
            view.CancelPushed += () => {
                StartOpen();
                view.FormClosed -= exitDel;
                view.Close();
            } ;
            view.FormClosed += exitDel;
            view.Show();
        }
    }
}
