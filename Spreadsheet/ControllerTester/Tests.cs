using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetGUI;
using System.IO;
using SS;
using static ControllerTester.SimpleView;

namespace ControllerTester
{
    [TestClass]
    public class Tests
    {
        /// <summary>
        /// Tests opening a new gui window. The context isn't set so a NullReference Should be thrown
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestHandleNew()
        {
            SimpleView view = new SimpleView("");
            Controller cont = new Controller(view);
            view.FireNewFile();
        }

        /// <summary>
        /// Tests the open function. Should throw a NullReference since the context is not defined
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestHandleOpen()
        {
            SimpleView view = new SimpleView("");
            Controller cont = new Controller(view);
            view.FireOpenFile("p");
        }

        /// <summary>
        /// Tests the HandleSave, handleChange SetCellContents (controller version, not spreadsheet) 
        /// </summary>
        [TestMethod]
        public void TestHandleSave()
        {
            SimpleView view = new SimpleView("");
            Controller cont = new Controller(view);
            view.FireSetContents("A1", "Test");
            view.FireSetContents("A2", (2.0).ToString());
            view.FireSetContents("A3", (2.0).ToString());
            view.FireSetContents("A4", "=A2+A3");
            view.FireSetContents("A5", "=A2+A3+A1");

            view.FireSaveFile(Directory.GetCurrentDirectory() + @"\savetest.ss");
            SimpleView view2 = new SimpleView(Directory.GetCurrentDirectory() + @"\savetest.ss");
            Controller cont2 = new Controller(view2);
            view2.FireSetContents("A1", "Test");
            view2.FireSetContents("A2", (2.0).ToString());
            view2.FireSetContents("A3", (2.0).ToString());
            view2.FireSetContents("A4", "=A2+A3");
            view2.FireSetContents("A5", "=A2+A3+A1");

            view2.FireSaveFile(Directory.GetCurrentDirectory() + @"\savetest2.ss");
            AbstractSpreadsheet ss;
            AbstractSpreadsheet ss2;
            using (StreamReader f = new StreamReader(Directory.GetCurrentDirectory() + @"\savetest.ss"))
            using (StreamReader f2 = new StreamReader(Directory.GetCurrentDirectory() + @"\savetest2.ss"))
            {
                ss = new Spreadsheet(f, new System.Text.RegularExpressions.Regex(".*"));
                ss2 = new Spreadsheet(f2, new System.Text.RegularExpressions.Regex(".*"));
            }

            foreach (string cell in ss.GetNamesOfAllNonemptyCells())
            {
                Assert.AreEqual(ss.GetCellValue(cell).ToString(), ss2.GetCellValue(cell).ToString());
                Assert.AreEqual(ss.GetCellContents(cell).ToString(), ss2.GetCellContents(cell).ToString());
            }

            foreach (string cell in ss2.GetNamesOfAllNonemptyCells())
            {
                Assert.AreEqual(ss.GetCellValue(cell).ToString(), ss2.GetCellValue(cell).ToString());
                Assert.AreEqual(ss.GetCellContents(cell).ToString(), ss2.GetCellContents(cell).ToString());
            }
        }

        /// <summary>
        /// Tests the HandleClose both cases when there are unsaved changes and when there are no unsaved changes
        /// </summary>
        [TestMethod]
        public void TestHandleClose()
        {
            SimpleView view = new SimpleView("");
            Controller cont = new Controller(view);
            view.FireSetContents("A1", "Test");
            view.FireSetContents("A2", (2.0).ToString());
            view.FireSetContents("A3", (2.0).ToString());
            view.FireSetContents("A4", "=A2+A3");
            view.FireSetContents("A5", "=A2+A3+A1");

            // Unsaved changes, but user decides to quit anyway.
            view.Close = true;
            view.FireCloseFile();

            view = new SimpleView("");
            cont = new Controller(view);
            view.FireSetContents("A1", "Test");
            view.FireSetContents("A2", (2.0).ToString());
            view.FireSetContents("A3", (2.0).ToString());
            view.FireSetContents("A4", "=A2+A3");
            view.FireSetContents("A5", "=A2+A3+A1");

            // Unsaved changes, and user decides to stay in view.
            view.Close = false;
            view.FireCloseFile();

            view = new SimpleView("");
            cont = new Controller(view);
            view.FireSetContents("A1", "Test");
            view.FireSetContents("A2", (2.0).ToString());
            view.FireSetContents("A3", (2.0).ToString());
            view.FireSetContents("A4", "=A2+A3");
            view.FireSetContents("A5", "=A2+A3+A1");

            view.FireSaveFile("testhandle.ss");
            view.Close = true;
            view.FireCloseFile();
        }

        /// <summary>
        /// Tests for if an invalid file is attemptting to be read
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ExceptionForTesting))]
        public void TestInvalidFileRead()
        {
            SimpleView view = new SimpleView("badfile.txt");
            view.ThrowException = true;
            Controller cont = new Controller(view);
            view.FireSetContents("A1", "Test");
            view.FireSetContents("A2", (2.0).ToString());
            view.FireSetContents("A3", (2.0).ToString());
            view.FireSetContents("A4", "=A2+A3");
            view.FireSetContents("A5", "=A2+A3+A1");
        }

        /// <summary>
        /// Tests for if an invalid file is attemptting to be read without throwing an exception in UnableToLoad
        /// </summary>
        [TestMethod]
        public void TestInvalidFileReadComplete()
        {
            SimpleView view = new SimpleView("badfile.txt");
            view.ThrowException = false;
            Controller cont = new Controller(view);

        }

        /// <summary>
        /// Mainly for code coverage 
        /// Case when there is a circular depedency and a FormulaFormatException in the graph 
        /// </summary>
        [TestMethod]
        public void TestCircularException()
        {
            SimpleView view = new SimpleView("");
            Controller cont = new Controller(view);
            view.FireSetContents("A1", "Test");
            view.FireSetContents("A2", (2.0).ToString());
            view.FireSetContents("A3", (2.0).ToString());
            view.FireSetContents("A4", "=A2+A3");
            view.FireSetContents("A5", "=A2+A3+A5");
            view.FireSetContents("A6", "=A2+A3+A/");

        }
    }
}
