namespace ArielSudoku.Models;

using static ArielSudoku.Common.Constants;
using static ArielSudoku.SudokuHelpers;

/// <summary>
/// Remember row, colum and box usage of 
/// digits for faster and easier access.
/// </summary>
public sealed partial class SudokuBoard
{
    private readonly int[] _rowMask = new int[BoardSize + 1];
    private readonly int[] _colMask = new int[BoardSize + 1];
    private readonly int[] _boxMask = new int[BoardSize + 1];
    public List<int> EmptyCellsIndexes { get; } = [];

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
                continue;
            }
            EmptyCellsIndexes.Add(cellNumber);
        }
    }

    /// <summary>
    /// Checks if a given digit can be safely placed at cellNumber.
    /// </summary>
    public bool IsSafeCell(int cellNumber, int digit)
    {
        int row, col, box;
        (row, col, box) = GetCellCoordinates(cellNumber);

        if (IsBitSet(_rowMask[row], digit)) return false;
        if (IsBitSet(_colMask[col], digit)) return false;
        if (IsBitSet(_boxMask[box], digit)) return false;

        return true;
    }

    /// <summary>
    /// Place a digit, and update the bitmasks
    /// </summary>
    public void PlaceDigit(int cellNumber, int digit)
    {
        this[cellNumber] = (char)(digit + '0');

        int row, col, box;
        (row, col, box) = GetCellCoordinates(cellNumber);

        _rowMask[row] = SetBit(_rowMask[row], digit);
        _colMask[col] = SetBit(_colMask[col], digit);
        _boxMask[box] = SetBit(_boxMask[box], digit);
    }

    /// <summary>
    /// Remove a digit, and update the bitmasks
    /// </summary>
    public void RemoveDigit(int cellNumber, int digit)
    {
        this[cellNumber] = '0';

        int row, col, box;
        (row, col, box) = GetCellCoordinates(cellNumber);

        _rowMask[row] = ClearBit(_rowMask[row], digit);
        _colMask[col] = ClearBit(_colMask[col], digit);
        _boxMask[box] = ClearBit(_boxMask[box], digit);
    }
}
