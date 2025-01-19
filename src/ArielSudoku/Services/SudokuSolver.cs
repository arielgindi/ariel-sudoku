using ArielSudoku.Models;
using System.Diagnostics;
using static ArielSudoku.Common.Constants;

internal class SudokuSolver
{
    private SudokuBoard board;
    private const int TimeLimitMilliseconds = 1000;
    private List<int> emptyCells;

    public void Solve(SudokuBoard sudokuBoard)
    {
        board = sudokuBoard;
        Stopwatch stopwatch = Stopwatch.StartNew();
        InitilazeSolver();

        bool solved = Backtrack(stopwatch);
        if (!solved)
        {
            throw new InvalidOperationException("Puzzle is unsolvable or incomplete.");
        }
    }

    private void InitilazeSolver()
    {
        emptyCells = [];

        for (int boardIndex = 0; boardIndex < CellCount; boardIndex++)
        {
            if (board[boardIndex] == '0')
            {
                emptyCells.Add(boardIndex);
            }
        }
    }

    /// <summary>
    /// Backtracking that checks elapsed time to avoid exceeding 1 second.
    /// </summary>
    private bool Backtrack(Stopwatch stopwatch, int emptyCellIndex = 0)
    {
        // If we exceed the time limit, throw an exception
        if (stopwatch.ElapsedMilliseconds > TimeLimitMilliseconds)
        {
            throw new TimeoutException("Puzzle took more than 1 second to solve.");
        }

        // meaning board is now solved
        if (emptyCellIndex == emptyCells.Count)
        {
            return true;
        }

        int boardCellIndex = emptyCells[emptyCellIndex];

        for (int digit = 1; digit <= BoardSize; digit++)
        {
            if (board.IsSafeCell(boardCellIndex, digit))
            {
                board.PlaceDigit(boardCellIndex, digit);

                if (Backtrack(stopwatch, emptyCellIndex + 1))
                {
                    return true;
                }

                board.RemoveDigit(boardCellIndex, digit);
            }
        }
        return false;
    }
}
