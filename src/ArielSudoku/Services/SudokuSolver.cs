using ArielSudoku.Exceptions;
using ArielSudoku.Models;
using System.Diagnostics;

internal class SudokuSolver
{
    public int BacktrackCallAmount { get; private set; }
    private readonly SudokuBoard _board;
    private readonly Stopwatch _stopwatch;
    private const int _TimeLimitMilliseconds = 1000;
    private const int _CheckFrequency = 1000;

    public SudokuSolver(SudokuBoard sudokuBoard)
    {
        _board = sudokuBoard;
        _stopwatch = new Stopwatch();
    }

    /// <summary>
    /// Try to solve the board up to 1 sec
    /// </summary>
    /// <exception cref="UnsolvableSudokuException">Thrown if puzzle cannot be solve</exception>
    public void Solve()
    {
        _stopwatch.Start();

        ApplyNakedSingles();

        bool solved = Backtrack();
        if (!solved)
        {
            throw new UnsolvableSudokuException("Puzzle is unsolvable or incomplete.");
        }
    }

    // Try to place digits in cells that have exactly one possibility
    private void ApplyNakedSingles()
    {
        bool changed;
        do
        {
            changed = false;
            for (int i = 0; i < CellCount; i++)
            {
                if (_board[i] == '0' && _board.HasSingleOption(i))
                {
                    int digit = _board.GetSingleCandidate(i);
                    _board.PlaceDigit(i, digit);
                    changed = true;
                }
            }
        }
        while (changed);
    }

    /// <summary>
    /// Backtrack with recussin until the sudoku is solved, 
    /// if cannot be solved, return false
    /// </summary>
    /// <param name="emptyCellIndex">Index inside the board of the next cell to check</param>
    /// <returns>True if it was solved</returns>
    /// <exception cref="TimeoutException">Thrown if took more than 1 sec to solve</exception>
    private bool Backtrack(int emptyCellIndex = 0)
    {
        BacktrackCallAmount++;

        // Only check if it took more than 1 sec even 1000 calls to improve performance by 8% on average
        if (BacktrackCallAmount % _CheckFrequency == 0 && _stopwatch.ElapsedMilliseconds > _TimeLimitMilliseconds)
        {
            throw new TimeoutException("Puzzle took more than 1 second to solve.");
        }

        // Meaning board is solved
        if (_board.IsSolved())
        {
            return true;
        }

        // Pick the next empty cell
        int cellNumber = _board.FindLeastOptionsCellIndex();
        if (cellNumber == -1)
        {
            return false;
        }

        // Try digits 1-9
        for (int digit = 1; digit <= BoardSize; digit++)
        {
            if (_board.IsSafeCell(cellNumber, digit))
            {
                _board.PlaceDigit(cellNumber, digit);

                if (Backtrack(emptyCellIndex + 1))
                {
                    return true;
                }

                _board.RemoveDigit(cellNumber, digit);
            }
        }
        return false;
    }
}
