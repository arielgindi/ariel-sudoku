namespace ArielSudoku.Models;

using static ArielSudoku.Common.Constants;
using static ArielSudoku.SudokuHelpers;
/// <summary>
/// A 9x9 Sudoku board stored as an array of chars.
/// </summary>
public sealed class SudokuBoard
{
    private readonly char[] _cells;

    private bool[,] _rowUsed;
    private bool[,] _colUsed;
    private bool[,] _boxUsed;

    public SudokuBoard(string input)
    {
        if (input == null)
            throw new ArgumentNullException(nameof(input));

        if (input.Length != CellCount)
            throw new ArgumentException($"Board must be exactly {CellCount} characters.", nameof(input));

        _cells = new char[CellCount];
        for (int cellNumber = 0; cellNumber < CellCount; cellNumber++)
        {

            char ch = input[cellNumber];

            // Convert '.' to '0'
            if (ch == '.')
            {
                ch = '0';
            }

            // Ensure c is between '0' - '9'
            if (ch < '0' || ch > (char)('0' + BoardSize))
            {
                int row, col;
                (row, col, _) = GetCellCoordinates(cellNumber);

                throw new FormatException(
                    $"Invalid given board! " +
                    $"The character '{ch}' in cell ({row}, {col}). " +
                    $"Only '0'-'${BoardSize}' or '.' are allowed."
                );
            }

            _cells[cellNumber] = ch;
        }


        InitializeUsedCells();
    }

    public char this[int cellNumber]
    {
        get => _cells[cellNumber];
        set => _cells[cellNumber] = value;
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

        for (int cellNumber = 0; cellNumber < CellCount; cellNumber++)
        {
            char cell = _cells[cellNumber];
            if (cell != '0')
            {
                int digit = cell - '0';
                PlaceDigit(cellNumber, digit);
            }
        }
    }

    /// <summary>
    /// Places a digit on the board and updates the tracking arrays.
    /// Throws an error if that digit is already used in the row/col/box.
    /// </summary>
    public void PlaceDigit(int cellNumber, int digit)
    {
        int row, col, box;
        (row, col, box) = GetCellCoordinates(cellNumber);



        // if its invalid throw an error
        if (_rowUsed[row, digit] || _colUsed[col, digit] || _boxUsed[box, digit])
        {
            List<string> conflicts = new List<string>
            {
                _rowUsed[row, digit] ? $"row" : "",
                _colUsed[col, digit] ? $"col" : "",
                _boxUsed[box, digit] ? $"box" : ""
            }
            .Where(conflict => conflict.Length > 0)
            .ToList();

            throw new InvalidOperationException(
                $"Cannot place digit {digit} at cell ({row},{col}), " +
                $"digit {digit} is already used in the same {string.Join(" and ", conflicts)}."
            );
        }

        // Otherwise, no conflicts place the digit
        _cells[cellNumber] = (char)(digit + '0');
        _rowUsed[row, digit] = true;
        _colUsed[col, digit] = true;
        _boxUsed[box, digit] = true;
    }

    /// <summary>
    /// Removes a digit from the board and updates the tracking arrays.
    /// </summary>
    public void RemoveDigit(int cellNumber, int digit)
    {
        int row, col, box;
        (row, col, box) = GetCellCoordinates(cellNumber);

        _cells[cellNumber] = '0';
        _rowUsed[row, digit] = false;
        _colUsed[col, digit] = false;
        _boxUsed[box, digit] = false;
    }

    /// <summary>
    /// Checks if a digit can safely be placed at the same cellNumber.
    /// </summary>
    public bool IsSafeCell(int cellNumber, int digit)
    {
        int row, col, box;
        (row, col, box) = GetCellCoordinates(cellNumber);

        return !_rowUsed[row, digit]
               && !_colUsed[col, digit]
               && !_boxUsed[box, digit];
    }



    /// <summary>
    /// Print the board as an 81 characters string.
    /// </summary>
    public override string ToString()
    {
        return new string(_cells);
    }
}