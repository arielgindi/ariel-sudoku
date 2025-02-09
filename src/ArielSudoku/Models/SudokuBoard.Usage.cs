namespace ArielSudoku.Models;

using ArielSudoku.Exceptions;
using System.Collections.Generic;
using static ArielSudoku.Common.SudokuHelpers;

/// <summary>
/// Remember row, colum and box usage of 
/// digits for faster and easier access.
/// </summary>
public sealed partial class SudokuBoard
{
    private int[] _rowMask;
    private int[] _colMask;
    private int[] _boxMask;

    // Each cell has a bitmask of valid digits (bits 1..BoardSize).
    private int[] _cellMasks;

    // Track how many possibilities each cell has,
    // For example: if cell index 55 has 3 valid options, _possPerCell[55] = 3
    private int[] _possPerCell;

    // Track indexes of cells that are currently empty
    private List<int> _emptyCells = [];

    private void SetUsageTracking()
    {
        _rowMask = new int[_constants.BoardSize];
        _colMask = new int[_constants.BoardSize];
        _boxMask = new int[_constants.BoardSize];

        _cellMasks = new int[_constants.CellCount];
        _possPerCell = new int[_constants.CellCount];
        _emptyCells = [];

        for (int cellIndex = 0; cellIndex < _constants.CellCount; cellIndex++)
        {
            int digit = this[cellIndex];
            if (digit != 0)
            {
                if (!IsSafeCell(cellIndex, digit))
                {
                    (int row, int col, int _) = _constants.CellCoordinates[cellIndex];
                    throw new SudokuInvalidBoardException(
                        $"Invalid sudoku board: the digit {digit} at ({row},{col}) conflicts with existing digits."
                    );
                }
                PlaceDigit(cellIndex, digit);
            }
        }

        // Find empty cells and update _cellMasks and _possPerCell
        for (int cellIndex = 0; cellIndex < _constants.CellCount; cellIndex++)
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
        (int row, int col, int box) = _constants.CellCoordinates[cellIndex];
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
        (int row, int col, int box) = _constants.CellCoordinates[cellIndex];

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
        (int row, int col, int box) = _constants.CellCoordinates[cellIndex];

        _rowMask[row] = ClearBit(_rowMask[row], digit);
        _colMask[col] = ClearBit(_colMask[col], digit);
        _boxMask[box] = ClearBit(_boxMask[box], digit);

        _emptyCells.Add(cellIndex);
        _cellMasks[cellIndex] = CalculateCellMask(cellIndex);
        _possPerCell[cellIndex] = CountBits(_cellMasks[cellIndex]);

        UpdateNeighborPossibilities(cellIndex);
    }

