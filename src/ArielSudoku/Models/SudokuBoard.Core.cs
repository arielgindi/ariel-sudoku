namespace ArielSudoku.Models;

using static ArielSudoku.SudokuHelpers;
using static ArielSudoku.Common.Constants;

/// <summary>
/// A 9x9 Sudoku board stored as an array of chars.
/// </summary>
public sealed partial class SudokuBoard
{
    private readonly char[] _cells = new char[CellCount];

    public SudokuBoard(string input)
    {
        if (input == null)
            throw new ArgumentNullException(nameof(input));

        if (input.Length != CellCount)
            throw new ArgumentException($"Board must be exactly {CellCount} characters.");

        InitializeBoardFromString(input);
        SetUsageTracking();
    }

    private void InitializeBoardFromString(string input)
    {
        for (int cellNumber = 0; cellNumber < CellCount; cellNumber++)
        {
            char ch = input[cellNumber];
            if (ch == '.') ch = '0';

            // Ensure c is between '0' - '9'
            int row = CellCoordinates[cellNumber].row;
            int col = CellCoordinates[cellNumber].col;
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
    public override string ToString() => new(_cells);
}
