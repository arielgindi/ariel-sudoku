namespace ArielSudoku.Models;

using static ArielSudoku.SudokuHelpers;
using static ArielSudoku.Common.Constants;

/// <summary>
/// Remember row, colum and box usage of 
/// digits for faster and easier access.
/// </summary>
public sealed class SudokuUsage
{
    private readonly bool[,] _rowUsed;
    private readonly bool[,] _colUsed;
    private readonly bool[,] _boxUsed;

    private readonly SudokuBoard _board;

    public SudokuUsage(SudokuBoard board)
    {
        _board = board;

        _rowUsed = new bool[BoardSize, BoardSize + 1];
        _colUsed = new bool[BoardSize, BoardSize + 1];
        _boxUsed = new bool[BoardSize, BoardSize + 1];

        Initialize();
    }

    private void Initialize()
    {
        // save usage for each non empty cell
        for (int cellNumber = 0; cellNumber < CellCount; cellNumber++)
        {
            char cell = _board[cellNumber];
            if (cell != '0')
            {
                int digit = cell - '0';
                PlaceDigit(cellNumber, digit);
            }
        }
    }

    /// <summary>
    /// Checks if a given digit can be safely placed at cellNumber.
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
    /// Place a digit, if its '0' it will set it to empty automaticly
    /// </summary>
    public void PlaceDigit(int cellNumber, int digit)
    {
        _board[cellNumber] = (char)(digit + '0');
        MarkUsed(cellNumber, digit, true);  
    }

    public void RemoveDigit(int cellNumber, int digit)
    {
        _board[cellNumber] = '0';
        MarkUsed(cellNumber, digit, false);
    }

    private void MarkUsed(int cellNumber, int digit, bool used)
    {
        var (row, col, box) = GetCellCoordinates(cellNumber);
        _rowUsed[row, digit] = used;
        _colUsed[col, digit] = used;
        _boxUsed[box, digit] = used;
    }
}
