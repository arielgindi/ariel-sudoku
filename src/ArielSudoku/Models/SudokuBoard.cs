namespace ArielSudoku.Models;

using static ArielSudoku.SudokuHelpers;
using static ArielSudoku.Common.Constants;

/// <summary>
/// A 9x9 Sudoku board stored as an array of chars.
/// </summary>
public sealed partial class SudokuBoard
{
    private readonly char[] _cells;

    public SudokuBoard(string input)
    {
        if (input == null)
            throw new ArgumentNullException(nameof(input));

        if (input.Length != CellCount)
            throw new ArgumentException($"Board must be exactly {CellCount} characters.");

        _cells = new char[CellCount];
        InitializeBoardFromString(input);
        SetUsageTracking();
    }

    private void InitializeBoardFromString(string input)
    {
        for (int cellNumber = 0; cellNumber < CellCount; cellNumber++)
        {
            char ch = input[cellNumber];

            // Convert '.' to '0'
            if (ch == '.') ch = '0';

            // Ensure c is between '0' - '9'
            int row, col, box;
            (row, col, box) = GetCellCoordinates(cellNumber);
            if (ch < '0' || ch > (char)('0' + BoardSize))
            {
                throw new FormatException(
                    $"Invalid board: '{ch}' at cell ({row},{col}). " +
                    $"Allowed characters are '0'-'{BoardSize}' or '.'."
                );
            }

            _cells[cellNumber] = ch;
        }
    }

    public char this[int cellNumber]
    {
        get => _cells[cellNumber];
        set => _cells[cellNumber] = value;
    }

    /// <summary>
    /// Check if the board is complete (no '0' cells).
    /// </summary>
    public bool IsComplete() => _cells.All(cell => cell != '0');

    /// <summary>
    /// Print the board as an 81 characters string.
    /// </summary>
    public override string ToString() => new(_cells);
}
