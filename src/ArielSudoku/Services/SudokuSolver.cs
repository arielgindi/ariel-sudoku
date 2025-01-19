using ArielSudoku.Models;
using System.Diagnostics;

internal class SudokuSolver
{
    private SudokuBoard board;
    private SudokuUsage usage; 

    private const int TimeLimitMilliseconds = 1000;
    private List<int> emptyCells;

    public void Solve(SudokuBoard sudokuBoard)
    {
        board = sudokuBoard;
        usage = new SudokuUsage(board);

        Stopwatch stopwatch = Stopwatch.StartNew();

        InitializeSolver();

        bool solved = Backtrack(stopwatch);
        if (!solved)
        {
            throw new InvalidOperationException("Puzzle is unsolvable or incomplete.");
        }
    }

    private void InitializeSolver()
    {
        emptyCells = [];

        for (int cellNumber = 0; cellNumber < CellCount; cellNumber++)
        {
            if (board[cellNumber] == '0')
            {
                emptyCells.Add(cellNumber);
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

        int cellNumber = emptyCells[emptyCellIndex];

        // Try digits 1-9
        for (int digit = 1; digit <= BoardSize; digit++)
        {
            if (usage.IsSafeCell(cellNumber, digit))
            {
                usage.PlaceDigit(cellNumber, digit);

                if (Backtrack(stopwatch, emptyCellIndex + 1))
                {
                    return true;
                }

                usage.RemoveDigit(cellNumber, digit);
            }
        }

        return false;
    }
}
