using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAlgorithms.Numerical
{
    /// <summary>
    /// Represents eight queens positioning problem on the chess board so that none of them can attack any of the other queens(no 2 queens on the same row, column or diagonal).
    /// </summary>
    /// <remarks>
    /// Algorithm uses backtracking to check if current step leads to a solution.
    /// </remarks>
    public class EightQueensProblem
    {
        private ushort queensPositioned;
        public bool?[,] Positions { get; set; }
        public ushort FirstRow { get; }
        public ushort FirstColumn { get; }

        /// <summary>
        /// Initializes chess grid and places first queen in the given field
        /// </summary>
        /// <param name="firstRow">First queens' row position</param>
        /// <param name="firstColumn">First queens' column position</param>
        public EightQueensProblem(ushort firstRow, ushort firstColumn)
        {
            if (firstRow < 0 | firstRow > 7 | firstColumn < 0 | firstColumn > 7) throw new ArgumentOutOfRangeException("Column and row indices must be between 1...7 inclusive");
            FirstRow = firstRow; FirstColumn = firstColumn;
            Positions = new bool?[8, 8];
            PlaceQueen(FirstRow, FirstColumn);
        }
        private bool PlaceQueen(int row, int col)
        {
            if (Positions[row, col].HasValue && Positions[row, col].Value == true) return false;
            Positions[row, col] = true;
            queensPositioned++;
            return true;
        }
        private void UndoPlacement(int row, int col)
        {
            Positions[row, col] = false;
            queensPositioned--;
        }
        /// <summary>
        /// Recursive algorithm using backtracking (tests if given step leads to a failure somehow for the next steps and cuts down all farther continuations of that step). Steps are choosen greedly (closest vacant field for next position).
        /// </summary>
        /// <returns>True if the problem is solved, false otherwise.</returns>
        /// <remarks>The runtime can be improved if we place queens more wisely (at the time of placement all illegal fields are marked to prevent the future placements).</remarks>
        public bool Solve()
        {
            if (!IsValid()) return false;                   //current configuration of queens is invalid, can't proceed
            if (queensPositioned == 8) return true;         //current conf. is valid & all queens are positioned

            // Place the next queen to a closest possible position and see if this lead a solution. Otherwise, abandon current step and try next closest position.
            for (int row = 0; row < 8; row++)
                for (int col = 0; col < 8; col++)
                {
                    if (PlaceQueen(row, col))               //place a queen on the first empty place
                    {
                        if (Solve()) return true;           //see if placing a queen leads to a solution
                        UndoPlacement(row, col);            //placing a queen did not lead to a solution, undo that step
                    }
                }
            return false;                                   //no solution is available
        }
        private bool IsValid()
        {
            int rowIndex, colIndex;
            for (int row = 0; row < 8; row++)
                for (int column = 0; column < 8; column++)
                {
                    bool queenPlaced = Positions[row, column].HasValue ? Positions[row, column].Value : false;
                    if (queenPlaced)
                    {
                        //no 2 queens on the same row
                        for (int i = 0; i < 8; i++)
                            if (column != i && Positions[row, i].HasValue)
                                if (Positions[row, i].Value) return false;

                        // no 2 queens on the same column
                        for (int i = 0; i < 8; i++)
                            if (row != i && Positions[i, column].HasValue)
                                if (Positions[i, column].Value) return false;

                        // no 2 queens on the same diagonal
                        rowIndex = row + 1; colIndex = column + 1;
                        while (colIndex < 8 & rowIndex < 8 & colIndex >= 0 & rowIndex >= 0)
                        {
                            if (Positions[rowIndex, colIndex].HasValue && Positions[rowIndex, colIndex].Value) return false;
                            rowIndex++; colIndex++;
                        }
                        rowIndex = row - 1; colIndex = column - 1;
                        while (colIndex < 8 & rowIndex < 8 & colIndex >= 0 & rowIndex >= 0)
                        {
                            if (Positions[rowIndex, colIndex].HasValue && Positions[rowIndex, colIndex].Value) return false;
                            rowIndex--; colIndex--;
                        }
                        rowIndex = row + 1; colIndex = column - 1;
                        while (colIndex < 8 & rowIndex < 8 & colIndex >= 0 & rowIndex >= 0)
                        {
                            if (Positions[rowIndex, colIndex].HasValue && Positions[rowIndex, colIndex].Value) return false;
                            rowIndex++; colIndex--;
                        }
                        rowIndex = row - 1; colIndex = column + 1;
                        while (colIndex < 8 & rowIndex < 8 & colIndex >= 0 & rowIndex >= 0)
                        {
                            if (Positions[rowIndex, colIndex].HasValue && Positions[rowIndex, colIndex].Value) return false;
                            rowIndex--; colIndex++;
                        }
                    }
                }
            return true;
        }
    }
}
