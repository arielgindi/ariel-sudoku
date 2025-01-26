namespace ArielSudoku.Models;

using ArielSudoku.Exceptions;
using static ArielSudoku.Common.Constants;
using static ArielSudoku.Common.SudokuHelpers;

/// <summary>
/// Remember row, colum and box usage of 
/// digits for faster and easier access.
/// </summary>
public sealed partial class SudokuBoard
{
    private readonly int[] _rowMask = new int[BoardSize];
    private readonly int[] _colMask = new int[BoardSize];
    private readonly int[] _boxMask = new int[BoardSize];

    // Each cell has a bitmask of valid digits (bits 1..BoardSize).
    private readonly int[] _cellMasks = new int[CellCount];

    // Buckets to store cell indexes by how many valid digits they currently have.
    // For example: if cell index 55 has 3 valid options, index 55 will be in list in _buckets[3] 
    private readonly List<int>[] _buckets = new List<int>[BoardSize + 1];

    // Tracks how many possibilities each cell has,
    // For example: if cell index 55 has 3 valid options, cell[55] = 3
    private readonly int[] _cellPossCount = new int[CellCount];

    private void SetUsageTracking()
    {
        // Init buckets before placing any digits
        for (int i = 0; i < _buckets.Length; i++)
        {
            _buckets[i] = [];
        }

        for (int cellIndex = 0; cellIndex < CellCount; cellIndex++)
        {
            char cell = this[cellIndex];
            if (cell != '0')
            {
                int digit = cell - '0';
                if (!IsSafeCell(cellIndex, digit))
                {
                    (int row, int col, int _) = CellCoordinates[cellIndex];
                    throw new SudokuInvalidDigitException(
                        $"Invalid sudoku board: the digit {digit} at ({row},{col}) " +
                        "conflicts with existing digits."
                    );
                }

                PlaceDigit(cellIndex, digit);
                continue;
            }
        }

        InitializePossibilities();
    }

    /// <summary>
    /// Checks if a given digit can be safely placed at cellNumber.
    /// </summary>
    public bool IsSafeCell(int cellNumber, int digit)
    {
        int row, col, box;
        (row, col, box) = CellCoordinates[cellNumber];

        int bit = GetMaskForDigit(digit);

        // For example if _rowMask=0100, _colMask=0000, _boxMask=0001 in binary, the OR command
        // combine them into 0101, than if digit (as bitmask) is 0100 than it returns true
        return ((_rowMask[row] | _colMask[col] | _boxMask[box]) & bit) == 0;
    }

    /// <summary>
    /// Place a digit, and update the bitmasks
    /// </summary>
    public void PlaceDigit(int cellIndex, int digit)
    {
        this[cellIndex] = (char)(digit + '0');

        int row, col, box;
        (row, col, box) = CellCoordinates[cellIndex];

        _rowMask[row] = SetBit(_rowMask[row], digit);
        _colMask[col] = SetBit(_colMask[col], digit);
        _boxMask[box] = SetBit(_boxMask[box], digit);
        UpdateAffectedCells(cellIndex);
    }

    /// <summary>
    /// Remove a digit, and update the bitmasks
    /// </summary>
    public void RemoveDigit(int cellNumber, int digit)
    {
        this[cellNumber] = '0';

        int row, col, box;
        (row, col, box) = CellCoordinates[cellNumber];

        _rowMask[row] = ClearBit(_rowMask[row], digit);
        _colMask[col] = ClearBit(_colMask[col], digit);
        _boxMask[box] = ClearBit(_boxMask[box], digit);
    }

    /// <summary>
    /// Returns the index of the cell with the fewest valid digits, 
    /// or -1 if there is no empty cell with any possibilities.
    /// </summary>
    public int FindLeastOptionsCellIndex()
    {
        // Searching from bucket[1] up to bucket[BoardSize].
        // Than try to find the first non empty bucket is the cell with the fewest options.
        for (int i = 1; i <= BoardSize; i++)
        {
            while (_buckets[i].Count > 0)
            {
                int lastIndex = _buckets[i].Count - 1;
                int cellIndex = _buckets[i][lastIndex];
                _buckets[i].RemoveAt(lastIndex);

                // If cell empty and has i possibilities, return it
                if (this[cellIndex] == '0' && CountBits(_cellMasks[cellIndex]) == i)
                {
                    return cellIndex;
                }
            }
        }
        // Did not find any empty cell with any possibilities
        return -1;
    }

    private void InitializePossibilities()
    {
        // For each empty cell, calculate its bitmask and put it in the correct bucket
        for (int cellIndex = 0; cellIndex < CellCount; cellIndex++)
        {
            if (this[cellIndex] == '0')
            {
                _cellMasks[cellIndex] = CalculateCellMask(cellIndex);
                int possCount = CountBits(_cellMasks[cellIndex]);
                _cellPossCount[cellIndex] = possCount;
                _buckets[possCount].Add(cellIndex);
            }
        }
    }

    private int CalculateCellMask(int cellIndex)
    {
        (int rowIndex, int colIndex, int boxIndex) = CellCoordinates[cellIndex];
        int combinedMask = _rowMask[rowIndex] | _colMask[colIndex] | _boxMask[boxIndex];
        return AllPossibleDigitsMask & ~combinedMask;
    }

    private void UpdateAffectedCells(int cellIndex)
    {
        // Loop through the precomputed peers of this cell
        foreach (int affectedCellIndex in CellNeighbors[cellIndex])
        {
            // If a peer is empty, recompute his possibilities
            if (this[affectedCellIndex] == '0')
            {
                _cellMasks[affectedCellIndex] = CalculateCellMask(affectedCellIndex);
                int newNumberOfPossibilities = CountBits(_cellMasks[affectedCellIndex]);

                // Remove from the old bucket and add to the new one
                int oldNumberOfPossibilities = _cellPossCount[affectedCellIndex];
                _buckets[oldNumberOfPossibilities].Remove(affectedCellIndex);

                _cellPossCount[affectedCellIndex] = newNumberOfPossibilities;

                if (newNumberOfPossibilities >= 0 && newNumberOfPossibilities <= BoardSize)
                {
                    _buckets[newNumberOfPossibilities].Add(affectedCellIndex);
                }
            }
        }
    }

    public bool IsSolved()
    {
        // Now check if each row, column, and box has the correct mask
        for (int i = 0; i < BoardSize; i++)
        {
            if (_rowMask[i] != AllPossibleDigitsMask)
            {
                return false;
            }
            if (_colMask[i] != AllPossibleDigitsMask)
            {
                return false;
            }
            if (_boxMask[i] != AllPossibleDigitsMask)
            {
                return false;
            }
        }

        return true;
    }

}
