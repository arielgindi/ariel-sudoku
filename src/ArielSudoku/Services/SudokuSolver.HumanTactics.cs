namespace ArielSudoku.Services;

public sealed partial class SudokuSolver
{
    // Try to place digits in cells that have exactly one possibility
    private void ApplyNakedSingles(Stack<(int cellIndex, int digit)>? humanTacticsStack)
    {
        bool changed;
        do
        {
            changed = false;
            for (int cellIndex = 0; cellIndex < CellCount; cellIndex++)
            {
                if (_board[cellIndex] == '0' && _board.HasSingleOption(cellIndex))
                {
                    int digit = _board.GetOnlyPossibleDigit(cellIndex);
                    _board.PlaceDigit(cellIndex, digit);
                    changed = true;

                    // Push if not null
                    humanTacticsStack?.Push((cellIndex, digit));
                }
            }
        }
        while (changed);
    }

    private void UndoHumanTacticsMoves(Stack<(int cellIndex, int digit)> humanTacticsStack)
    {
        while (humanTacticsStack.Count > 0)
        {
            (int cellIndex, int digit) = humanTacticsStack.Pop();
            _board.RemoveDigit(cellIndex, digit);
        }
    }
}
