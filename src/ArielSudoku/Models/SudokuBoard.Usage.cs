namespace ArielSudoku.Models;

using ArielSudoku.Exceptions;
using System.Collections.Generic;
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

    // Track how many possibilities each cell has,
    // For example: if cell index 55 has 3 valid options, _possPerCell[55] = 3
    private readonly int[] _possPerCell = new int[CellCount];

    // Track indexes of cells that are currently empty
    private readonly List<int> _emptyCells = [];

    private void SetUsageTracking()
    {
        for (int cellIndex = 0; cellIndex < CellCount; cellIndex++)
        {
            int digit = this[cellIndex];
            if (digit != 0)
            {
                if (!IsSafeCell(cellIndex, digit))
                {
                    (int row, int col, int _) = CellCoordinates[cellIndex];
                    throw new SudokuInvalidDigitException(
                        $"Invalid sudoku board: the digit {digit} at ({row},{col}) conflicts with existing digits."
                    );
                }
                PlaceDigit(cellIndex, digit);
            }
        }

        // Find empty cells and update _cellMasks and _possPerCell
        for (int cellIndex = 0; cellIndex < CellCount; cellIndex++)
        {
            if (this[cellIndex] == 0)
            {
                _emptyCells.Add(cellIndex);
                _cellMasks[cellIndex] = CalculateCellMask(cellIndex);
                _possPerCell[cellIndex] = CountBits(_cellMasks[cellIndex]);
            }
        }
    }

    /// <summary>
    /// Checks if a given digit can be placed at cellIndex.
    /// </summary>
    public bool IsSafeCell(int cellIndex, int digit)
    {
        (int row, int col, int box) = CellCoordinates[cellIndex];
        int bit = GetMaskForDigit(digit);
        return ((_rowMask[row] | _colMask[col] | _boxMask[box]) & bit) == 0;
    }

    /// <summary>
    /// Place a digit, and update the usage
    /// </summary>
    public void PlaceDigit(int cellIndex, int digit)
    {
        PlaceDigitAmount++;
        this[cellIndex] = digit;
        (int row, int col, int box) = CellCoordinates[cellIndex];

        _rowMask[row] = SetBit(_rowMask[row], digit);
        _colMask[col] = SetBit(_colMask[col], digit);
        _boxMask[box] = SetBit(_boxMask[box], digit);

        _emptyCells.Remove(cellIndex);
        UpdateNeighborPossibilities(cellIndex);
    }

    /// <summary>
    /// Remove a digit, and update the usage
    /// </summary>
    public void RemoveDigit(int cellIndex, int digit)
    {
        this[cellIndex] = 0;
        (int row, int col, int box) = CellCoordinates[cellIndex];

        _rowMask[row] = ClearBit(_rowMask[row], digit);
        _colMask[col] = ClearBit(_colMask[col], digit);
        _boxMask[box] = ClearBit(_boxMask[box], digit);

        _emptyCells.Add(cellIndex);
        _cellMasks[cellIndex] = CalculateCellMask(cellIndex);
        _possPerCell[cellIndex] = CountBits(_cellMasks[cellIndex]);

        UpdateNeighborPossibilities(cellIndex);
    }

    /// <summary>
    /// Returns the index of the cell with the fewest valid digits, 
    /// or -1 if there is no empty cell with any possibilities.
    /// </summary>
    public int FindLeastOptionsCellIndex()
    {
        int bestCellIndex = -1;
        int bestCount = BoardSize + 1;

        for (int i = 0; i < _emptyCells.Count; i++)
        {
            int cellIndex = _emptyCells[i];
            if (this[cellIndex] == 0)
            {
                int count = _possPerCell[cellIndex];
                if (count < bestCount)
                {
                    bestCount = count;
                    bestCellIndex = cellIndex;
                    if (bestCount == 1) break;
                }
            }
        }

        return bestCellIndex;
    }

    public bool IsSolved()
    {
        // Now check if each row, column, and box has the correct mask
        for (int i = 0; i < BoardSize; i++)
        {
            if (_rowMask[i] != AllPossibleDigitsMask) return false;
            if (_colMask[i] != AllPossibleDigitsMask) return false;
            if (_boxMask[i] != AllPossibleDigitsMask) return false;
        }
        return true;
    }

    /// <summary>
    /// Check if the cell is empty and has exactly one valid digit
    /// </summary>
    public bool HasSingleOption(int cellIndex)
    {
        if (this[cellIndex] != 0) return false;
        return _possPerCell[cellIndex] == 1;
    }

    public int GetOnlyPossibleDigit(int cellIndex)
    {
        int mask = _cellMasks[cellIndex];
        for (int digit = 1; digit <= BoardSize; digit++)
        {
            int bit = 1 << digit;
            if ((mask & bit) != 0)
            {
                return digit;
            }
        }
        return 0;
    }
    /// <summary>
    /// Updates the possibility masks for neighbors of the given cell
    /// </summary>
    private void UpdateNeighborPossibilities(int cellIndex)
    {
        foreach (int neighborIndex in CellNeighbors[cellIndex])
        {
            if (this[neighborIndex] == 0)
            {
                _cellMasks[neighborIndex] = CalculateCellMask(neighborIndex);
                _possPerCell[neighborIndex] = CountBits(_cellMasks[neighborIndex]);
            }
        }
    }
    /// <summary>
    /// Get a mask containing all valid digits of a cell index
    /// </summary>
    private int CalculateCellMask(int cellIndex)
    {
        (int rowIndex, int colIndex, int boxIndex) = CellCoordinates[cellIndex];
        int combined = _rowMask[rowIndex] | _colMask[colIndex] | _boxMask[boxIndex];
        return AllPossibleDigitsMask & ~combined;
    }
    /// <summary>
    /// Check if any cell has no valid digits or if a digit doesn't have any possibilites
    /// This prevents continue to try solving a dead end path.
    /// </summary>

    public bool HasDeadEnd()
    {
        foreach (int emptyCellIndex in _emptyCells)
        {
            if (_possPerCell[emptyCellIndex] == 0)
            {
                return true;
            }
        }

        for (int digit = 1; digit <= BoardSize; digit++)
        {
            int bit = GetMaskForDigit(digit);
            if (HasNoSpotForDigit(CellsInRow, _rowMask, digit, bit)) return true;
            if (HasNoSpotForDigit(CellsInCol, _colMask, digit, bit)) return true;
            if (HasNoSpotForDigit(CellsInBox, _boxMask, digit, bit)) return true;
        }
        return false;
    }

    /// <summary>
    /// Check if a given digit can't be placed anywhere in a given unit
    /// </summary>
    private bool HasNoSpotForDigit(int[][] units, int[] usageMask, int digit, int bit)
    {
        for (int i = 0; i < BoardSize; i++)
        {
            if ((usageMask[i] & bit) == 0 && !CanDigitFit(units[i], digit))
            {
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// Check if at least one empty cell in the unit can hold this digit
    /// </summary>
    private bool CanDigitFit(int[] cellsInUnit, int digit)
    {
        for (int i = 0; i < cellsInUnit.Length; i++)
        {
            int cellIndex = cellsInUnit[i];
            if (_cells[cellIndex] == 0 && IsSafeCell(cellIndex, digit))
            {
                return true;
            }
        }
        return false;
    }
}
