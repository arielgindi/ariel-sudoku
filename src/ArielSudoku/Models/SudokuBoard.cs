﻿namespace ArielSudoku.Models;

using static ArielSudoku.SudokuHelpers;
using static ArielSudoku.Common.Constants;
/// <summary>
/// A 9x9 Sudoku board stored as an array of chars.
/// </summary>
public sealed class SudokuBoard
{
    private readonly char[,] _cells;

    private bool[,] _rowUsed;
    private bool[,] _colUsed;
    private bool[,] _boxUsed;

    public SudokuBoard(string input)
    {
        if (input == null)
            throw new ArgumentNullException(nameof(input));

        if (input.Length != CellCount)
            throw new ArgumentException($"Board must be exactly {CellCount} characters.", nameof(input));

        _cells = new char[BoardSize, BoardSize];
        for (int cellNumber = 0; cellNumber < CellCount; cellNumber++)
        {
            int row, col;
            (row, col, _) = GetCellCoordinates(cellNumber);
            char ch = input[cellNumber];

            // Convert '.' to '0' (denoting empty cell)
            if (ch == '.')
            {
                ch = '0';
            }

            // Ensure c is between '0' - '9'
            if (ch < '0' || ch > (char)('0' + BoardSize))
            {
                throw new FormatException(
                    $"Invalid given board! " +
                    $"The character '{ch}' in cell ({row}, {col}). " +
                    $"Only '0'-'${BoardSize}' or '.' are allowed."
                );
            }

            _cells[row, col] = ch;
        }


        InitializeUsedCells();
    }

    public char this[int row, int col]
    {
        get => _cells[row, col];
        set => _cells[row, col] = value;
    }

    public bool IsComplete() => _cells.Cast<char>().All(cell => cell != '0');

    /// <summary>
    /// Initializes row, column, and box usage based on the current board.
    /// </summary>
    public void InitializeUsedCells()
    {
        _rowUsed = new bool[BoardSize, BoardSize + 1];
        _colUsed = new bool[BoardSize, BoardSize + 1];
        _boxUsed = new bool[BoardSize, BoardSize + 1];

        for (int row = 0; row < BoardSize; row++)
        {
            for (int col = 0; col < BoardSize; col++)
            {
                char cell = _cells[row, col];
                if (cell != '0')
                {
                    int digit = cell - '0';
                    PlaceDigit(row, col, digit);
                }
            }
        }
    }

    /// <summary>
    /// Places a digit on the board and updates the tracking arrays.
    /// Throws an error if that digit is already used in the row/col/box.
    /// </summary>
    public void PlaceDigit(int row, int col, int digit)
    {
        int boxIndex = GetBoxIndex(row, col);


        // if its invalid throw an error
        if (_rowUsed[row, digit] || _colUsed[col, digit] || _boxUsed[boxIndex, digit])
        {
            List<string> conflicts = new List<string>
            {
                _rowUsed[row, digit] ? $"row" : "",
                _colUsed[col, digit] ? $"col" : "",
                _boxUsed[boxIndex, digit] ? $"box" : ""
            }
            .Where(conflict => conflict.Length > 0)
            .ToList();

            throw new InvalidOperationException(
                $"Cannot place digit {digit} at cell ({row},{col}), " +
                $"digit {digit} is already used in the same {string.Join(" and ", conflicts)}."
            );
        }

        // Otherwise, no conflicts place the digit
        _cells[row, col] = (char)(digit + '0');
        _rowUsed[row, digit] = true;
        _colUsed[col, digit] = true;
        _boxUsed[boxIndex, digit] = true;
    }

    /// <summary>
    /// Removes a digit from the board and updates the tracking arrays.
    /// </summary>
    public void RemoveDigit(int row, int col, int digit)
    {
        _cells[row, col] = '0';
        _rowUsed[row, digit] = false;
        _colUsed[col, digit] = false;
        _boxUsed[GetBoxIndex(row, col), digit] = false;
    }

    /// <summary>
    /// Checks if a digit can safely be placed at [row, col].
    /// </summary>
    public bool IsSafeCell(int row, int col, int digit)
    {
        int boxIndex = GetBoxIndex(row, col);
        return !_rowUsed[row, digit]
               && !_colUsed[col, digit]
               && !_boxUsed[boxIndex, digit];
    }



    /// <summary>
    /// Print the board as an 81 characters string.
    /// </summary>
    public override string ToString()
    {
        char[] chars = new char[CellCount];
        for (int i = 0; i < CellCount; i++)
        {
            int row = i / BoardSize;
            int col = i % BoardSize;
            chars[i] = _cells[row, col];
        }
        return new string(chars);
    }
}