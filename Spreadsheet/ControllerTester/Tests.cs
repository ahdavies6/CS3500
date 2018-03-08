using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetGUI;
using SpreadsheetController;

namespace ControllerTester
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void NewTest1()
        {
            IView view = new SimpleView();
            Controller controller = new Controller("this.ss", view);

            // todo: test NewFile
        }

        [TestMethod]
        public void NewTest2()
        {

        }

        [TestMethod]
        public void NewTest3()
        {

        }
    }
}
