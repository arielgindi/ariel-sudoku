namespace ArielSudoku.Models;

using static ArielSudoku.Common.Constants;
using static ArielSudoku.SudokuHelpers;
using System.Numerics;

/// <summary>
/// Remember row, colum and box usage of 
/// digits for faster and easier access.
/// </summary>
public sealed partial class SudokuBoard
{
    private readonly int[] _rowMask = new int[BoardSize];
    private readonly int[] _colMask = new int[BoardSize];
    private readonly int[] _boxMask = new int[BoardSize];
    public List<int> EmptyCellsIndexes { get; } = [];

    // Each cell has a bitmask of valid digits (bits 1..BoardSize).
    private readonly int[] _cellMasks = new int[CellCount];

    // Priority queue storing (cellIndex, numberOfValidDigits).
    private readonly PriorityQueue<int, int> _cellsQueue = new();

    // Bitmask that store as int an mask that is the same as if the cell
    // Was the same if cell could contain any digit
    // For example: if BoardSize == 4 than AllPossibiliteDigitsMask is 00011110
    private readonly int AllPossibleDigitsMask = ((1 << BoardSize) - 1) << 1;

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
        while (_cellsQueue.Count > 0)
        {
            // This create two new int: 'cellIndex' and 'possCount'.
            // 'cellIndex' is the cell's position,
            // and 'possCount' is how many digits were possible when passed earlier toqueued.
            _cellsQueue.TryDequeue(out int cellIndex, out int possCount);

            // If it's an empty cell and the number of possible digits per cell == 'possCount'
            // This mean nothing changed for this cell from last time return cell index.
            if (this[cellIndex] == '0' && CountBits(_cellMasks[cellIndex]) == possCount)
            {
                return cellIndex;
            }
        }
        // Did not find any empty cell with any possibilities
        return -1;
    }

    private void InitializePossibilities()
    {
        for (int cellIndex = 0; cellIndex < CellCount; cellIndex++)
        {
            if (this[cellIndex] == '0')
            {
                _cellMasks[cellIndex] = CalculateCellMask(cellIndex);
                _cellsQueue.Enqueue(cellIndex, CountBits(_cellMasks[cellIndex]));
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
        // Using an HashSet to remove duplication and O(1) access
        // For example: if there is a cell that was effected by row and by call, 
        // it will only add it once
        HashSet<int> affectedCells = [];
        (int rowIndex, int colIndex, int _) = CellCoordinates[cellIndex];

        // Add all cells in the same row
        int startRow = rowIndex * BoardSize;
        for (int i = 0; i < BoardSize; i++)
        {
            affectedCells.Add(startRow + i);
        }

        // Add all cells in the same col
        for (int i = 0; i < BoardSize; i++)
        {
            affectedCells.Add(i * BoardSize + colIndex);
        }

        // Add all cells in the same box
        int boxRow = (rowIndex / BoxSize) * BoxSize;
        int boxCol = (colIndex / BoxSize) * BoxSize;
        for (int r = 0; r < BoxSize; r++)
        {
            for (int c = 0; c < BoxSize; c++)
            {
                affectedCells.Add((boxRow + r) * BoardSize + (boxCol + c));
            }
        }

        // Recalculate each cell mask and if needed
        foreach (int affectedCellIndex in affectedCells)
        {
            if (this[affectedCellIndex] == '0')
            {
                _cellMasks[affectedCellIndex] = CalculateCellMask(affectedCellIndex);
                int possCount = CountBits(_cellMasks[affectedCellIndex]);
                if (possCount > 0)
                {
                    _cellsQueue.Enqueue(affectedCellIndex, possCount);
                }
            }
        }

    }
}
