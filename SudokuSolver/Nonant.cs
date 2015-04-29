using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
    class Nonant
    {
        private Node[,] nonant;
        private List<int> numsFound;
        private List<int> numsMissing;
        private Line[] rows;
        private Line[] cols;
        private int numAnswered;

        public Nonant(Node[,] arr, Line[] r, Line[] c)
        {
            numsFound = new List<int>();
            numsMissing = new List<int>();
            nonant = arr;
            rows = r;
            cols = c;
            for (int i = 1; i < 10; i++)
            {
                numsMissing.Add(i);
            }
        }

        public void FindNumsFound()
        {
            int temp;
            numsFound.Clear();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    temp = nonant[i, j].getDefinite;
                    if (temp != 0)
                    {
                        numsFound.Add(temp);
                        numsMissing.Remove(temp);
                    }
                }
            }

            numAnswered = numsFound.Count;
        }

        public void EvaluateNonant()
        {
            ScanNodes();

            for (int i = 0; i < 3; i++)
            {
                rows[i].EvaluateLine();
                ScanNodes();
                cols[i].EvaluateLine();
                ScanNodes();
            }

            int count;
            Node temp = null;
            foreach (int i in numsMissing)
            {
                count = 0;
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        if (nonant[j, k].IsPossible(i))
                        {
                            count++;
                            temp = nonant[j, k];
                        }
                    }
                }
                if (count == 1)
                {
                    temp.SetDefinite(i);
                }
            }

            int curNumFound = numsFound.Count;
            FindNumsFound();
            if (curNumFound < numsFound.Count)
            {
                EvaluateNonant();
            }
        }

        public void ScanNodes()
        {
            FindNumsFound();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    foreach (int k in numsFound)
                    {
                        nonant[i, j].RemovePossible(k);
                    }
                }
            }
        }

        public bool CheckNonant()
        {
            int count;
            for (int i = 1; i < 10; i++)
            {
                count = 0;
                foreach (Node n in nonant)
                {
                    if (n.getDefinite == i) count++;
                }
                if (count != 1) return false;
            }
            return true;
        }

        public void GuessNodes()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (nonant[i, j].getDefinite == 0)
                    {
                        nonant[i, j].Guess();
                        EvaluateNonant();
                    }
                }
            }
        }

        public int getNumberAnswered
        {
            get
            {
                FindNumsFound();
                return numAnswered;
            }
        }

        public Node[,] getNodes
        {
            get
            {
                return nonant;
            }
        }
    }
}
