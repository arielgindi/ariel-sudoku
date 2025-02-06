namespace ArielSudoku.Models;

using static ArielSudoku.Common.Constants;
using ArielSudoku.Exceptions;

/// <summary>
/// A 9x9 Sudoku board stored as an array of chars.
/// </summary>
public sealed partial class SudokuBoard
{
    private readonly int[] _cells = new int[CellCount];
    public int PlaceDigitAmount { get; private set; } = 0;

    public SudokuBoard(string puzzleString)
    {
        ArgumentNullException.ThrowIfNull(puzzleString);

        if (puzzleString.Length != CellCount)
            throw new InputInvalidLengthException($"Input must be {CellCount} characters, but it is {puzzleString.Length}.");

        InitializeBoardFromString(puzzleString);
        SetUsageTracking();
    }

    private void InitializeBoardFromString(string input)
    {
        for (int cellNumber = 0; cellNumber < CellCount; cellNumber++)
        {
            char ch = input[cellNumber];
            if (ch == '.') ch = '0';
            int digit = ch - '0';


            if (digit < 0 || BoardSize < digit)
            {
                // Ensure c is between '0' - '9'
                int row = CellCoordinates[cellNumber].row;
                int col = CellCoordinates[cellNumber].col;
                throw new SudokuInvalidBoardException(
                    $"Invalid board: '{digit}' at cell ({row},{col}). " +
                    $"Allowed characters are '0'-'{BoardSize}' or '.'."
                );
            }

            _cells[cellNumber] = digit;
        }
    }
    public int this[int cellNumber]
    {
        get => _cells[cellNumber];
        set => _cells[cellNumber] = value;
    }
    public override string ToString()
    {
        char[] result = new char[CellCount];
        for (int i = 0; i < CellCount; i++)
        {
            result[i] = (char)(_cells[i] + '0');
        }
        return new string(result);
    }
}