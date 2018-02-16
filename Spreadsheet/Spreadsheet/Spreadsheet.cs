using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dependencies;
using Formulas;

namespace SS
{
    class Spreadsheet : AbstractSpreadsheet
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
        /// Zero argument constructor that make an empty spreadsheet object
        /// </summary>
        public Spreadsheet()
        {
            cells = new Dictionary<string, Cell>();
            dg = new DependencyGraph();
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            //Null case or if the name is invalid
            if (name is null || !ValidCellName(name))
            {
                throw new InvalidNameException();
            }

            //Checking to see if the name is in the nonempty list
            if (cells.ContainsKey(name))
            {
                return cells[name].content;
            }
            //If there is not a cell that is full, just return an empty string
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
            //If it is a key, then it is nonempty
            foreach (string key in cells.Keys)
            {
                yield return key;
            }
        }


        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, double number)
        {
            //Checking if name is null or invalid
            if (name is null || !ValidCellName(name))
            {
                throw new InvalidNameException();
            }

            //Checking if the cells dictionary already contains the key of name
            //If so we remove it to make a new entry
            if (cells.ContainsKey(name))
            {
                cells.Remove(name);

                //Deleting all the old dependees since it is just a double now
                //Doubles will never have an dependees
                foreach (string s in dg.GetDependees(name))
                {
                    dg.RemoveDependency(s, name);
                }
            }

            //Add the new cell value
            cells.Add(name, new Cell(number));

            //Finds all the cells that are dependent on the cell that just changed
            HashSet<string> cellsToRecalculate = new HashSet<string>();

            foreach (string recalc in GetCellsToRecalculate(name))
            {
                cellsToRecalculate.Add(recalc);
            }

            return cellsToRecalculate;
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, string text)
        {
            //Null case or if name is invalid
            if (name is null || !ValidCellName(name))
            {
                throw new InvalidNameException();
            }

            //Null case of text
            if (text is null)
            {
                throw new ArgumentNullException();
            }

            //Checking if the cell was nonempty before
            if (cells.ContainsKey(name))
            {
                cells.Remove(name);

                //Deleting all the references to dependees if there are any 
                foreach (string s in dg.GetDependees(name))
                {
                    dg.RemoveDependency(s, name);
                }
            }

            //Add the new cell
            cells.Add(name, new Cell(text));

            //Returning all the cells that depended on this cell
            HashSet<string> cellsRecalc = new HashSet<string>();

            foreach (string recalc in GetCellsToRecalculate(name))
            {
                cellsRecalc.Add(recalc);
            }

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
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            //Null and ivalid cases
            if (name is null || !ValidCellName(name))
            {
                throw new InvalidNameException();
            }

            //Checking to see if all the variables in formula are valid cell names 
            foreach (string v in formula.GetVariables())
            {
                if (!ValidCellName(v))
                {
                    throw new ArgumentException();
                }
            }

            //Keeping track of the old dependees 
            HashSet<string> oldDependees = new HashSet<string>();

            object oldVal = null;

            //Remove all the old dependees and old cell
            if (cells.ContainsKey(name))
            {
                //Keep track of the old value of the cell in case a cycle is found 
                oldVal = cells[name].content;


                foreach (string s in dg.GetDependees(name))
                {
                    oldDependees.Add(s);
                    dg.RemoveDependency(s, name);
                }

                cells.Remove(name);
            }
            //Add the new cell 
            cells.Add(name, new Cell(formula));

            //Add all the new dependees
            foreach (string dep in formula.GetVariables())
            {
                dg.AddDependency(dep, name);
            }

            try
            {
                //get a list of all the dependents to recalc
                //If a cycle is detected, a circular exception is thrown from within GetCellsToRecaculate
                HashSet<string> recalc = new HashSet<string>();

                foreach (string calc in GetCellsToRecalculate(name))
                {
                    recalc.Add(calc);
                }

                return recalc;
            }
            catch (CircularException e)
            {
                // removing all the new dependecies added
                foreach (string dep in formula.GetVariables())
                {
                    dg.RemoveDependency(dep, name);
                }

                if (oldVal != null)
                {
                    //Adding in all the old dependencies
                    foreach (string dep in oldDependees)
                    {
                        dg.AddDependency(dep, name);
                    }

                    //Add the old cell value back
                    cells.Add(name, new Cell(oldVal));
                }

                throw e;
            }
        }

        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
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
            //Null case
            if (name is null)
            {
                throw new ArgumentNullException();
            }

            //Testing name validity
            if (!ValidCellName(name))
            {
                throw new InvalidNameException();
            }

            //Make a hashset to make sure there are no duplicates of dependents
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

        private bool ValidCellName(string name)
        {

            //Creates a regex that makes sure there is at least 
            //one or more letter followed by a nonzero digit followed by 0 or more digits
            Regex reg = new Regex(@"^[A-z]+[1-9]{1}[\d]*$");
            return reg.IsMatch(name);
        }

    }

    /// <summary>
    /// Helper struct that keeps track of the properties of cells
    /// </summary>
    struct Cell
    {
        /// <summary>
        /// Content of the cell 
        /// </summary>
        public object content { get; }

        /// <summary>
        /// Constructor that takes in a single param, the contents of the cell
        /// </summary>
        /// <param name="_content"></param>
        public Cell(object _content)
        {
            content = _content;
        }

    }
}
