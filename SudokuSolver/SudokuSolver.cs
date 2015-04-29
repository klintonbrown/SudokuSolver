using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SudokuSolver
{
    public partial class SudokuSolver : Form
    {

        private Board SudokuBoard;
        private TextBox[,] ltb;
        private int numberOfGuesses;
        private static int MAXGUESSES = 100;

        public SudokuSolver()
        {
            InitializeComponent();
            ltb = new TextBox[9, 9];
            InitializeList();
            numberOfGuesses = 0;
        }

        public void FillBoard()
        {
            int[,] board = new int[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    try
                    {
                        board[i, j] = Convert.ToInt32(ltb[i, j].Text);
                        if (board[i, j] > 9 || board[i, j] < 1) throw new Exception();
                    }
                    catch (Exception)
                    {
                        //if the input is anything other than a number from 1 to 9, it is read as 0
                        board[i, j] = 0;
                    }
                }
            }
            SudokuBoard = new Board(board);
        }

        public void InitializeList()
        {
            ltb[0, 0] = ULUL;
            ltb[0, 1] = ULUM;
            ltb[0, 2] = ULUR;
            ltb[0, 3] = UMUL;
            ltb[0, 4] = UMUM;
            ltb[0, 5] = UMUR;
            ltb[0, 6] = URUL;
            ltb[0, 7] = URUM;
            ltb[0, 8] = URUR;

            ltb[1, 0] = ULML;
            ltb[1, 1] = ULMM;
            ltb[1, 2] = ULMR;
            ltb[1, 3] = UMML;
            ltb[1, 4] = UMMM;
            ltb[1, 5] = UMMR;
            ltb[1, 6] = URML;
            ltb[1, 7] = URMM;
            ltb[1, 8] = URMR;

            ltb[2, 0] = ULDL;
            ltb[2, 1] = ULDM;
            ltb[2, 2] = ULDR;
            ltb[2, 3] = UMDL;
            ltb[2, 4] = UMDM;
            ltb[2, 5] = UMDR;
            ltb[2, 6] = URDL;
            ltb[2, 7] = URDM;
            ltb[2, 8] = URDR;

            ltb[3, 0] = MLUL;
            ltb[3, 1] = MLUM;
            ltb[3, 2] = MLUR;
            ltb[3, 3] = MMUL;
            ltb[3, 4] = MMUM;
            ltb[3, 5] = MMUR;
            ltb[3, 6] = MRUL;
            ltb[3, 7] = MRUM;
            ltb[3, 8] = MRUR;

            ltb[4, 0] = MLML;
            ltb[4, 1] = MLMM;
            ltb[4, 2] = MLMR;
            ltb[4, 3] = MMML;
            ltb[4, 4] = MMMM;
            ltb[4, 5] = MMMR;
            ltb[4, 6] = MRML;
            ltb[4, 7] = MRMM;
            ltb[4, 8] = MRMR;

            ltb[5, 0] = MLDL;
            ltb[5, 1] = MLDM;
            ltb[5, 2] = MLDR;
            ltb[5, 3] = MMDL;
            ltb[5, 4] = MMDM;
            ltb[5, 5] = MMDR;
            ltb[5, 6] = MRDL;
            ltb[5, 7] = MRDM;
            ltb[5, 8] = MRDR;

            ltb[6, 0] = DLUL;
            ltb[6, 1] = DLUM;
            ltb[6, 2] = DLUR;
            ltb[6, 3] = DMUL;
            ltb[6, 4] = DMUM;
            ltb[6, 5] = DMUR;
            ltb[6, 6] = DRUL;
            ltb[6, 7] = DRUM;
            ltb[6, 8] = DRUR;

            ltb[7, 0] = DLML;
            ltb[7, 1] = DLMM;
            ltb[7, 2] = DLMR;
            ltb[7, 3] = DMML;
            ltb[7, 4] = DMMM;
            ltb[7, 5] = DMMR;
            ltb[7, 6] = DRML;
            ltb[7, 7] = DRMM;
            ltb[7, 8] = DRMR;

            ltb[8, 0] = DLDL;
            ltb[8, 1] = DLDM;
            ltb[8, 2] = DLDR;
            ltb[8, 3] = DMDL;
            ltb[8, 4] = DMDM;
            ltb[8, 5] = DMDR;
            ltb[8, 6] = DRDL;
            ltb[8, 7] = DRDM;
            ltb[8, 8] = DRDR;
        }

        private void SolveButton_Click(object sender, EventArgs e)
        {
            FillBoard();
            SudokuBoard.SolveBoard();

            //If puzzle hasn't been completely solved, guess and check.
            if (SudokuBoard.getNumFound > 16 && SudokuBoard.getNumFound < 81)
            {
                GuessAndCheck();
            }
            PrintBoard();
        }

        public void PrintBoard()
        {
            Node[,] temp = SudokuBoard.getArray;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    ltb[i, j].Text = temp[i, j].getDefinite.ToString();
                }
            }
            SolutionIndicator.Text = numberOfGuesses.ToString() + " guesses";
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    ltb[i, j].Clear();
                }
            }
            numberOfGuesses = 0;
            SolutionIndicator.Clear();
        }

        private void GuessAndCheck()
        {
            Board tempBoard;
            bool check;
            do
            {
                tempBoard = SudokuBoard.CopyBoard();
                check = tempBoard.GuessAndCheck();
                numberOfGuesses++;
                Console.WriteLine(numberOfGuesses);
            } while (!check && numberOfGuesses < MAXGUESSES);

            if (check)
                SudokuBoard = tempBoard;
            else MessageBox.Show("Could not solve in " + MAXGUESSES + " guesses.");
        }
    }
}
