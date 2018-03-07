//Nithin Chalapathi - u0847388 - Spring '18 - University of Utah

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using Dependencies;
using Formulas;

namespace SS
{
    /// <summary>
    /// An Spreadsheet object represents the state of a simple spreadsheet.  A 
    /// spreadsheet consists of an infinite number of named cells.
    /// 
    /// A string s is a valid cell name if and only if it consists of one or more letters, 
    /// followed by a non-zero digit, followed by zero or more digits.
    /// 
    /// For example, "A15", "a15", "XY32", and "BC7" are valid cell names.  On the other hand, 
    /// "Z", "X07", and "hello" are not valid cell names.
    /// 
    /// A spreadsheet contains a unique cell corresponding to each possible cell name.  
    /// In addition to a name, each cell has a contents and a Value.  The distinction is
    /// important, and it is important that you understand the distinction and use
    /// the right term when writing code, writing comments, and asking questions.
    /// 
    /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
    /// contents is an empty string, we say that the cell is empty.  (By analogy, the contents
    /// of a cell in Excel is what is displayed on the editing line when the cell is selected.)
    /// 
    /// In an empty spreadsheet, the contents of every cell is the empty string.
    ///  
    /// The Value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
    /// (By analogy, the Value of an Excel cell is what is displayed in that cell's position
    /// in the grid.)
    /// 
    /// If a cell's contents is a string, its Value is that string.
    /// 
    /// If a cell's contents is a double, its Value is that double.
    /// 
    /// If a cell's contents is a Formula, its Value is either a double or a FormulaError.
    /// The Value of a Formula, of course, can depend on the values of variables.  The Value 
    /// of a Formula variable is the Value of the spreadsheet cell it names (if that cell's 
    /// Value is a double) or is undefined (otherwise).  If a Formula depends on an undefined
    /// variable or on a division by zero, its Value is a FormulaError.  Otherwise, its Value
    /// is a double, as specified in Formula.Evaluate.
    /// 
    /// Spreadsheets are never allowed to contain a combination of Formulas that establish
    /// a circular dependency.  A circular dependency exists when a cell depends on itself.
    /// For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
    /// A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
    /// dependency.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        /// <summary>
        /// Dictionary containing the non-empty cells and associated cell structs
        /// </summary>
        private Dictionary<String, Cell> cells;

        /// <summary>
        /// Dependency graph to model all the dependencies in the spreadsheet
        /// </summary>
        private DependencyGraph dg;

        /// <summary>
        /// Private regex expression set in the constructors that determines if the cell name is valid
        /// </summary>
        private Regex IsValid;

        /// <summary>
        /// Zero argument constructor that make an empty spreadsheet object
        /// </summary>
        public Spreadsheet()
        {
            cells = new Dictionary<string, Cell>();
            dg = new DependencyGraph();
            IsValid = new Regex(".*");
            Changed = false;
        }

        /// <summary>
        /// Creates an empty Spreadsheet whose IsValid regular expression is provided as the parameter
        /// </summary>
        public Spreadsheet(Regex isValid) : this()
        {
            this.IsValid = isValid;
            Changed = false;
        }

