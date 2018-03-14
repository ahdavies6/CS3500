using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoggleClient
{
    // todo: doc comments
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

        // todo: this
        // do I need to use this "wrapper", or should I just make StartOpen public?
        public void Start()
        {
            StartOpen();
        }

        // todo: this
        private void StartOpen()
        {
            OpenView view = new OpenView();
            // pass OpenController the openView
            OpenController controller = new OpenController();

            //view.Closed += () => { ExitThread(); };

            // hook an event for user exiting
            //view.Cancel += Start;

            // hook an event for user starting a game
            // add params (for StartGame's GameController)
            //view.Next += StartGame();

            view.Show();
        }

        // todo: this
        private void StartGame()
        {
            GameView view = new GameView();
            // pass GameController the gameView
            // pass GameController more params?
            GameController controller = new GameController();

            System.Timers.Timer timer = new System.Timers.Timer(1000);
            //timer.Elapsed += (sender, e) => controller.Refresh();

            //view.Closed += () => { ExitThread(); };

            // hook an event for user exiting
            //view.Cancel += Start;

            // hook an event for user completing a game
            // add params (for StartScore's ScoreController)
            //view.Next += StartScore;

            view.Show();
        }

        // todo: this
        private void StartScore()
        {
            ScoreView view = new ScoreView();
            // pass ScoreController the view
            // pass ScoreController more params?
            ScoreController controller = new ScoreController();

            //view.Closed += () => { ExitThread(); };

            // hook an event for user exiting
            //view.Cancel += Start;

            view.Show();
        }
    }
}
