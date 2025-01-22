namespace ArielSudoku.Models;

using static ArielSudoku.Common.Constants;
using static ArielSudoku.SudokuHelpers;

/// <summary>
/// Remember row, colum and box usage of 
/// digits for faster and easier access.
/// </summary>
public sealed partial class SudokuBoard
{
    private readonly bool[,] _rowUsed = new bool[BoardSize, BoardSize + 1];
    private readonly bool[,] _colUsed = new bool[BoardSize, BoardSize + 1]; 
    private readonly bool[,] _boxUsed = new bool[BoardSize, BoardSize + 1];

    private void SetUsageTracking()
    {
        // save usage for each non empty cell
        for (int cellNumber = 0; cellNumber < CellCount; cellNumber++)
        {
            char cell = this[cellNumber];
            if (cell != '0')
            {
                int digit = cell - '0';
                PlaceDigit(cellNumber, digit);
            }
        }
    }

    /// <summary>
    /// Checks if a given digit can be safely placed at cellNumber.
    /// </summary>
    public bool IsSafeCell(int cellNumber, int digit)
    {
        int row, col, box;
        (row, col, box) = GetCellCoordinates(cellNumber);

        return !_rowUsed[row, digit]
            && !_colUsed[col, digit]
            && !_boxUsed[box, digit];
    }

    /// <summary>
    /// Place a digit, if its '0' it will set it to empty automaticly
    /// </summary>
    public void PlaceDigit(int cellNumber, int digit)
    {
        this[cellNumber] = (char)(digit + '0');
        MarkUsed(cellNumber, digit, true);
    }

    public void RemoveDigit(int cellNumber, int digit)
    {
        this[cellNumber] = '0';
        MarkUsed(cellNumber, digit, false);
    }

    private void MarkUsed(int cellNumber, int digit, bool used)
    {
        int row, col, box;
        (row, col, box) = GetCellCoordinates(cellNumber);
        _rowUsed[row, digit] = used;
        _colUsed[col, digit] = used;
        _boxUsed[box, digit] = used;
    }
}
