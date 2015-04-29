using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SudokuSolver
{
    public class Line
    {

        private Node[] arr;

        //The numbers in the line that are known, i.e. the solved nodes.
        private List<int> numsFound;

        //The numbers in the line that have not yet been found.
        private List<int> numsMissing;

        //Each seg represents the segments in different nonants.
        private Node[] seg1;
        private Node[] seg2;
        private Node[] seg3;

        public Line(Node[] n)
        {
            arr = n;
            seg1 = new Node[3] { arr[0], arr[1], arr[2] };
            seg2 = new Node[3] { arr[3], arr[4], arr[5] };
            seg3 = new Node[3] { arr[6], arr[7], arr[8] };
            numsFound = new List<int>();
            numsMissing = new List<int>();
            for (int i = 1; i < 10; i++)
            {
                numsMissing.Add(i);
            }
            FindNumsFound();
        }

        /// <summary>
        /// Finds the numbers in the row that have already been located.
        /// </summary>
        public void FindNumsFound()
        {
            int numTemp;
            numsFound.Clear();
            for (int i = 0; i < 9; i++)
            {
                numTemp = arr[i].getDefinite;
                if (numTemp != 0)
                {
                    numsFound.Add(numTemp);
                    numsMissing.Remove(numTemp);
                }
            }
        }

        public void EvaluateLine()
        {
            FindNumsFound();
            foreach (Node n in arr)
            {
                foreach (int i in numsFound)
                {
                    n.RemovePossible(i);
                }
            }

            int curNumFound = numsFound.Count;
            FindNumsFound();

            //if (numsMissing.Count > 2) FindTwins();
            if (curNumFound < numsFound.Count)
            {
                EvaluateLine();
            }
        }

        public void FindTwins()
        {
            int count;
            List<int> NumsInTwins = new List<int>();

            for (int i = 1; i < 10; i++)
            {
                count = 0;
                for (int j = 0; j < 9; j++)
                {
                    if (arr[j].IsPossible(i)) count++;
                }
                if (count == 2) NumsInTwins.Add(i);
            }

            if (NumsInTwins.Count == 2)
            {

                int[] NITarray = NumsInTwins.ToArray();
                List<Node> twins = new List<Node>();
                for (int i = 0; i < 9; i++)
                {
                    if (arr[i].IsPossible(NITarray[0]) && arr[i].IsPossible(NITarray[1]))
                    {
                        twins.Add(arr[i]);
                    }
                }

                if (twins.Count == 2)
                {
                    foreach (Node n in twins)
                    {
                        n.ClearExcept(NITarray[0], NITarray[1]);
                    }
                }
            }
        }

        public bool CheckLine()
        {
            int count;
            for (int i = 1; i < 10; i++)
            {
                count = 0;
                foreach (Node n in arr)
                {
                    if (n.getDefinite == i) count++;
                }
                if (count != 1) return false;
            }
            return true;
        }

        public void GuessNodes()
        {
            for (int i = 0; i < 9; i++)
            {
                if (arr[i].getDefinite == 0)
                {
                    arr[i].Guess();
                    EvaluateLine();
                }
            }
        }

        public List<int> getNumsFound
        {
            get
            {
                return numsFound;
            }
        }

        public List<int> getNumsMissing
        {
            get
            {
                return numsMissing;
            }
        }

        public int getNumberFound
        {
            get
            {
                return numsFound.Count;
            }
        }

    }
}
