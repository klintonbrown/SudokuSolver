using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SudokuSolver
{
    class Board
    {

        private int[,] board;
        private Node[,] MainBoard;
        private Line[] rows;
        private Line[] cols;
        private Nonant[,] sections;
        private int numberFound;

        public Board(int[,] intBoard)
        {
            board = intBoard;
            MainBoard = new Node[9, 9];
            rows = new Line[9];
            cols = new Line[9];
            sections = new Nonant[3, 3];
            numberFound = 0;

            MakeBoard();
            MakeLines();
            MakeNonants();
            CalculateNumFound();
        }

        public Line[] getRows
        {
            get
            {
                return rows;
            }
        }

        public Line[] getCols
        {
            get
            {
                return cols;
            }
        }

        public Nonant[,] getNonants
        {
            get
            {
                return sections;
            }
        }

        public Node[,] getArray
        {
            get
            {
                return MainBoard;
            }
        }

        public int[,] getValArray
        {
            get
            {
                return board;
            }
        }

        public int getNumFound
        {
            get
            {
                return numberFound;
            }
        }

        public void SolveBoard()
        {
            if (numberFound > 16) //The fewest givens a sudoku puzzle can have and still be solvable is 17.
            {
                int oldNumFound;
                do
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            sections[i, j].EvaluateNonant();
                        }
                    }

                    oldNumFound = numberFound;
                    CalculateNumFound();
                } while (oldNumFound < numberFound);
            }
            else MessageBox.Show("Too few givens available to solve this puzzle.");
        }

        public bool GuessAndCheck()
        {
            while (numberFound < 81)
            {
                Nonant tempNon = FindMostSolvedNonant(); //find the nonant with the fewest
                                                         //answers left to find
                //Line tempLine = FindMostSolvedLine();   //find the line with the fewest answers left to find

                //if (tempNon.getNumberAnswered > tempLine.getNumberFound)
                //{
                //    tempNon.GuessNodes();
                //}
                //else tempLine.GuessNodes();
                tempNon.GuessNodes();
                SolveBoard();
            }
            foreach (Nonant non in sections)
            {
                if (!non.CheckNonant()) return false;
            }
            for (int i = 0; i < 9; i++)
            {
                if (!rows[i].CheckLine()) return false;
                if (!cols[i].CheckLine()) return false;
            }
            return true;
        }

        /// <summary>
        /// Finds the number of solved nodes.
        /// </summary>
        private void CalculateNumFound()
        {
            numberFound = 0;
            for (int i = 0; i < 9; i++)
            {
                numberFound += rows[i].getNumberFound;
            }
        }

        /// <summary>
        /// Creates a board of Nodes
        /// </summary>
        private void MakeBoard()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    MainBoard[i, j] = new Node(board[i, j]);
                }
            }
        }

        /// <summary>
        /// Creates the rows and columns of the board
        /// </summary>
        private void MakeLines()
        {
            Node[] rowTemp;
            Node[] colTemp;

            for (int i = 0; i < 9; i++)
            {
                rowTemp = new Node[9];
                colTemp = new Node[9];
                for (int j = 0; j < 9; j++)
                {
                    rowTemp[j] = MainBoard[i, j];
                    colTemp[j] = MainBoard[j, i];
                }
                rows[i] = new Line(rowTemp);
                cols[i] = new Line(colTemp);
            }
        }

        /// <summary>
        /// Creates the Nonants of the board.
        /// </summary>
        private void MakeNonants()
        {
            Node[,] temp1 = new Node[3, 3];
            Node[,] temp2 = new Node[3, 3];
            Node[,] temp3 = new Node[3, 3];
            Node[,] temp4 = new Node[3, 3];
            Node[,] temp5 = new Node[3, 3];
            Node[,] temp6 = new Node[3, 3];
            Node[,] temp7 = new Node[3, 3];
            Node[,] temp8 = new Node[3, 3];
            Node[,] temp9 = new Node[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    temp1[i, j] = MainBoard[i, j];
                    temp2[i, j] = MainBoard[i, j + 3];
                    temp3[i, j] = MainBoard[i, j + 6];
                    temp4[i, j] = MainBoard[i + 3, j];
                    temp5[i, j] = MainBoard[i + 3, j + 3];
                    temp6[i, j] = MainBoard[i + 3, j + 6];
                    temp7[i, j] = MainBoard[i + 6, j];
                    temp8[i, j] = MainBoard[i + 6, j + 3];
                    temp9[i, j] = MainBoard[i + 6, j + 6];
                }
            }

            sections[0, 0] = new Nonant(temp1, new Line[3] { rows[0], rows[1], rows[2] }, new Line[3] { cols[0], cols[1], cols[2] });

            sections[0, 1] = new Nonant(temp2, new Line[3] { rows[0], rows[1], rows[2] }, new Line[3] { cols[3], cols[4], cols[5] });

            sections[0, 2] = new Nonant(temp3, new Line[3] { rows[0], rows[1], rows[2] }, new Line[3] { cols[6], cols[7], cols[8] });

            sections[1, 0] = new Nonant(temp4, new Line[3] { rows[3], rows[4], rows[5] }, new Line[3] { cols[0], cols[1], cols[2] });

            sections[1, 1] = new Nonant(temp5, new Line[3] { rows[3], rows[4], rows[5] }, new Line[3] { cols[3], cols[4], cols[5] });

            sections[1, 2] = new Nonant(temp6, new Line[3] { rows[3], rows[4], rows[5] }, new Line[3] { cols[6], cols[7], cols[8] });

            sections[2, 0] = new Nonant(temp7, new Line[3] { rows[6], rows[7], rows[8] }, new Line[3] { cols[0], cols[1], cols[2] });

            sections[2, 1] = new Nonant(temp8, new Line[3] { rows[6], rows[7], rows[8] }, new Line[3] { cols[3], cols[4], cols[5] });

            sections[2, 2] = new Nonant(temp9, new Line[3] { rows[6], rows[7], rows[8] }, new Line[3] { cols[6], cols[7], cols[8] });
        }

        private Nonant FindMostSolvedNonant()
        {
            Nonant temp = null;

            foreach (Nonant n in sections)
            {
                if (n.getNumberAnswered < 9)
                {
                    temp = n;
                    break;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (sections[i, j].getNumberAnswered < 9 && sections[i,j].getNumberAnswered > temp.getNumberAnswered)
                    {
                        temp = sections[i, j];
                    }
                }
            }
            return temp;
        }

        private Line FindMostSolvedLine()
        {
            Line temp = rows[0];

            for (int i = 0; i < 9; i++)
            {
                if (rows[i].getNumberFound < 9 && rows[i].getNumberFound > temp.getNumberFound)
                {
                    temp = rows[i];
                }
                if (cols[i].getNumberFound < 9 && cols[i].getNumberFound > temp.getNumberFound)
                {
                    temp = cols[i];
                }
            }
            return temp;
        }

        public Board CopyBoard()
        {
            int[,] tempBoard = new int[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    tempBoard[i, j] = MainBoard[i, j].getDefinite;
                }
            }
            return new Board(tempBoard);
        }
    }
}
