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

        /// <summary>
        /// Returns the active context
        /// </summary>
        private Context() { }

        /// <summary>
        /// Returns the active context if one exists.
        /// Otherwise, creates and returns a new context
        /// </summary>
        /// <returns></returns>
        public static Context GetContext()
        {
            if (context == null)
            {
                context = new Context();
            }
            return context;
        }

        /// <summary>
        /// Starts a new boggle client
        /// </summary>
        public void Start()
        {
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

        /// <summary>
        /// Opens the server connection GUI
        /// </summary>
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

        /// <summary>
        /// Starts a new game with server URL, username nickname, user ID token userID, game duration
        /// gameLength, and game ID token gameID.
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="nickname"></param>
        /// <param name="userID"></param>
        /// <param name="gameLength"></param>
        /// <param name="gameID"></param>
        private void StartGame(string URL, string nickname, string userID, int gameLength, string gameID)
        {
            GameView view = new GameView();
            view.Show();
            GameController controller = new GameController(URL, nickname, userID, gameID, view);

            view.AddWord += (sender, e) => controller.AddWordToGame(sender, e);
            FormClosedEventHandler exitDel = delegate (object sender, FormClosedEventArgs e) { ExitThread(); };
            view.CancelPushed += () =>
            {
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
            controller.NextPhase += (sender, e) => timer.Stop();
            view.CancelPushed += () => timer.Stop();
            view.FormClosing += (sender, e) => timer.Stop();
            timer.Start();
            timer.Elapsed += (sender, e) =>
            {
                lock (view)
                {
                    if (!view.IsDisposed)
                    {
                        view.Invoke(new Action(() => controller?.Refresh(false)));

                    };
                }
            };
            timer.AutoReset = true;
            controller.Refresh(true);
        }

        /// <summary>
        /// Starts the score GUI with the gameID token at the server URL
        /// </summary>
        /// <param name="gameID"></param>
        /// <param name="URL"></param>
        private void StartScore(string gameID, string URL)
        {
            ScoreView view = new ScoreView();
            ScoreController controller = new ScoreController(view, gameID, URL);
            FormClosedEventHandler exitDel = delegate (object sender, FormClosedEventArgs e) { ExitThread(); };
            view.CancelPushed += () =>
            {
                StartOpen();
                view.FormClosed -= exitDel;
                view.Close();
            };
            view.FormClosed += exitDel;
            view.Show();
        }
    }
}
