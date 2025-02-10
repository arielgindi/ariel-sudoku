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
        BacktrackCallAmount++;

        // Only check if it took more than 1 sec every 1000 calls to improve performance
        if (BacktrackCallAmount % _CheckFrequency == 0 && _stopwatch.ElapsedMilliseconds > _TimeLimitMilliseconds)
        {
            throw new TimeoutException($"Puzzle took more than {_TimeLimitMilliseconds / 1000} to solve.");
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

        // Try digits 1-boardSize
        for (int digit = 1; digit <= _constants.BoardSize; digit++)
        {
            if (_board.IsSafeCell(cellNumber, digit))
            {
                _board.PlaceDigit(cellNumber, digit);
                Stack<(int cellIndex, int digit)> humanTacticsStack = new();
                ApplyHumanTactics(humanTacticsStack);

                if (_board.HasDeadEnd())
                {
                    UndoHumanTacticsMoves(humanTacticsStack);
                    _board.RemoveDigit(cellNumber, digit);
                    continue;
                }

                if (_board.IsSolved() || Backtrack())
                {
                    return true;
                }

                UndoHumanTacticsMoves(humanTacticsStack);
                _board.RemoveDigit(cellNumber, digit);
            }
        }
        return false;
    }
}