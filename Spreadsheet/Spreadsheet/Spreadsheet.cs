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
            if(name is null || !ValidCellName(name))
            {
                throw new InvalidNameException();
            }

            //Checking if the cells dictionary already contains the key of name
            //If so we remove it to make a new entry
            if (cells.ContainsKey(name))
            {
                cells.Remove(name);
            }

            //Add the new cell value
            cells.Add(name, new Cell(number));

            //Deleting all the old dependees since it is just a double now
            //Doubles will never have an dependees
            foreach(string s in dg.GetDependees(name))
            {
                dg.RemoveDependency(s, name);
            }

            //Finds all the cells that are dependent on the cell that just changed
            HashSet<string> cellsToRecalculate = new HashSet<string>();

            foreach (string recalc in GetCellsToRecalculate(name)) {
                cellsToRecalculate.Add(recalc);
            }

            return cellsToRecalculate;
        }

        public override ISet<string> SetCellContents(string name, string text)
        {
            throw new NotImplementedException();
        }

        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            throw new NotImplementedException();
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
