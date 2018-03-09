using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetGUI;
using SpreadsheetController;
using System.IO;

namespace ControllerTester
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void NewTest1()
        {
            using (StringReader sr = new StringReader(""))
            {
                IView view = new SimpleView();
                Controller controller = new Controller(sr, view);
            }
        }

        [TestMethod]
        public void NewTest2()
        {
            using (StringReader sr = new StringReader(""))
            {
                IView view = new SimpleView();
                Controller controller = new Controller(sr, view);

                string temp = sr.ToString();
            }
        }

        [TestMethod]
        public void NewTest3()
        {

        }
    }
}
