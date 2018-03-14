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
    // todo: write doc comments
    class Context : ApplicationContext
    {
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

        // todo: do I need to use this "wrapper", or should I just make StartOpen public?
        public void Start()
        {
            StartOpen();
        }

        // todo: revisit after implementing OpenView
        private void StartOpen()
        {
            OpenView view = new OpenView();
            // pass OpenController the openView?
            OpenController controller = new OpenController();

            view.FormClosed += (sender, e) => ExitThread();
            // revisit this:
            view.CancelPushed += Start;
            view.NextState += (sender, e) => StartGame(e.UserID, e.URL, e.View);

            view.Show();
        }

        // todo: do I need to pull in an IGameView here (requires understanding StartOpen's NextState and
        // GameView better), or can I just use the old first line?
        private void StartGame(string userID, string URL, IGameView gameView)
        {
            // todo: go back to this? remove deprecated? does the replaced line's cast even work?
            //GameView view = new GameView();
            GameView view = (GameView)gameView;
            GameController controller = new GameController(userID, URL, gameView);

            view.FormClosed += (sender, e) => ExitThread();
            view.CancelPushed += Start;
            // todo: revisit upon ScoreView implementation; StartScore should have parameters
            view.NextState += (sender, e) => StartScore();

            System.Timers.Timer timer = new System.Timers.Timer(1000);
            // todo: GameController.Refresh should have some parameters, right?
            timer.Elapsed += (sender, e) => controller.Refresh();

            view.Show();
        }

        // todo: should this method have parameters (which will be passed as the properties of
        // Game.NextStateEventArgs)?
        private void StartScore()
        {
            ScoreView view = new ScoreView();
            // pass ScoreController the view?
            // pass ScoreController more params?
            ScoreController controller = new ScoreController();

            view.FormClosed += (sender, e) => ExitThread();
            view.CancelPushed += Start;
            // todo: either hook view.NewGame, or remove it from ScoreView and IScoreView

            view.Show();
        }
    }
}
