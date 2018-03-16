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
            //        view.FormClosed;
            //    }
            //};

            view.Show();

            // todo: delete this once OpenController has been implemented
            string URL = "http://ice.users.coe.utah.edu/";
            string nickname = "Adam";
            string userID = "e8cce19e-da8b-4a00-8861-ceb13e2e55aa";
            int gameLength = 119;
            string gameID = "G1549";
            StartGame(URL, nickname, userID, gameLength, gameID);
            view.Close();
        }

        private void StartGame(string URL, string nickname, string userID, int gameLength, string gameID)
        {
            GameView view = new GameView();
            // todo: where to work with gameLength?
            GameController controller = new GameController(URL, nickname, userID, gameID, view);

            view.AddWord += (sender, e) => controller.AddWordToGame(sender, e);
            view.CancelPushed += Start;
            controller.NextPhase += (sender, e) => StartScore(e.GameID);
            view.FormClosed += (sender, e) => ExitThread();

            System.Timers.Timer timer = new System.Timers.Timer(1000);
            // todo: should GameController.Refresh have some parameters?
            timer.Elapsed += (sender, e) => controller.Refresh();
            controller.Refresh();

            view.Show();
            view.Close();
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

            view.CancelPushed += Start;
            view.FormClosed += (sender, e) => ExitThread();

            view.Show();
        }
    }
}
