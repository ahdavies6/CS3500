using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetGUI;
using System.IO;
using SS;

namespace ControllerTester
{
    [TestClass]
    public class Tests
    {
        // todo: delete this
        // holy grail:
        //using (StringWriter sw = new StringWriter())
        //using (StringReader sr = new StringReader(sw.ToString()))
        //{
        //    IView view = new SimpleView();
        //    Controller controller = new Controller(sr, sw, view);
        //}

        [TestMethod]
        public void DeleteMe()
        {
            Spreadsheet ss = new Spreadsheet(new StringReader(""), new System.Text.RegularExpressions.Regex(".*"));
        }

        [TestMethod]
        public void NewTest1()
        {
            // expected exception: unknown, but it's whatever trying to make a Spreadsheet from an
            // empty file throws
            using (StringWriter sw = new StringWriter())
            using (StringReader sr = new StringReader("")) // todo: MAKE SURE THIS WORKS LATER
            {
                IView view = new SimpleView();
               // Controller controller = new Controller(sr, sw, view);
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
                //Controller controller = new Controller(sr, view);
            }
        }

        [TestMethod]
        public void NewTest3()
        {

        }
    }
}
