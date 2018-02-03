// Skeleton implementation written by Joe Zachary for CS 3500, January 2018.
// Remainder implementation written by Nithin Chalapathi u0847388, CS3500, spring '18

using System;
using System.Collections.Generic;

namespace Dependencies
{
    /// <summary>
    /// A DependencyGraph can be modeled as a set of dependencies, where a dependency is an ordered 
    /// pair of strings.  Two dependencies (s1,t1) and (s2,t2) are considered equal if and only if 
    /// s1 equals s2 and t1 equals t2.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that the dependency (s,t) is in DG 
    ///    is called the dependents of s, which we will denote as dependents(s).
    ///        
    ///    (2) If t is a string, the set of all strings s such that the dependency (s,t) is in DG 
    ///    is called the dependees of t, which we will denote as dependees(t).
    ///    
    /// The notations dependents(s) and dependees(s) are used in the specification of the methods of this class.
    ///
    /// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    ///     dependents("a") = {"b", "c"}
    ///     dependents("b") = {"d"}
    ///     dependents("c") = {}
    ///     dependents("d") = {"d"}
    ///     dependees("a") = {}
    ///     dependees("b") = {"a"}
    ///     dependees("c") = {"a"}
    ///     dependees("d") = {"b", "d"}
    ///     
    /// All of the methods below require their string parameters to be non-null.  This means that 
    /// the behavior of the method is undefined when a string parameter is null.  
    ///
    /// IMPORTANT IMPLEMENTATION NOTE
    /// 
    /// The simplest way to describe a DependencyGraph and its methods is as a set of dependencies, 
    /// as discussed above.
    /// 
    /// However, physically representing a DependencyGraph as, say, a set of ordered pairs will not
    /// yield an acceptably efficient representation.  DO NOT USE SUCH A REPRESENTATION.
    /// 
    /// You'll need to be more clever than that.  Design a representation that is both easy to work
    /// with as well acceptably efficient according to the guidelines in the PS3 writeup. Some of
    /// the test cases with which you will be graded will create massive DependencyGraphs.  If you
    /// build an inefficient DependencyGraph this week, you will be regretting it for the next month.
    /// </summary>
    public class DependencyGraph
    {
        /// <summary>
        /// Contains a list of all the dependees in the graph and a linkedlist of all the dependants it links to 
        /// </summary>
        private Dictionary<string, LinkedList<string>> dependees;

        /// <summary>
        /// Contains a list of all possible dependents and a linked list of all the dependees it links to
        /// </summary>
        private Dictionary<string, LinkedList<string>> dependents;

        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            dependents = new Dictionary<string, LinkedList<string>>();
            dependees = new Dictionary<string, LinkedList<string>>();
        }

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get
            {
                int sum = 0;
                if (dependees.Count < dependents.Count)
                {
                    foreach (LinkedList<string> list in dependees.Values)
                    {
                        sum += list.Count;
                    }
                }
                else
                {
                    foreach (LinkedList<string> list in dependents.Values)
                    {
                        sum += list.Count;
                    }
                }
                return sum;
            }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependents(string s)
        {
            //Null check
            if (s is null)
            {
                return false;
            }


            //If key exists and the associated linkedlist is longer than 0, return true
            if (dependees.ContainsKey(s) && dependees[s].Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependees(string s)
        {
            //Null check 
            if (s is null)
            {
                return false;
            }

            //If key s exists and the associated linked list is longer than length 0, return true
            if (dependents.ContainsKey(s) && dependents[s].Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Enumerates dependents(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {

            //null case
            if (s is null)
            {
                yield break;
            }

            //If s is a key, return each dependent
            if (dependees.ContainsKey(s))
            {
                foreach (string dep in dependees[s])
                {
                    yield return dep;
                }
            }
        }

        /// <summary>
        /// Enumerates dependees(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            //Null case 
            if (s is null)
            {
                yield break;
            }

            //If s is a key, return each dependee
            if (dependents.ContainsKey(s))
            {
                foreach (string dep in dependents[s])
                {
                    yield return dep;
                }
            }
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void AddDependency(string s, string t)
        {
            //Null case
            if (s is null || t is null)
            {
                return;
            }

            //Check to see if s is in the dependees
            //If not, add it and createa new linked list
            //Add t to the end of the linked list
            if (!dependees.ContainsKey(s))
            {
                dependees.Add(s, new LinkedList<string>());
            }

            if (!dependees[s].Contains(t))
            {
                dependees[s].AddLast(t);
            }

            //Check to see if t is in the dependents
            //If not, add it and createa new linked list
            //Add s to the end of the linked list
            if (!dependents.ContainsKey(t))
            {
                dependents.Add(t, new LinkedList<string>());
            }

            if (!dependents[t].Contains(s))
            {
                dependents[t].AddLast(s);
            }
        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            //Null case
            if (s is null || t is null)
            {
                return;
            }

            //Removing from the dependee dict
            if (dependees.ContainsKey(s))
            {
                dependees[s].Remove(t);
            }

            //Removing from the dependent dict
            if (dependents.ContainsKey(t))
            {
                dependents[t].Remove(s);
            }

        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {

            if (s is null || newDependents is null)
            {
                return;
            }

            //Loops through all the dependents of s and deletes s from their linkedlist
            if (dependees.ContainsKey(s))
            {
                foreach (string t in dependees[s])
                {
                    if (dependents.ContainsKey(t))
                    {
                        dependents[t].Remove(s);
                    }
                }

                //Remove the old value for s
                dependees.Remove(s);
            }

            //Creates an entry for s
            dependees.Add(s, new LinkedList<string>());


            //Add each one of the new dependents
            foreach (string t in newDependents)
            {
                AddDependency(s, t);
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            if (t is null || newDependees is null)
            {
                return;
            }

            //Loops through all the dependees of t and deletes t from the linkedlist of those dependees
            if (dependents.ContainsKey(t))
            {
                foreach (string s in dependents[t])
                {
                    if (dependees.ContainsKey(s))
                    {
                        dependees[s].Remove(t);
                    }
                }

                //Create a new linked list
                dependents.Remove(t);
            }
            

            //Creates a new extry for t
            dependents.Add(t, new LinkedList<string>());


            //Add each of the new dependees
            foreach (string s in newDependees)
            {
                AddDependency(s, t);
            }
        }
    }
}
