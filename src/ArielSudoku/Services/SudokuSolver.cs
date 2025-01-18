using System.Diagnostics;

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

        for (int cellNumber = 0; cellNumber < CellCount; cellNumber++)
        {
            (int row, int col, _) = SudokuBoard.GetCellCoordinates(cellNumber);
            if (board[row, col] == '0')
            {
                emptyCells.Add(cellNumber);
            }
        }
    }

    /// <summary>
    /// Backtracking that checks elapsed time to avoid exceeding 1 second.
    /// </summary>
    private bool Backtrack(Stopwatch stopwatch, int index = 0)
    {
        // If we exceed the time limit, throw an exception
        if (stopwatch.ElapsedMilliseconds > TimeLimitMilliseconds)
        {
            throw new TimeoutException("Puzzle took more than 1 second to solve.");
        }

        // meaning board is now solved
        if (index == emptyCells.Count)
        {
            return true;
        }

        (int row, int col, _) = SudokuBoard.GetCellCoordinates(emptyCells[index]);

        for (int digit = 1; digit <= BoardSize; digit++)
        {
            if (board.IsSafeCell(row, col, digit))
            {
                board.PlaceDigit(row, col, digit);

                if (Backtrack(stopwatch, index + 1))
                {
                    return true;
                }

                board.RemoveDigit(row, col, digit);
            }
        }
        return false;
    }
}
