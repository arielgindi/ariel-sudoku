using System.Diagnostics;

internal class SudokuSolver
{
    private SudokuBoard board;
    private const int BoardSize = 9;
    private const int TimeLimitMilliseconds = 1000;

    public void Solve(SudokuBoard sudokuBoard)
    {
        board = sudokuBoard;

        Stopwatch stopwatch = Stopwatch.StartNew();

        bool solved = Backtrack(stopwatch);
        if (!solved)
        {
            throw new InvalidOperationException("Puzzle is unsolvable or incomplete.");
        }
    }

    /// <summary>
    /// Backtracking that checks elapsed time to avoid exceeding 1 second.
    /// </summary>
    private bool Backtrack(Stopwatch stopwatch)
    {
        // If we exceed the time limit, throw an exception
        if (stopwatch.ElapsedMilliseconds > TimeLimitMilliseconds)
        {
            throw new TimeoutException("Puzzle took more than 1 second to solve.");
        }

        for (int row = 0; row < BoardSize; row++)
        {
            for (int col = 0; col < BoardSize; col++)
            {
                // Look for an empty cell
                if (board[row, col] == '0')
                {
                    // Try digits 1..9
                    for (int digit = 1; digit <= 9; digit++)
                    {
                        if (board.IsSafeCell(row, col, digit))
                        {
                            board.PlaceDigit(row, col, digit);

                            if (Backtrack(stopwatch))
                            {
                                return true;
                            }

                            board.RemoveDigit(row, col, digit);
                        }
                    }
                    // If no digit fits, we need to backtrack
                    return false;
                }
            }
        }
        // No empty cell => puzzle is solved
        return true;
    }
}