    /// <summary>
    /// Find the empty cell with the fewest valid digits, if two cells have the same count, 
    /// the one that his neighbors have the smallest total possibilities is chosen.
    /// Return -1 if no valid cell can be found
    /// </summary>
    public int FindLeastOptionsCellIndex()
    {
        int bestCellIndex = -1;
        int bestCount = _constants.BoardSize + 1;
        int bestNeighborSum = int.MaxValue;

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
                    bestNeighborSum = GetNeighborsPossibilitySum(cellIndex);

                    if (bestCount == 1)
                        break;
                }
                else if (count == bestCount)
                {
                    int neighborSum = GetNeighborsPossibilitySum(cellIndex);
                    if (neighborSum < bestNeighborSum)
                    {
                        bestNeighborSum = neighborSum;
                        bestCellIndex = cellIndex;
                    }
                }
            }
        }

        return bestCellIndex;
    }

    /// <summary>
    /// Calculate the total possibilities for all neighbors of a specific cell.
    /// </summary>
    private int GetNeighborsPossibilitySum(int cellIndex)
    {
        int sum = 0;
        foreach (int neighborIndex in _constants.CellNeighbors[cellIndex])
        {
            if (this[neighborIndex] == 0)
            {
                sum += _possPerCell[neighborIndex];
            }
        }
        return sum;
    }


    public bool IsSolved()
    {
        return _emptyCells.Count() == 0;
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
        for (int digit = 1; digit <= _constants.BoardSize; digit++)
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
        foreach (int neighborIndex in _constants.CellNeighbors[cellIndex])
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
        (int rowIndex, int colIndex, int boxIndex) = _constants.CellCoordinates[cellIndex];
        int combined = _rowMask[rowIndex] | _colMask[colIndex] | _boxMask[boxIndex];
        return _constants.AllPossibleDigitsMask & ~combined;
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

        for (int digit = 1; digit <= _constants.BoardSize; digit++)
        {
            int bit = GetMaskForDigit(digit);
            if (HasNoSpotForDigit(_constants.CellsInRow, _rowMask, digit, bit)) return true;
            if (HasNoSpotForDigit(_constants.CellsInCol, _colMask, digit, bit)) return true;
            if (HasNoSpotForDigit(_constants.CellsInBox, _boxMask, digit, bit)) return true;
        }
        return false;
    }

    /// <summary>
    /// Check if a given digit can't be placed anywhere in a given unit
    /// </summary>
    private bool HasNoSpotForDigit(int[][] units, int[] usageMask, int digit, int bit)
    {
        for (int i = 0; i < _constants.BoardSize; i++)
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


    /// <summary>
    /// Helper function for ApplyHiddenSingles
    /// Given a row, col or box to find a number that exist there only once
    /// For example: if digit 6 is the only possibile digit in that row, it place it there
    /// </summary>
    /// <param name="cellsInUnit">Unit of cells to check if there are hidden singles (row, col, or box)</param>
    /// <returns>True if a hidden single was found and placed, else false</returns>
    public bool FindHiddenSinglesInUnit(int[] cellsInUnit, Stack<(int cellIndex, int digit)>? humanTacticsStack)
    {
        int hiddenSinglesMask = FindHiddenSinglesMask(cellsInUnit);
        bool changed = false;

        while (hiddenSinglesMask != 0)
        {
            int digitBit = hiddenSinglesMask & -hiddenSinglesMask;
            int digit = BitToDigit(digitBit);

            int targetCell = LocateUniqueCellForDigit(cellsInUnit, digitBit);
            if (targetCell != -1)
            {
                PlaceDigit(targetCell, digit);
                humanTacticsStack?.Push((targetCell, digit));
                changed = true;
            }

            hiddenSinglesMask = ClearBit(hiddenSinglesMask, digit);
        }

        return changed;
    }


    /// <summary>
    /// can all empty cells in the unit
    /// (first storing all possible digits in seenDigits, after that excluding multiple possibilites cells)
    /// For exmaple: If digit 5 is the only digit in the cell, return it
    /// </summary>
    /// <param name="cellsInUnit"></param>
    /// <returns>A bitmask of hidden singles or zero</returns>
    private int FindHiddenSinglesMask(int[] cellsInUnit)
    {
        int seenDigits = 0;
        int notUniqueDigits = 0;

        for (int i = 0; i < cellsInUnit.Length; i++)
        {
            int cellIndex = cellsInUnit[i];
            if (this[cellIndex] != 0)
                continue;

            int cellMask = _cellMasks[cellIndex];
            notUniqueDigits |= (seenDigits & cellMask);
            seenDigits |= cellMask;
        }

        return seenDigits & ~notUniqueDigits;
    }

    private int LocateUniqueCellForDigit(int[] cellsInUnit, int digitBit)
    {
        int digit = BitToDigit(digitBit);

        for (int i = 0; i < cellsInUnit.Length; i++)
        {
            int cellIndex = cellsInUnit[i];
            if (this[cellIndex] == 0 && HasBitSet(_cellMasks[cellIndex], digit))
                // If only one cell has that bit set return cellIndex
                return cellIndex;
        }
        return -1;
    }
}