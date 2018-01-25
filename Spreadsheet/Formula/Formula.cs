// Skeleton written by Joe Zachary for CS 3500, January 2017
// Remainder implementation wrriten by Nithin Chalapathi - u0847388 - Spring 18

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Formulas
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  Provides a means to evaluate Formulas.  Formulas can be composed of
    /// non-negative floating-point numbers, variables, left and right parentheses, and
    /// the four binary operator symbols +, -, *, and /.  (The unary operators + and -
    /// are not allowed.)
    /// </summary>
    public class Formula
    {

        /// <summary>
        /// 
        /// </summary>
        private LinkedList<string> formulaTokens;

        /// <summary>
        /// Creates a Formula from a string that consists of a standard infix expression composed
        /// from non-negative floating-point numbers (using C#-like syntax for double/int literals), 
        /// variable symbols (a letter followed by zero or more letters and/or digits), left and right
        /// parentheses, and the four binary operator symbols +, -, *, and /.  White space is
        /// permitted between tokens, but is not required.
        /// 
        /// Examples of a valid parameter to this constructor are:
        ///     "2.5e9 + x5 / 17"
        ///     "(5 * 2) + 8"
        ///     "x*y-2+35/9"
        ///     
        /// Examples of invalid parameters are:
        ///     "_"
        ///     "-5.3"
        ///     "2 5 + 3"
        /// 
        /// If the formula is syntacticaly invalid, throws a FormulaFormatException with an 
        /// explanatory Message.
        /// </summary>
        public Formula(String formula)
        {
            //Generate the linked list
            formulaTokens = new LinkedList<string>();

            //Go through the list of tokens 
            //We don't use a foreach loop in order to manually control enumeration
            var tokens = GetTokens(formula).GetEnumerator();

            //left paren
            int numLeftParen = 0;

            //right paren
            int numRightParen = 0;

            //Formula token types 
            string leftParen = "(";
            string rightParen = ")";

            //valid operaters
            string operators = "+-/*";

            //Add all valid tokens to the linkedlist
            foreach (string token in GetTokens(formula))
            {
                if (double.TryParse(token, out double dummy) || IsVar(token) || token.Equals(leftParen) || token.Equals(rightParen) || operators.Contains(token))
                {
                    formulaTokens.AddLast(token);
                }
                else
                {
                    throw new FormulaFormatException("Invalid Token was passed: " + token);
                }
            }

            for (var currNode = formulaTokens.First; currNode != null && currNode.Next != null; currNode = currNode.Next)
            {

                if (numRightParen > numLeftParen)
                {
                    throw new FormulaFormatException("The number of closing parentheses is greater than the number of opening parentheses at some point.");
                }

                //Var case and number and closing paren case  where the next token is not an operator or right paren
                if ((IsVar(currNode.Value) || double.TryParse(currNode.Value, out double n) || currNode.Value.Equals(rightParen))
                    && !(operators.Contains(currNode.Next.Value) || currNode.Next.Value.Equals(rightParen)))
                {
                    throw new FormulaFormatException("One of the tokens following a variable, double, or closing parenthsis, was not an operator or closing parenthesis.");
                }

                //Open parenthesis or operator case where the next token is not a number, variable, or an opening paren
                else if ((operators.Contains(currNode.Value) || currNode.Value.Equals(leftParen))
                    && !(double.TryParse(currNode.Next.Value, out double number) || IsVar(currNode.Next.Value) || currNode.Next.Value.Equals(leftParen)))
                {
                    throw new FormulaFormatException("One of the tokens following an open parenthesis or an operator was not a number, variable, or opening parenthesis");
                }

                //Counting the parethesis
                if (currNode.Value.Equals(leftParen))
                {
                    numLeftParen++;
                }
                else if (currNode.Value.Equals(rightParen))
                {
                    numRightParen++;
                }

            }

            //first token case must be a number, variable, or opening parenthesis
            if (!(double.TryParse(formulaTokens.First.Value, out double num) || IsVar(formulaTokens.First.Value) || formulaTokens.First.Value.Equals(leftParen)))
            {
                throw new FormulaFormatException("First token was not a number, variable, or opening parenthesis");
            }


            if (formulaTokens.Count == 0)
            {
                throw new FormulaFormatException("No valid tokens were passed.");
            }

            //Last token must be a number or variable or closing parenthesis
            if (!(double.TryParse(formulaTokens.Last.Value, out double d) || IsVar(formulaTokens.Last.Value) || formulaTokens.Last.Value.Equals(rightParen)))
            {
                throw new FormulaFormatException("The last token is not a number, variable, or closing parenthesis.");
            }

            if (numLeftParen != numRightParen)
            {
                throw new FormulaFormatException("The number of left parentheses does not equal the number of right parentheses.");
            }
        }

        /// <summary>
        /// Private helper to determine if the passed token is a variable (a letter followed by zero or more letters and/or digits).
        /// </summary>
        private bool IsVar(string token)
        {
            char[] variable = token.ToCharArray();

            if (variable.Length == 0 || !char.IsLetter(variable[0])) return false;

            foreach (char c in variable)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    return false;
                }
            }

            //All the checks are done for this to be a proper variable
            return true;
        }

        /// <summary>
        /// Evaluates this Formula, using the Lookup delegate to determine the values of variables.  (The
        /// delegate takes a variable name as a parameter and returns its value (if it has one) or throws
        /// an UndefinedVariableException (otherwise).  Uses the standard precedence rules when doing the evaluation.
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, its value is returned.  Otherwise, throws a FormulaEvaluationException  
        /// with an explanatory Message.
        /// </summary>
        public double Evaluate(Lookup lookup)
        {
            //Create the two stacks
            Stack<double> values = new Stack<double>();
            Stack<string> ops = new Stack<string>();

            //operaters and parens
            string times = "*";
            string divide = "/";
            string add = "+";
            string minus = "-";
            string leftParen = "(";
            string rightParen = ")";

            //Iterate through all tokens
            foreach (string token in formulaTokens)
            {
                //Double case
                double value;
                if (double.TryParse(token, out value))
                {
                    if (ops.Count > 0 && ops.Peek().Equals(times))
                    {
                        double otherVal = values.Pop();
                        ops.Pop();
                        values.Push(value * otherVal);
                    }
                    else if (ops.Count > 0 && ops.Peek().Equals(divide))
                    {
                        double otherVal = values.Pop();
                        ops.Pop();
                        try
                        {
                            values.Push(otherVal / value);
                        }
                        catch (DivideByZeroException e)
                        {
                            throw new FormulaEvaluationException("Division by zero is not possible.");
                        }
                    }
                    else
                    {
                        values.Push(value);
                    }
                }

                //variable case
                if (IsVar(token))
                {
                    try
                    {
                        value = lookup(token);
                    }
                    catch (UndefinedVariableException e)
                    {
                        throw new FormulaEvaluationException("One of the variables doesn't have a lookup value.");
                    }

                    //Same procedure as a number
                    if (ops.Count > 0 && ops.Peek().Equals(times))
                    {
                        double otherVal = values.Pop();
                        ops.Pop();
                        values.Push(value * otherVal);
                    }
                    else if (ops.Count > 0 && ops.Peek().Equals(divide))
                    {
                        double otherVal = values.Pop();
                        ops.Pop();
                        try
                        {
                            values.Push(otherVal / value);
                        }
                        catch (DivideByZeroException e)
                        {
                            throw new FormulaEvaluationException("Division by zero is not possible.");
                        }
                    }
                    else
                    {
                        values.Push(value);
                    }

                }

                //+ and - case 
                if (token.Equals(add) || token.Equals(minus))
                {
                    if (ops.Count > 0 && ops.Peek().Equals(add))
                    {
                        ops.Pop();
                        double val1 = values.Pop();
                        double val2 = values.Pop();
                        values.Push(val1 + val2);
                    }
                    else if (ops.Count > 0 && ops.Peek().Equals(minus))
                    {
                        ops.Pop();
                        double val1 = values.Pop();
                        double val2 = values.Pop();
                        values.Push(val2 - val1);
                    }

                    ops.Push(token);
                }

                // * or / case
                if (token.Equals(divide) || token.Equals(times))
                {
                    ops.Push(token);
                }

                // ( case 
                if (token.Equals(leftParen))
                {
                    ops.Push(token);
                }

                // ) case
                if (token.Equals(rightParen))
                {

                    if (ops.Count > 0 && ops.Peek().Equals(add))
                    {
                        ops.Pop();
                        double val1 = values.Pop();
                        double val2 = values.Pop();
                        values.Push(val1 + val2);
                    }
                    else if (ops.Count > 0 && ops.Peek().Equals(minus))
                    {
                        ops.Pop();
                        double val1 = values.Pop();
                        double val2 = values.Pop();
                        values.Push(val2 - val1);
                    }

                    //Pops the top (
                    ops.Pop();

                    if (ops.Count > 0 && ops.Peek().Equals(times))
                    {
                        ops.Pop();
                        double val1 = values.Pop();
                        double val2 = values.Pop();
                        values.Push(val1 * val2);
                    }
                    else if (ops.Count > 0 && ops.Peek().Equals(divide))
                    {
                        ops.Pop();
                        double val1 = values.Pop();
                        double val2 = values.Pop();
                        values.Push(val2 / val1);
                    }

                }


            }

            //After all tokens are processed, the final actions
            if (ops.Count == 0)
            {
                return values.Pop();
            }
            else
            {
                if (ops.Peek().Equals(add))
                {
                    ops.Pop();
                    double val1 = values.Pop();
                    double val2 = values.Pop();
                    values.Push(val1 + val2);
                }
                else if (ops.Peek().Equals(minus))
                {
                    ops.Pop();
                    double val1 = values.Pop();
                    double val2 = values.Pop();
                    values.Push(val2 - val1);
                }

                return values.Pop();
            }

        }

        /// <summary>
        /// Given a formula, enumerates the tokens that compose it.  Tokens are left paren,
        /// right paren, one of the four operator symbols, a string consisting of a letter followed by
        /// zero or more digits and/or letters, a double literal, and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens.
            // NOTE:  These patterns are designed to be used to create a pattern to split a string into tokens.
            // For example, the opPattern will match any string that contains an operator symbol, such as
            // "abc+def".  If you want to use one of these patterns to match an entire string (e.g., make it so
            // the opPattern will match "+" but not "abc+def", you need to add ^ to the beginning of the pattern
            // and $ to the end (e.g., opPattern would need to be @"^[\+\-*/]$".)
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z][0-9a-zA-Z]*";

            // PLEASE NOTE:  I have added white space to this regex to make it more readable.
            // When the regex is used, it is necessary to include a parameter that says
            // embedded white space should be ignored.  See below for an example of this.
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern.  It contains embedded white space that must be ignored when
            // it is used.  See below for an example of this.  This pattern is useful for 
            // splitting a string into tokens.
            String splittingPattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            // PLEASE NOTE:  Notice the second parameter to Split, which says to ignore embedded white space
            /// in the pattern.
            foreach (String s in Regex.Split(formula, splittingPattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }
    }

    /// <summary>
    /// A Lookup method is one that maps some strings to double values.  Given a string,
    /// such a function can either return a double (meaning that the string maps to the
    /// double) or throw an UndefinedVariableException (meaning that the string is unmapped 
    /// to a value. Exactly how a Lookup method decides which strings map to doubles and which
    /// don't is up to the implementation of the method.
    /// </summary>
    public delegate double Lookup(string var);

    /// <summary>
    /// Used to report that a Lookup delegate is unable to determine the value
    /// of a variable.
    /// </summary>
    [Serializable]
    public class UndefinedVariableException : Exception
    {
        /// <summary>
        /// Constructs an UndefinedVariableException containing whose message is the
        /// undefined variable.
        /// </summary>
        /// <param name="variable"></param>
        public UndefinedVariableException(String variable)
            : base(variable)
        {
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the parameter to the Formula constructor.
    /// </summary>
    [Serializable]
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message) : base(message)
        {
        }
    }

    /// <summary>
    /// Used to report errors that occur when evaluating a Formula.
    /// </summary>
    [Serializable]
    public class FormulaEvaluationException : Exception
    {
        /// <summary>
        /// Constructs a FormulaEvaluationException containing the explanatory message.
        /// </summary>
        public FormulaEvaluationException(String message) : base(message)
        {
        }
    }
}
