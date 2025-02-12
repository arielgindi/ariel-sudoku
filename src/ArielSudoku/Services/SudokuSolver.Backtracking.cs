using ArielSudoku.Models;

namespace ArielSudoku.Services;
public sealed partial class SudokuSolver
{
    /// <summary>
    /// Backtracking with recursion until the sudoku is solved,
    /// if cannot be solved, return false
    /// </summary>
    /// <param name="emptyCellIndex">Index inside the board of the next cell to check</param>
    /// <returns>True if it was solved</returns>
    /// <exception cref="TimeoutException">Thrown if took more than 1 sec to solve</exception>
    private bool Backtrack()
    {
        if (_cancellationToken.IsCancellationRequested)
        {
            return false;
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

        // Take a snapshot of the boardUsage before a guess
        SudokuBoard.SudokuSnapshot snapshot = _board.TakeSnapshot();

        // Try digits 1-boardSize
        for (int digit = 1; digit <= _constants.BoardSize; digit++)
        {
            if (!_board.IsSafeCell(cellNumber, digit))
                continue;

            _runtimeStats.GuessCount++;
            _board.PlaceDigit(cellNumber, digit);

            ApplyHumanTactics();

            if (_board.HasDeadEnd())
            {
                // If it's a dead end, restore the snapshot and skip to the next digit
                _board.RestoreSnapshot(snapshot);
                continue;
            }

            if (_board.IsSolved() || Backtrack())
            {
                return true;
            }

            _board.RestoreSnapshot(snapshot);
        }
        return false;
    }
}