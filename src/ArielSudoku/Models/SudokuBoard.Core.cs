namespace ArielSudoku.Models;

using ArielSudoku.Common;
using ArielSudoku.Exceptions;

/// <summary>
/// A 9x9 Sudoku board stored as an array of chars.
/// </summary>
public sealed partial class SudokuBoard
{
    private readonly int[] _cells;
    public int PlaceDigitAmount { get; private set; } = 0;
    public int HasDeadEndAmount { get; private set; } = 0;
    public readonly Constants _constants ;

    public SudokuBoard(string puzzleString)
    {
        int length = puzzleString.Length;
        int boxSize = SudokuHelpers.CalculateBoxSize(length);

        _constants = ConstantsManager.GetOrCreateConstants(boxSize);

        // Make sure the puzzle string length matches the constants
        if (length != _constants.CellCount)
        {
            throw new InputInvalidLengthException(
                $"Puzzle length = {length}, expected = {_constants.CellCount}."
            );
        }

        // Allocate the array of cells for the puzzle
        _cells = new int[_constants.CellCount];
        InitializeBoardFromString(puzzleString);
        SetUsageTracking();
    }





    private void InitializeBoardFromString(string input)
    {
        for (int cellNumber = 0; cellNumber < _constants.CellCount; cellNumber++)
        {
            char ch = input[cellNumber];
            if (ch == '.') ch = '0';
            int digit = ch - '0';


            if (digit < 0 || _constants.BoardSize < digit)
            {
                // Ensure c is between '0' - '9'
                int row = _constants.CellCoordinates[cellNumber].row;
                int col = _constants.CellCoordinates[cellNumber].col;
                throw new SudokuInvalidBoardException(
                    $"Invalid board: '{digit}' at cell ({row},{col}). " +
                    $"Allowed characters are '0'-'{_constants.BoardSize}' or '.'."
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
        char[] result = new char[_constants.CellCount];
        for (int i = 0; i < _constants.CellCount; i++)
        {
            result[i] = (char)(_cells[i] + '0');
        }
        return new string(result);
    }
}