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

        private void StartOpen()
        {
            OpenView view = new OpenView();
            // pass OpenController's constructor the openView
            OpenController controller = new OpenController();

            // todo: something along these lines:
            //view.ConnectToServer += (sender, e) => controller.Connect(e.URL, e.Nickname);
            //view.SearchGame += (sender, e) => controller.Search(e.GameLength);
            //view.CancelPushed += () => controller.StopConnect();
            //view.CancelSearch += () => controller.StopSearch();

            view.FormClosed += (sender, e) => ExitThread();

            // remove deprecated:
            //view.NextState += (sender, e) => StartGame(e.UserToken, e.URL, e.Nickname);

            // todo: implement an event in Controller that is fired when a game is found and (roughly)
            // follows this spec: "event ...EventArgs GameFound", where EventArgs contains:
            //     URL, Nickname, UserID, GameLength, GameID
            //controller.GameFound += (sender, e) =>
            //{
            //    if (URL != null && Nickname != null)
            //    {
            //        StartGame(e.URL, e.Nickname, e.UserID, e.GameLength, e.GameID);
            //    }
            //};

            view.Show();
        }

        private void StartGame(string URL, string nickname, string userID, int gameLength, string gameID)
        {
            GameView view = new GameView();
            GameController controller = new GameController(userID, URL, view);
            // todo: rework the above to accept something like:
            //GameController controller = new GameController(URL, nickname, userID, gameLength, gameID, view);

            // todo: add something like these once GameController is implemented:
            //view.AddWord += controller.AddWord();

            view.CancelPushed += Start; 

            // todo: StartScore params once GameController is implemented
            //view.NextState += (sender, e) => StartScore();

            view.FormClosed += (sender, e) => ExitThread();

            System.Timers.Timer timer = new System.Timers.Timer(1000);

            // todo: should GameController.Refresh have some parameters?
            timer.Elapsed += (sender, e) => controller.Refresh();

            view.Show();
        }

        // todo: pick from these two constructors:
        //private void StartScore(string playerName, int playerScore, string[] playerWords, int[] playerScores, 
        //    string opponentName, int opponentScore, string[] opponentWords, int[] opponentScores)
        private void StartScore(string gameID)
        {
            ScoreView view = new ScoreView();
            // pass ScoreController the view
            // pass ScoreController more params (e.g. gameID or userID)?
            ScoreController controller = new ScoreController();

            view.CancelPushed += Start;
            view.FormClosed += (sender, e) => ExitThread();

            view.Show();
        }
    }
}
