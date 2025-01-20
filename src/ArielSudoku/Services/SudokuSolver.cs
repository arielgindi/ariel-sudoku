using ArielSudoku.Models;
using System.Diagnostics;
using static ArielSudoku.Common.Constants;

internal class SudokuSolver
{
    private readonly SudokuBoard board;
    private const int TimeLimitMilliseconds = 1000;
    private readonly Stopwatch stopwatch;

    public SudokuSolver(SudokuBoard sudokuBoard)
    {
        board = sudokuBoard ?? throw new ArgumentNullException(nameof(sudokuBoard));
        stopwatch = new Stopwatch();
    }

    public void Solve()
    {
        stopwatch.Restart();

        bool solved = Backtrack();
        if (!solved)
        {
            throw new InvalidOperationException("Puzzle is unsolvable or incomplete.");
        }
    }
    /// <summary>
    /// Backtracking that checks elapsed time to avoid exceeding 1 second.
    /// </summary>
    private bool Backtrack(int emptyCellIndex = 0)
    {
        // If we exceed the time limit, throw an exception
        if (stopwatch.ElapsedMilliseconds > TimeLimitMilliseconds)
        {
            throw new TimeoutException("Puzzle took more than 1 second to solve.");
        }

        // meaning board is now solved
        if (emptyCellIndex == board.EmptyCells.Count)
        {
            return true;
        }

        // Pick the next empty cell
        int cellNumber = board.EmptyCells[emptyCellIndex];
         
        // Try digits 1-9
        for (int digit = 1; digit <= BoardSize; digit++)
        {
            if (board.IsSafeCell(cellNumber, digit))
            {
                board.PlaceDigit(cellNumber, digit);

                if (Backtrack(emptyCellIndex + 1))
                {
                    return true;
                }

                board.RemoveDigit(cellNumber, digit);
            }
        }

        return false;
    }

    private bool IsComplete() => board.All(cell => cell != '0');
}
