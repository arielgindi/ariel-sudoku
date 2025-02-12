namespace ArielSudoku.Services;

public sealed partial class SudokuSolver
{

    private void ApplyHumanTactics()
    {
        bool isChanged;
        do
        {
            isChanged = false;

            while (ApplyNakedSingles())
            {
                isChanged = true;
            }

            if (ApplyHiddenSingles())
            {
                isChanged = true;
            }
        }
        while (isChanged);
    }

    /// <summary>
    /// Try to place digits in cells that have exactly one possibility
    /// </summary>
    /// <param name="humanTacticsStack"></param>
    /// <returns></returns>
    private bool ApplyNakedSingles()
    {
        bool isChanged = false;
        for (int cellIndex = 0; cellIndex < _constants.CellCount; cellIndex++)
        {
            if (_board[cellIndex] == 0 && _board.HasSingleOption(cellIndex))
            {
                int digit = _board.GetOnlyPossibleDigit(cellIndex);
                _board.PlaceDigit(cellIndex, digit);
                isChanged = true;

                _runtimeStats.NakedSinglesCount++;
            }
        }
        return isChanged;
    }

    /// <summary>
    /// Search for cells that there is only one
    /// possible solution it their same (row, col or box)
    /// For example: if digit 6 is the only possible digit in that row, it place it there
    /// </summary>
    /// <param name="humanTacticsStack"></param>
    /// <returns></returns>
    private bool ApplyHiddenSingles()
    {
        bool isChanged = false;

        for (int unitIndex = 0; unitIndex < _constants.BoardSize; unitIndex++)
        {
            isChanged |= _board.FindHiddenSinglesInUnit(_constants.CellsInRow[unitIndex]);
            isChanged |= _board.FindHiddenSinglesInUnit(_constants.CellsInCol[unitIndex]);
            isChanged |= _board.FindHiddenSinglesInUnit(_constants.CellsInBox[unitIndex]);
        }

        return isChanged;
    }
}