        /// <summary>
        /// Creates a Spreadsheet that is a duplicate of the spreadsheet saved in source.
        ///
        /// See the AbstractSpreadsheet.Save method and Spreadsheet.xsd for the file format 
        /// specification.  
        ///
        /// If there's a problem reading source, throws an IOException.
        ///
        /// Else if the contents of source are not consistent with the schema in Spreadsheet.xsd, 
        /// throws a SpreadsheetReadException.  
        ///
        /// Else if the IsValid string contained in source is not a valid C# regular expression, throws
        /// a SpreadsheetReadException.  (If the exception is not thrown, this regex is referred to
        /// below as oldIsValid.)
        ///
        /// Else if there is a duplicate cell name in the source, throws a SpreadsheetReadException.
        /// (Two cell names are duplicates if they are identical after being converted to upper case.)
        ///
        /// Else if there is an invalid cell name or an invalid formula in the source, throws a 
        /// SpreadsheetReadException.  (Use oldIsValid in place of IsValid in the definition of 
        /// cell name validity.)
        ///
        /// Else if there is an invalid cell name or an invalid formula in the source, throws a
        /// SpreadsheetVersionException.  (Use newIsValid in place of IsValid in the definition of
        /// cell name validity.)
        ///
        /// Else if there's a formula that causes a circular dependency, throws a SpreadsheetReadException. 
        ///
        /// Else, create a Spreadsheet that is a duplicate of the one encoded in source except that
        /// the new Spreadsheet's IsValid regular expression should be newIsValid.
        /// </summary>
        public Spreadsheet(TextReader source, Regex newIsValid) : this()
        {
            XmlSchemaSet sc = new XmlSchemaSet();
            sc.Add(null, "Spreadsheet.xsd");

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = sc;
            settings.ValidationEventHandler += (object x, ValidationEventArgs y) => { throw new SpreadsheetReadException(y.Message); };

            HashSet<string> cellNames = new HashSet<string>();

            Regex oldIsValid = null;

            using (XmlReader reader = XmlReader.Create(source, settings))
            {
                try
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "spreadsheet":
                                    oldIsValid = new Regex(reader["IsValid"]);
                                    break;

                                case "cell":
                                    string cellName = reader["name"];
                                    this.IsValid = oldIsValid;
                                    if (!ValidCellName(ref cellName) || cellNames.Contains(cellName))
                                    {
                                        throw new SpreadsheetReadException("Bad Cellname or repeat of cellname");
                                    }

                                    if (reader["contents"][0] == '=')
                                    {
                                        Formula f = new Formula(reader["contents"].Substring(1), s => s.ToUpper(), ValidCellName);
                                    }

                                    this.IsValid = newIsValid;
                                    try
                                    {
                                        this.SetContentsOfCell(cellName, reader["contents"]);
                                    }
                                    catch (FormulaFormatException)
                                    {
                                        throw new SpreadsheetVersionException("Formula Format");
                                    }
                                    catch (InvalidNameException)
                                    {
                                        throw new SpreadsheetVersionException("Invalid name");
                                    }
                                    catch (CircularException)
                                    {
                                        throw new SpreadsheetReadException("Cycle Format");
                                    }

                                    cellNames.Add(cellName);
                                    break;
                            }
                        }
                    }
                }
                catch (ArgumentException)
                {
                    throw new SpreadsheetReadException("Bad IsValid");
                }
                catch (FormulaFormatException)
                {
                    throw new SpreadsheetReadException("Bad formula");
                }
                this.IsValid = newIsValid;
            }

            Changed = false;
        }

        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed { get; protected set; }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the Value) of the named cell.  The return
        /// Value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            if (name is null || !ValidCellName(ref name))
            {
                throw new InvalidNameException();
            }

            if (cells.ContainsKey(name))
            {
                return cells[name].Contents;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, returns the Value (as opposed to the contents) of the named cell.  The return
        /// Value should be either a string, a double, or a FormulaError.
        /// </summary>
        public override object GetCellValue(string name)
        {
            if (name is null || !ValidCellName(ref name))
            {
                throw new InvalidNameException();
            }

            if (cells.Keys.Contains(name))
            {
                return cells[name].Value;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            foreach (string key in cells.Keys)
            {
                yield return key;
            }
        }

        /// <summary>
        /// Writes the contents of this spreadsheet to dest using an XML format.
        /// The XML elements should be structured as follows:
        ///
        /// <spreadsheet IsValid="IsValid regex goes here">
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        /// </spreadsheet>
        ///
        /// The Value of the IsValid attribute should be IsValid.ToString()
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.
        /// If the cell contains a string, the string (without surrounding double quotes) should be written as the contents.
        /// If the cell contains a double d, d.ToString() should be written as the contents.
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        ///
        /// If there are any problems writing to dest, the method should throw an IOException.
        /// </summary>
        public override void Save(TextWriter dest)
        {
            using (XmlWriter writer = XmlWriter.Create(dest))
            {
                // Write boilerplate
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("IsValid", IsValid.ToString());

                foreach (string s in GetNamesOfAllNonemptyCells())
                {
                    writer.WriteStartElement("cell");
                    writer.WriteAttributeString("name", s);
                    // Formula case
                    if (cells[s].Contents is Formula)
                    {
                        writer.WriteAttributeString("contents", "=" + cells[s].Contents.ToString());
                    }
                    // Double or string case
                    else
                    {
                        writer.WriteAttributeString("contents", cells[s].Contents.ToString());
                    }

                    writer.WriteFullEndElement();
                }

                writer.WriteFullEndElement();
                writer.WriteEndDocument();

                // Change the status of changed
                Changed = false;
            }
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose Value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, double number)
        {
            if (name is null || !ValidCellName(ref name))
            {
                throw new InvalidNameException();
            }

            if (cells.ContainsKey(name))
            {
                cells.Remove(name);

                HashSet<String> oldDependees = new HashSet<string>();
                foreach (string s in dg.GetDependees(name))
                {
                    oldDependees.Add(s);
                }

                foreach (string s in oldDependees)
                {
                    dg.RemoveDependency(s, name);
                }
            }

            cells.Add(name, new Cell(number, GetCellValue));

            // Finds all the cells that are dependent on the cell that just changed
            HashSet<string> cellsToRecalculate = new HashSet<string>();

            foreach (string recalc in GetCellsToRecalculate(name))
            {
                cellsToRecalculate.Add(recalc);
            }

            Changed = true;
            return cellsToRecalculate;
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose Value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, string text)
        {
            if (name is null || !ValidCellName(ref name))
            {
                throw new InvalidNameException();
            }

            if (text is null)
            {
                throw new ArgumentNullException();
            }

            if (cells.ContainsKey(name))
            {
                cells.Remove(name);

                HashSet<String> oldDependees = new HashSet<string>();
                foreach (string s in dg.GetDependees(name))
                {
                    oldDependees.Add(s);
                }

                foreach (string s in oldDependees)
                {
                    dg.RemoveDependency(s, name);
                }
            }

            cells.Add(name, new Cell(text, GetCellValue));

            HashSet<string> cellsRecalc = new HashSet<string>();

            foreach (string recalc in GetCellsToRecalculate(name))
            {
                cellsRecalc.Add(recalc);
            }

            Changed = true;
            return cellsRecalc;
        }

        /// <summary>
        /// Requires that all of the variables in formula are valid cell names.
        /// 
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose Value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            if (name is null || !ValidCellName(ref name))
            {
                throw new InvalidNameException();
            }

            foreach (string v in formula.GetVariables())
            {
                if (!ValidCellName(v))
                {
                    throw new ArgumentException();
                }

                if (v.Equals(name))
                {
                    throw new CircularException();
                }
            }


            HashSet<string> oldDependees = new HashSet<string>();

            object oldVal = null;

            if (cells.ContainsKey(name))
            {
                oldVal = cells[name].Contents;

                foreach (string s in dg.GetDependees(name))
                {
                    oldDependees.Add(s);
                }

                foreach (string s in oldDependees)
                {
                    dg.RemoveDependency(s, name);
                }

                cells.Remove(name);
            }
            cells.Add(name, new Cell(formula, GetCellValue));

            foreach (string dep in formula.GetVariables())
            {
                dg.AddDependency(dep, name);
            }

            try
            {
                HashSet<string> recalc = new HashSet<string>();

                foreach (string calc in GetCellsToRecalculate(name))
                {
                    recalc.Add(calc);
                }

                Changed = true;

                return recalc;
            }
            catch (CircularException e)
            {
                cells.Remove(name);

                foreach (string dep in formula.GetVariables())
                {
                    dg.RemoveDependency(dep, name);
                }

                if (oldVal != null)
                {
                    foreach (string dep in oldDependees)
                    {
                        dg.AddDependency(dep, name);
                    }

                    cells.Add(name, new Cell(oldVal, GetCellValue));
                }

                throw e;
            }
        }

        /// <summary>
        /// If Contents is null, throws an ArgumentNullException.
        ///
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, if Contents parses as a double, the contents of the named
        /// cell becomes that double.
        ///
        /// Otherwise, if Contents begins with the character '=', an attempt is made
        /// to parse the remainder of Contents into a Formula f using the Formula
        /// constructor with s => s.ToUpper() as the normalizer and a validator that
        /// checks that s is a valid cell name as defined in the AbstractSpreadsheet
        /// class comment.  There are then three possibilities:
        ///
        ///   (1) If the remainder of Contents cannot be parsed into a Formula, a
        ///       Formulas.FormulaFormatException is thrown.
        ///
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///
        ///   (3) Otherwise, the contents of the named cell becomes f.
        ///
        /// Otherwise, the contents of the named cell becomes Contents.
        ///
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose Value depends, directly
        /// or indirectly, on the named cell.
        ///
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            if (content is null)
            {
                throw new ArgumentNullException();
            }
            else if (name is null || !ValidCellName(ref name))
            {
                throw new InvalidNameException();
            }

            if(content.Length == 0)
            {
                this.cells.Remove(name);
                dg.ReplaceDependees(name, new HashSet<string> { });
                dg.ReplaceDependents(name, new HashSet<string> { });
                return new HashSet<string> { };
            }

            ISet<string> recalc;

            // Double case
            if (double.TryParse(content, out double d))
            {
                recalc = this.SetCellContents(name, d);
            }
            // Formula case
            else if (content[0] == '=')
            {
                Formula f = new Formula(content.Substring(1), s => s.ToUpper(), ValidCellName);
                recalc = this.SetCellContents(name, f);
            }
            // String case
            else
            {
                recalc = this.SetCellContents(name, content);
            }

            foreach(string recalculate in this.GetCellsToRecalculate(name))
            {
                this.cells[recalculate].ReCalculateValue();
            }

            return recalc;
        }

        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the Value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException();
            }

            if (!ValidCellName(ref name))
            {
                throw new InvalidNameException();
            }

            HashSet<string> deps = new HashSet<string>();

            foreach (string dep in dg.GetDependents(name))
            {
                deps.Add(dep);
            }

            foreach (string s in deps)
            {
                yield return s;
            }
        }

        /// <summary>
        /// Helper method to check if a name is valid. This version does  convert the name to upper case
        /// 
        /// </summary>
        private bool ValidCellName(ref string name)
        {
            name = name.ToUpper();

            // Creates a regex that makes sure there is at least 
            // one or more letter followed by a nonzero digit followed by 0 or more digits
            Regex reg = new Regex(@"^[A-z]+[1-9]{1}[\d]*$");
            return reg.IsMatch(name) && IsValid.IsMatch(name);
        }

        /// <summary>
        /// Helper method to check if a name is valid. This version does not convert the name to upper case
        /// </summary>
        private bool ValidCellName(string name)
        {

            // Creates a regex that makes sure there is at least 
            // one or more letter followed by a nonzero digit followed by 0 or more digits
            Regex reg = new Regex(@"^[A-z]+[1-9]{1}[\d]*$");
            return reg.IsMatch(name) && IsValid.IsMatch(name.ToUpper());
        }
    }

    /// <summary>
    /// Helper struct that keeps track of the properties of cells
    /// </summary>
    class Cell
    {
        /// <summary>
        /// Contents of the cell 
        /// </summary>
        public object Contents { get; }

        /// <summary>
        /// The actual Value of the cell
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// Stores the passed delegate for future use
        /// </summary>
        private LookupCellValue lookup;

        /// <summary>
        /// Delegate to lookup a cell in the spreadsheet
        /// </summary>
        public delegate object LookupCellValue(string name);

        /// <summary>
        /// Constructor that takes in a single param, the contents of the cell
        /// </summary>
        public Cell(object contents, LookupCellValue lookup)
        {
            this.Contents = contents;
            Value = null;
            this.lookup = lookup;
            ReCalculateValue();
        }

        /// <summary>
        /// Recalculates the Value for the cell
        /// Requires contents to be non-null
        /// </summary>
        public void ReCalculateValue()
        {
            if (Contents is string)
            {
                Value = Contents;
            }
            else if (Contents is double)
            {
                Value = Contents;
            }
            else if (Contents is Formula)
            {
                try
                {
                    LookupCellValue lookup = this.lookup;
                    var result = ((Formula)Contents).Evaluate(delegate (string x)
                   {
                       object val = lookup(x);
                       if (val is double)
                       {
                           return (double)val;
                       }
                       else
                       {
                           throw new UndefinedVariableException(x);
                       }
                   });

                    Value = result;
                }
                catch (FormulaEvaluationException e)
                {
                    Value = new FormulaError(e.Message);
                }
            }
        }
    }
}
