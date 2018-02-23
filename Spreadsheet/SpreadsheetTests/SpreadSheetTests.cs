//Nithin Chalapathi - u0847388 - Spring '18 - University of Utah

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using Formulas;
using System.Collections.Generic;

namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadSheetTests
    {

        public static AbstractSpreadsheet ss;

        //////////////////////////////////////////////////////////////////////////////////////
        // The following spreadsheet is used as the starting point for tests                //
        //                                                                                  //
        //      |      1      |      2      |      3      |      4      |      5      |     //
        //   --------------------------------------------------------------------------     //
        //   A  |      1      |      2      |      3      |      4      |      5      |     //
        //   --------------------------------------------------------------------------     //
        //   B  |      6      |    CS3500   |      7      |             |      8      |     //
        //   --------------------------------------------------------------------------     //
        //   C  |    A1+B1    |     A1*2    |             |             |             |     //
        //   --------------------------------------------------------------------------     //
        //   D  |             |             |    CS4150   |      A4     |             |     //
        //   --------------------------------------------------------------------------     //
        //   E  |    C1+C2    |     C1*C2   |    A3+B3    |             |             |     //
        //   --------------------------------------------------------------------------     //
        //                                                                                  //
        //////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Generates the above spreadsheet and stores it into the static variable ss
        /// 
        /// Implicitly tests to make sure SetContentsOfCell works
        /// </summary>
        public static void GenerateNewSS()
        {
            ss = new Spreadsheet();

            //Setting the first row
            for (int i = 1; i < 6; i++)
            {
                ss.SetContentsOfCell("A" + i, i.ToString());
            }

            //Setting the second row
            ss.SetContentsOfCell("B1", "6");
            ss.SetContentsOfCell("B2", "CS3500");
            ss.SetContentsOfCell("B3", "7");
            ss.SetContentsOfCell("B5", "8");

            //Set the third row
            ss.SetContentsOfCell("C1", "=A1+B1");
            ss.SetContentsOfCell("C2", "=A1*2");

            //Set the fourth row 
            ss.SetContentsOfCell("D3", "CS4150");
            ss.SetContentsOfCell("D4", "=A4");

            //Set the fifth row
            ss.SetContentsOfCell("E1", "=C1+C2");
            ss.SetContentsOfCell("E2", "=C1*C2");
            ss.SetContentsOfCell("E3", "=A3+B3");

        }

        /// <summary>
        /// Helper method that is used to evalute the formulas inside the ss
        /// This is the delgate passed to Evalute in Formula
        /// </summary>
        public static double lookup(string x)
        {
            object val = ss.GetCellContents(x);

            if (val is Formula)
            {
                return ((Formula)val).Evaluate(lookup);
            }
            else if (val is double)
            {
                return (double)val;
            }
            else if (val is string)
            {
                throw new UndefinedVariableException(x);
            }

            return -1;
        }

        ////////////////////////////////////////Begin Black box//////////////////////////////

        /// <summary>
        /// Tests GetNamesOfAllNonemptyCells using ss
        /// </summary>
        [TestMethod]
        public void TestGetNamesOfAllNonemptyCells()
        {
            GenerateNewSS();
            HashSet<string> cells = new HashSet<string>();
            //Setting the first row
            for (int i = 1; i < 6; i++)
            {
                cells.Add("A" + i);
            }

            //Add the other rows
            cells.Add("B1");
            cells.Add("B2");
            cells.Add("B3");
            cells.Add("B5");
            cells.Add("C1");
            cells.Add("C2");
            cells.Add("D3");
            cells.Add("D4");
            cells.Add("E1");
            cells.Add("E2");
            cells.Add("E3");

            foreach (string s in ss.GetNamesOfAllNonemptyCells())
            {
                Assert.IsTrue(cells.Contains(s));
                cells.Remove(s);
            }

            Assert.AreEqual(0, cells.Count);
        }


        /// <summary>
        /// Tests getting the cell content of each cell in ss
        /// </summary>
        [TestMethod]
        public void TestGetCellContent()
        {
            Dictionary<string, object> correctVals = new Dictionary<string, object>();
            GenerateNewSS();
            //first row 
            for (double i = 1; i < 6; i++)
            {
                correctVals.Add("A" + i, i);
            }

            //second row
            correctVals.Add("B1", 6.0);
            correctVals.Add("B2", "CS3500");
            correctVals.Add("B3", 7.0);
            correctVals.Add("B5", 8.0);

            //third row
            correctVals.Add("C1", "A1+B1");
            correctVals.Add("C2", "A1*2");

            //fourth row 
            correctVals.Add("D3", "CS4150");
            correctVals.Add("D4", "A4");

            //fifth row
            correctVals.Add("E1", "C1+C2");
            correctVals.Add("E2", "C1*C2");
            correctVals.Add("E3", "A3+B3");

            foreach (string cell in correctVals.Keys)
            {
                Assert.AreEqual(correctVals[cell].ToString(), ss.GetCellContents(cell).ToString());
            }

            foreach (string cell in ss.GetNamesOfAllNonemptyCells())
            {
                Assert.AreEqual(correctVals[cell].ToString(), ss.GetCellContents(cell).ToString());
                correctVals.Remove(cell);
            }

            Assert.AreEqual(0, correctVals.Count);
        }

        /// <summary>
        /// Tests that an empty cell should return an empty string
        /// </summary>
        [TestMethod]
        public void TestGetCellContentEmpty()
        {
            //Empty ss
            ss = new Spreadsheet();
            Assert.AreEqual("", ss.GetCellContents("A1"));

            //populated ss
            GenerateNewSS();
            Assert.AreEqual("", ss.GetCellContents("B4"));
        }

        /// <summary>
        /// Triggers a null case in GetCellContents
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellContentsNull()
        {
            GenerateNewSS();

            ss.GetCellContents(null);
        }

        /// <summary>
        /// Triggers an invald case in GetCellContents by using "A"
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellContentsInvalidName()
        {
            GenerateNewSS();
            ss.GetCellContents("A");
        }

        /// <summary>
        /// Tests SetContentsOfCell(str, double) when the name is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellDoubleNull()
        {
            GenerateNewSS();
            ss.SetContentsOfCell(null, "1.0");
        }

        /// <summary>
        /// Tests SetContentsOfCell(str, double) when name is invalid
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellDoubleInvalid()
        {
            GenerateNewSS();
            ss.SetContentsOfCell("A", "1.0");
        }

        /// <summary>
        /// Tests SetContentsOfCell(str, str) when the name is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellStringNullInput()
        {
            GenerateNewSS();
            ss.SetContentsOfCell(null, "test");
        }

        /// <summary>
        /// Tests SetContentsOfCell(str, str) when name is invalid
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellStringInvalidInput()
        {
            GenerateNewSS();
            ss.SetContentsOfCell("A", "test");
        }

        /// <summary>
        /// Tests SetContentsOfCell(str, str) when text is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetCellStringNullTextInput()
        {
            GenerateNewSS();
            ss.SetContentsOfCell("A1", null);
        }

        /// <summary>
        /// Tests SetContentsOfCell(str, Formula) when name is null 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellFormulaNullName()
        {
            GenerateNewSS();
            ss.SetContentsOfCell(null, "=A1");
        }

        /// <summary>
        /// Tests SetContentsOfCell(str, Formula) when name is invalid 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellFormulaInvalidName()
        {
            GenerateNewSS();
            ss.SetContentsOfCell("BC", "=A1");
        }

        /// <summary>
        /// Tests SetContentsOfCell(str, Formula) when formula has variables that are not valid cells
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestSetCellFormulaInvalidFormulaVariables()
        {
            GenerateNewSS();
            ss.SetContentsOfCell("A1", "=a+b");
        }

        /// <summary>
        /// Tests SetContentsOfCell(str, formula) when the new formula
        /// creates a circular dependency
        /// </summary>
        [TestMethod]
        public void TestSetCellFormulaCircular()
        {
            GenerateNewSS();
            try
            {
                ss.SetContentsOfCell("C1", "=E2+1");

                //Should never reach this
                Assert.Fail();
            }
            catch (CircularException e)
            {
                //Do nothing
            }

            //Graph should have reverted
            Assert.AreEqual(7, ((Formula)ss.GetCellContents("C1")).Evaluate(lookup));

            try
            {
                ss.SetContentsOfCell("A1", "=A1+A1");

                //Should never reach this
                Assert.Fail();
            }
            catch (CircularException e)
            {
                //Do Nothing
            }

        }

        /// <summary>
        /// Tests setting the cell when the new value is a double
        /// 
        /// Tests 2 cases 
        /// 1. the cell has no dependees
        /// 2. The cell has dependees
        /// </summary>
        [TestMethod]
        public void TestSetCellDouble()
        {

            //Case when the the original cell had no dependees
            GenerateNewSS();

            ISet<string> deps = ss.SetContentsOfCell("A1", "2.0");

            HashSet<string> correctDeps = new HashSet<string>();
            correctDeps.Add("A1");
            correctDeps.Add("C1");
            correctDeps.Add("C2");
            correctDeps.Add("E1");
            correctDeps.Add("E2");

            foreach (string s in deps)
            {
                Assert.IsTrue(correctDeps.Contains(s));
                correctDeps.Remove(s);
            }

            Assert.AreEqual(0, correctDeps.Count);
            Assert.AreEqual(2.0, ss.GetCellContents("A1"));

            //Case when the the original cell had dependees
            GenerateNewSS();

            deps = ss.SetContentsOfCell("C1", "2.0");

            correctDeps = new HashSet<string>();
            correctDeps.Add("C1");
            correctDeps.Add("E1");
            correctDeps.Add("E2");

            foreach (string s in deps)
            {
                Assert.IsTrue(correctDeps.Contains(s));
                correctDeps.Remove(s);
            }

            Assert.AreEqual(0, correctDeps.Count);
            Assert.AreEqual(2.0, ss.GetCellContents("C1"));

        }

        /// <summary>
        /// Tests setting the cell when the new value is a string
        ///
        /// Tests 2 cases 
        /// 1. the cell has no dependees
        /// 2. The cell has dependees
        /// </summary>
        [TestMethod]
        public void TestSetCellString()
        {
            //Case when the the original cell has no dependees
            GenerateNewSS();

            ISet<string> deps = ss.SetContentsOfCell("A1", "This Is a Test");

            HashSet<string> correctDeps = new HashSet<string>();
            correctDeps.Add("A1");
            correctDeps.Add("C1");
            correctDeps.Add("C2");
            correctDeps.Add("E1");
            correctDeps.Add("E2");

            foreach (string s in deps)
            {
                Assert.IsTrue(correctDeps.Contains(s));
                correctDeps.Remove(s);
            }

            Assert.AreEqual(0, correctDeps.Count);
            Assert.AreEqual("This Is a Test", ss.GetCellContents("A1"));

            //Case when the the original cell had  dependees
            GenerateNewSS();

            deps = ss.SetContentsOfCell("C1", "test");

            correctDeps = new HashSet<string>();
            correctDeps.Add("C1");
            correctDeps.Add("E1");
            correctDeps.Add("E2");

            foreach (string s in deps)
            {
                Assert.IsTrue(correctDeps.Contains(s));
                correctDeps.Remove(s);
            }

            Assert.AreEqual(0, correctDeps.Count);
            Assert.AreEqual("test", ss.GetCellContents("C1"));
        }

        /// <summary>
        /// Tests setting the cell when the new value is a Formula
        /// 
        /// Tests 2 cases 
        /// 1. the cell has no dependees
        /// 2. The cell has dependees
        /// </summary>
        [TestMethod]
        public void TestSetCellFormula()
        {
            //Case when the the original cell has no dependees

            GenerateNewSS();

            ISet<string> deps = ss.SetContentsOfCell("A1", "=A2+A3");

            HashSet<string> correctDeps = new HashSet<string>();
            correctDeps.Add("A1");
            correctDeps.Add("C1");
            correctDeps.Add("C2");
            correctDeps.Add("E1");
            correctDeps.Add("E2");

            foreach (string s in deps)
            {
                Assert.IsTrue(correctDeps.Contains(s));
                correctDeps.Remove(s);
            }

            Assert.AreEqual(0, correctDeps.Count);
            Assert.AreEqual("A2+A3", ss.GetCellContents("A1").ToString());

            //Case when the the original cell had dependees
            GenerateNewSS();

            deps = ss.SetContentsOfCell("C1", "=A3+A2");

            correctDeps = new HashSet<string>();
            correctDeps.Add("C1");
            correctDeps.Add("E1");
            correctDeps.Add("E2");

            foreach (string s in deps)
            {
                Assert.IsTrue(correctDeps.Contains(s));
                correctDeps.Remove(s);
            }

            Assert.AreEqual(0, correctDeps.Count);
            Assert.AreEqual("A3+A2", ss.GetCellContents("C1").ToString());
        }

        /// <summary>
        /// Tests an invalid formula to see if a formulaerror is stored as the value of the cell
        /// Implicitly tests getcellvalue
        /// </summary>
        [TestMethod]
        public void TestSetContentsOfCell()
        {
            GenerateNewSS();
            ss.SetContentsOfCell("F9", "=b10+c39");

            Assert.IsTrue(ss.GetCellValue("F9") is FormulaError);

        }

        /// <summary>
        /// Tests GetCellValue if name is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellValueNullName()
        {
            GenerateNewSS();
            ss.GetCellValue(null);
        }

        /// <summary>
        /// Tests GetCellValue if name is not valid
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellValueInvalidName()
        {
            GenerateNewSS();
            ss.GetCellValue("test");
        }

    }
}
