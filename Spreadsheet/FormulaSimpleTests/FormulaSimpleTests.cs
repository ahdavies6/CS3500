// Written by Joe Zachary for CS 3500, January 2017.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formulas;
using System.Collections;

namespace FormulaTestCases
{
    /// <summary>
    /// These test cases are in no sense comprehensive!  They are intended to show you how
    /// client code can make use of the Formula class, and to show you how to create your
    /// own (which we strongly recommend).  To run them, pull down the Test menu and do
    /// Run > All Tests.
    /// </summary>
    [TestClass]
    public class UnitTests
    {
        //////////////////////////////////BEGIN CONSTRUCTOR EXCEPTION TESTING //////////////////////////

        /// <summary>
        /// This tests that a syntactically incorrect parameter to Formula results
        /// in a FormulaFormatException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct1()
        {
            Formula f = new Formula("_");
        }

        /// <summary>
        /// This is another syntax error
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct2()
        {
            Formula f = new Formula("2++3");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct3()
        {
            Formula f = new Formula("2 3");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void SpacedStringFormulaTest()
        {
            Formula f = new Formula("  ");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void EmptyStringFormulaTest()
        {
            Formula f = new Formula(" ");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FirstTokenInvalid()
        {
            Formula f = new Formula("_x+y");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void LastTokenInvalid()
        {
            Formula f = new Formula("x+y\\");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void MoreLeftParenThanRightTest()
        {
            Formula f = new Formula("((x+y)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void MoreRightParenThanLeftTest()
        {
            Formula f = new Formula("(x+y)))");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void VariableFollowedByVariableTest()
        {
            Formula f = new Formula("x86 x64");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void InValidExampleFromDocComment1()
        {
            Formula f = new Formula("_");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void InValidExampleFromDocComment2()
        {
            Formula f = new Formula("-.5.3");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void InValidExampleFromDocComment3()
        {
            Formula f = new Formula("2 5 +3");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FirstTokenCloseParenTest()
        {
            Formula f = new Formula(")1+3");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void InvalidEndingTokenOpenParenTest()
        {
            Formula f = new Formula("x+y(");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ClosingParenFollowedByOpenParenTest()
        {
            Formula f = new Formula("(x+y)(x+z)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void OpenParenFollowedByClosingParen()
        {
            Formula f = new Formula("()");
        }

        ///////////////////////////////////BEGIN EVALUTE EXCEPTION//////////////////////////

        /// <summary>
        /// Helper method that defines certain variables for testing.
        /// </summary>
        public double lookupHelper(string x)
        {
            switch (x)
            {
                case "x": return 1;
                case "y": return 2;
                case "z": return 5;
                case "a": return 0;
                case "b": return .5;
                default: throw new UndefinedVariableException(x);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void DivideByZeroErrorTest()
        {
            Formula f = new Formula("z / a");
            f.Evaluate(lookupHelper);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void LookUpFailedException()
        {
            Formula f = new Formula("a+j");
            f.Evaluate(lookupHelper);
        }

        /////////////////////////////// NORMAL EVALUTE TESTS ////////////////////////////////
        [TestMethod]
        public void AdditionOpsTest()
        {
            Formula f = new Formula("a+b+x+y+z");
            Assert.AreEqual(f.Evaluate(lookupHelper), 8.5, 1e-6);
            f = new Formula("(a+b)+(x+y)");
            Assert.AreEqual(f.Evaluate(lookupHelper), 3.5, 1e-6);
        }

        [TestMethod]
        public void SubtractionOpsTest()
        {
            Formula f = new Formula("0-a-b-x-y-z");
            Assert.AreEqual(f.Evaluate(lookupHelper), -8.5, 1e-6);
            f = new Formula("(0-a-b)+(0-x-y)");
            Assert.AreEqual(f.Evaluate(lookupHelper), -3.5, 1e-6);
        }

        [TestMethod]
        public void MultiplicationOpsTest()
        {
            Formula f = new Formula("a*b*x*y*z");
            Assert.AreEqual(f.Evaluate(lookupHelper), 0, 1e-6);
            f = new Formula("(a*b)+(x*y)");
            Assert.AreEqual(f.Evaluate(lookupHelper), 2, 1e-6);
        }

        [TestMethod]
        public void DivideOpsTest()
        {
            Formula f = new Formula("x/y");
            Assert.AreEqual(f.Evaluate(lookupHelper), .5, 1e-6);
            f = new Formula("z/y");
            Assert.AreEqual(f.Evaluate(lookupHelper), 2.5, 1e-6);
        }

        /////////////////////////////////////JOE'S EVALUATE TESTS////////////////////////////


        /// <summary>
        /// Makes sure that "2+3" evaluates to 5.  Since the Formula
        /// contains no variables, the delegate passed in as the
        /// parameter doesn't matter.  We are passing in one that
        /// maps all variables to zero.
        /// </summary>
        [TestMethod]
        public void Evaluate1()
        {
            Formula f = new Formula("2+3");
            Assert.AreEqual(f.Evaluate(v => 0), 5.0, 1e-6);
        }

        /// <summary>
        /// The Formula consists of a single variable (x5).  The value of
        /// the Formula depends on the value of x5, which is determined by
        /// the delegate passed to Evaluate.  Since this delegate maps all
        /// variables to 22.5, the return value should be 22.5.
        /// </summary>
        [TestMethod]
        public void Evaluate2()
        {
            Formula f = new Formula("x5");
            Assert.AreEqual(f.Evaluate(v => 22.5), 22.5, 1e-6);
        }

        /// <summary>
        /// Here, the delegate passed to Evaluate always throws a
        /// UndefinedVariableException (meaning that no variables have
        /// values).  The test case checks that the result of
        /// evaluating the Formula is a FormulaEvaluationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate3()
        {
            Formula f = new Formula("x + y");
            f.Evaluate(v => { throw new UndefinedVariableException(v); });
        }

        /// <summary>
        /// The delegate passed to Evaluate is defined below.  We check
        /// that evaluating the formula returns in 10.
        /// </summary>
        [TestMethod]
        public void Evaluate4()
        {
            Formula f = new Formula("x + y");
            Assert.AreEqual(f.Evaluate(Lookup4), 10.0, 1e-6);
        }

        /// <summary>
        /// This uses one of each kind of token.
        /// </summary>
        [TestMethod]
        public void Evaluate5()
        {
            Formula f = new Formula("(x + y) * (z / x) * 1.0");
            Assert.AreEqual(f.Evaluate(Lookup4), 20.0, 1e-6);
        }

        /// <summary>
        /// A Lookup method that maps x to 4.0, y to 6.0, and z to 8.0.
        /// All other variables result in an UndefinedVariableException.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double Lookup4(String v)
        {
            switch (v)
            {
                case "x": return 4.0;
                case "y": return 6.0;
                case "z": return 8.0;
                default: throw new UndefinedVariableException(v);
            }
        }

        ///////////////////////////////////////////PS4 Tests and Struct tests /////////////////////

        /// <summary>
        /// Tests that the zero arg constructor behaves as Formula("0");
        /// </summary>
        [TestMethod]
        public void TestDefaultStructConstructor()
        {
            Formula f = new Formula();
            Assert.AreEqual(0.0, f.Evaluate(x => 1));
            Assert.AreEqual(0.0, f.Evaluate(Lookup4));
        }

        /// <summary>
        /// Intentionally passes a normalizer that makes an invalid var
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestNormalizerError()
        {
            Formula f = new Formula("x+y", x => "--", x => true);
        }

        /// <summary>
        /// Intentionally passes a validator that always fails
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestValidatorError()
        {
            Formula f = new Formula("x+y", x => x, x => false);
        }

        /// <summary>
        /// Passes a valid normalizer and a validator that fails the format of the normalizer
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestValidNormalizerAndInvalidValidator()
        {
            Formula f = new Formula("x+y", x => x + "1", x => x.Length == 1);
        }

        /// <summary>
        /// Creates a valid formula object with a normalizer that makes every varaible upper case
        /// and a validator that requires uppercase characters
        /// 
        /// Making sure no exception is thrown
        /// </summary>
        [TestMethod]
        public void TestValidNoralizerAndValidator()
        {
            Formula f = new Formula("x+y", x => x.ToUpper(), x => char.IsUpper(x.ToCharArray()[0]));
        }


        /// <summary>
        /// Using the 3 arg constructor, creates a formula and tests various lookup delegates
        /// </summary>
        [TestMethod]
        public void Test3ArgConstructor()
        {
            Formula f = new Formula("x+y", x => x.ToUpper(), x => char.IsUpper(x.ToCharArray()[0]));
            Assert.AreEqual(2, f.Evaluate(x => 1));
            Assert.AreEqual(5, f.Evaluate(Lookup3Arg));
        }

        /// <summary>
        /// Helper to test the 3 arg constructor
        /// </summary>
        /// <param name="var"></param>
        /// <returns></returns>
        public double Lookup3Arg(String s)
        {
            if (s.Equals("X"))
            {
                return 2;
            }
            else if (s.Equals("Y"))
            {
                return 3;
            }
            else
            {
                throw new UndefinedVariableException(s);
            }
        }

        /// <summary>
        /// Tests the GetVariables method with both the 1 and 3 arg constructor
        /// </summary>
        [TestMethod]
        public void TestGetVariables()
        {
            Formula f = new Formula("x+y+z");
            ArrayList vars = new ArrayList();
            vars.Add("x");
            vars.Add("y");
            vars.Add("z");
            foreach (string s in f.GetVariables())
            {
                Assert.IsTrue(vars.Contains(s));
            }

            Assert.AreEqual(3, f.GetVariables().Count);

            f = new Formula("X+Y+Z");
            vars = new ArrayList();
            vars.Add("X");
            vars.Add("Y");
            vars.Add("Z");
            foreach (string s in f.GetVariables())
            {
                Assert.IsTrue(vars.Contains(s));
            }

            Assert.AreEqual(3, f.GetVariables().Count);
        }

        /// <summary>
        /// Tests the toString method of Formula
        /// </summary>
        [TestMethod]
        public void TestToString()
        {
            Formula f = new Formula("x+y", x => x.ToUpper(), x => true);
            Formula f2 = new Formula(f.ToString(), x => x.ToUpper(), x => true);
            Assert.AreEqual(f.Evaluate(x => 1), f2.Evaluate(x => 1));
            Assert.AreEqual(f.Evaluate(Lookup3Arg), f2.Evaluate(Lookup3Arg));
        }

        ////////////////////////////////Null tests /////////////////////////

        /// <summary>
        /// Test the null case in a one arg constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullSingleArgConstructor()
        {
            Formula f = new Formula(null);
        }

        /// <summary>
        /// Test the null case in the 3 arg constructor: formula string
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullFormulaParam()
        {
            Formula f = new Formula(null, x => x, x => true);
        }

        /// <summary>
        /// Test the null case in the 3 arg constructor: normalizer method
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullNormalizerParam()
        {
            Formula f = new Formula("x+y", null, x => true);
        }

        /// <summary>
        /// Test the null case in the 3 arg constructor: Validator method
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullValidatorParam()
        {
            Formula f = new Formula("x+y", x => x, null);
        }

        /// <summary>
        /// Test the null case when lookup is null in evalute
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullLookupParam()
        {
            Formula f = new Formula("x+y", x => x, x => true);
            f.Evaluate(null);
        }

        ///////////////////////////////////Begin grading tests////////////////////////////////

        // Tests of syntax errors detected by the constructor
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test1()
        {
            Formula f = new Formula("        ");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test2()
        {
            Formula f = new Formula("((2 + 5))) + 8");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test3()
        {
            Formula f = new Formula("2+5*8)");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test4()
        {
            Formula f = new Formula("((3+5*7)");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test5()
        {
            Formula f = new Formula("+3");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test6()
        {
            Formula f = new Formula("-y");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test7()
        {
            Formula f = new Formula("*7");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test8()
        {
            Formula f = new Formula("/z2x");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test9()
        {
            Formula f = new Formula(")");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test10()
        {
            Formula f = new Formula("(*5)");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test11()
        {
            Formula f = new Formula("2 5");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test12()
        {
            Formula f = new Formula("x5 y");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test13()
        {
            Formula f = new Formula("((((((((((2)))))))))");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test14()
        {
            Formula f = new Formula("$");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test15()
        {
            Formula f = new Formula("x5 + x6 + x7 + (x8) +");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test15a()
        {
            Formula f = new Formula("x1 ++ y1");
        }

        // Simple tests that throw FormulaEvaluationExceptions
        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test16()
        {
            Formula f = new Formula("2+x");
            f.Evaluate(s => { throw new UndefinedVariableException(s); });
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test17()
        {
            Formula f = new Formula("5/0");
            f.Evaluate(s => 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test18()
        {
            Formula f = new Formula("(5 + x) / (y - 3)");
            f.Evaluate(s => 3);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test18a()
        {
            Formula f = new Formula("(5 + x) / (3 * 2 - 12 / 2)");
            f.Evaluate(s => 3);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test19()
        {
            Formula f = new Formula("x + y");
            f.Evaluate(s => { if (s == "x") return 0; else throw new UndefinedVariableException(s); });
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test20()
        {
            Formula f = new Formula("x1 + x2 * x3 + x4 * x5 * x6 + x7");
            f.Evaluate(s => { if (s == "x7") throw new UndefinedVariableException(s); else return 1; });
        }

        // Simple formulas
        [TestMethod()]
        public void Test21()
        {
            Formula f = new Formula("4.5e1");
            Assert.AreEqual(45, f.Evaluate(s => 0), 1e-6);
        }

        [TestMethod()]
        public void Test21a()
        {
            Formula f = new Formula("4");
            Assert.AreEqual(4, f.Evaluate(s => 0), 1e-6);
        }

        [TestMethod()]
        public void Test22()
        {
            Formula f = new Formula("a05");
            Assert.AreEqual(10, f.Evaluate(s => 10), 1e-6);
        }

        [TestMethod()]
        public void Test22a()
        {
            Formula f = new Formula("a1b2c3d4e5f6g7h8i9j10");
            Assert.AreEqual(10, f.Evaluate(s => 10), 1e-6);
        }

        [TestMethod()]
        public void Test23()
        {
            Formula f = new Formula("5 + x");
            Assert.AreEqual(9, f.Evaluate(s => 4), 1e-6);
        }

        [TestMethod()]
        public void Test24()
        {
            Formula f = new Formula("5 - y");
            Assert.AreEqual(1, f.Evaluate(s => 4), 1e-6);
        }

        [TestMethod()]
        public void Test25()
        {
            Formula f = new Formula("5 * z");
            Assert.AreEqual(20, f.Evaluate(s => 4), 1e-6);
        }

        [TestMethod()]
        public void Test26()
        {
            Formula f = new Formula("8 / xx");
            Assert.AreEqual(2, f.Evaluate(s => 4), 1e-6);
        }

        [TestMethod()]
        public void Test27()
        {
            Formula f = new Formula("(5 + 4) * 2");
            Assert.AreEqual(18, f.Evaluate(s => 0), 1e-6);
        }

        [TestMethod()]
        public void Test28()
        {
            Formula f = new Formula("1 + 2 + 3 * 4 + 5");
            Assert.AreEqual(20, f.Evaluate(s => 0), 1e-6);
        }

        [TestMethod()]
        public void Test29()
        {
            Formula f = new Formula("(1 + 2 + 3 * 4 + 5) * 2");
            Assert.AreEqual(40, f.Evaluate(s => 0), 1e-6);
        }

        [TestMethod()]
        public void Test30()
        {
            Formula f = new Formula("((((((((((((3))))))))))))");
            Assert.AreEqual(3, f.Evaluate(s => 0), 1e-6);
        }

        [TestMethod()]
        public void Test31()
        {
            Formula f = new Formula("((((((((((((x))))))))))))");
            Assert.AreEqual(7, f.Evaluate(s => 7), 1e-6);
        }

        // Some more complicated formula evaluations
        [TestMethod()]
        public void Test32()
        {
            Formula f = new Formula("y*3-8/2+4*(8-9*2)/14*x");
            Assert.AreEqual(5.14285714285714, (double)f.Evaluate(s => (s == "x") ? 1 : 4), 1e-9);
        }

        [TestMethod()]
        public void Test32a()
        {
            Formula f = new Formula("a + b * c - d + 3 * 3.0 - 3.0e0 / 0.003e3");
            Assert.AreEqual(17, (double)f.Evaluate(s => 3), 1e-9);
        }

        [TestMethod()]
        public void Test33()
        {
            Formula f = new Formula("a+(b+(c+(d+(e+f))))");
            Assert.AreEqual(6, (double)f.Evaluate(s => 1), 1e-9);
        }

        [TestMethod()]
        public void Test34()
        {
            Formula f = new Formula("((((x1+x2)+x3)+x4)+x5)+x6");
            Assert.AreEqual(12, (double)f.Evaluate(s => 2), 1e-9);
        }

        [TestMethod()]
        public void Test35()
        {
            Formula f = new Formula("a-a*a/a");
            Assert.AreEqual(0, (double)f.Evaluate(s => 3), 1e-9);
        }

        // Tests to make sure there can be more than one formula at a time
        [TestMethod()]
        public void Test36()
        {
            Formula f1 = new Formula("xx+3");
            Formula f2 = new Formula("xx-3");
            Assert.AreEqual(6, f1.Evaluate(s => 3), 1e-6);
            Assert.AreEqual(0, f2.Evaluate(s => 3), 1e-6);
        }

        [TestMethod()]
        public void Test37()
        {
            Test36();
        }

        [TestMethod()]
        public void Test38()
        {
            Test36();
        }

        [TestMethod()]
        public void Test39()
        {
            Test36();
        }

        [TestMethod()]
        public void Test40()
        {
            Test36();
        }

        // Stress test for constructor
        [TestMethod()]
        public void Test41()
        {
            Formula f = new Formula("(((((2+3*a)/(7e-5+b-c))*d+.0005e+92)-8.2)*3.14159) * ((e+3.1)-.00000000008)");
        }

        // Stress test for constructor, repeated five times to give it extra weight.
        [TestMethod()]
        public void Test42()
        {
            Test41();
        }

        // Stress test for constructor
        [TestMethod()]
        public void Test43()
        {
            Test41();
        }

        // Stress test for constructor
        [TestMethod()]
        public void Test44()
        {
            Test41();
        }

        // Stress test for constructor
        [TestMethod()]
        public void Test45()
        {
            Test41();
        }
        ////////////////////////////////////////////////////////////////////////////////

    }

}
