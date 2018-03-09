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
            // expected exception: unknown, but it's whatever trying to make a Spreadsheet from an
            // empty file throws
            using (StringReader sr = new StringReader(""))
            {
                IView view = new SimpleView();
                Controller controller = new Controller(sr, view);
            }
        }

        [TestMethod]
        public void NewTest2()
        {
            // expected exception: unknown, but it's whatever trying to make a Spreadsheet from an
            // invalid file throws
            using (StringReader sr = new StringReader(""))
            {
                IView view = new SimpleView();
                Controller controller = new Controller(sr, view);
            }
        }

        [TestMethod]
        public void NewTest3()
        {

        }
    }
}
