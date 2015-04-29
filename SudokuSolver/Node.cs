using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
    public class Node
    {
        public static Random r = new Random();
        //The possible numbers a node can have.
        private List<int> possible;
        //The definite answer for a node. Set to 0 if unknown.
        private int definite;

        public Node(int val)
        {
            definite = val;
            possible = new List<int>();
            if (val == 0)
            {
                for (int i = 1; i < 10; i++)
                {
                    possible.Add(i);
                }
            }
        }

        public void SetDefinite(int i)
        {
            if (IsPossible(i))
            {
                definite = i;
                possible.Clear();
            }
        }

        public bool IsPossible(int i)
        {
            if (possible.Contains(i)) return true;
            else return false;
        }

        public void RemovePossible(int toRemove)
        {
            if (IsPossible(toRemove))
            {
                possible.Remove(toRemove);
            }
            if (possible.Count == 1)
            {
                SetDefinite(possible.ElementAt(0));
            }
        }

        public void ClearExcept(int a, int b)
        {
            possible.Clear();
            possible.Add(a);
            possible.Add(b);
        }

        public void Guess()
        {
            if (definite == 0)
            {
                int index = r.Next(possible.Count);
                this.SetDefinite(possible.ToArray()[index]);
            }
        }

        public int getDefinite
        {
            get
            {
                return definite;
            }
        }
    }
}
