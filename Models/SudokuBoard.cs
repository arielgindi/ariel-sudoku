/// <summary>
/// A 9x9 Sudoku board stored as an array of chars.
/// </summary>
public sealed class SudokuBoard
{
    private const int BoardSize = 9;

    private readonly char[,] _cells;

    public SudokuBoard()
    {
        _cells = new char[BoardSize, BoardSize];
        // Initialize all cells to '0' to represent an empty cell.
        for (int row = 0; row < BoardSize; row++)
        {
            for (int col = 0; col < BoardSize; col++)
            {
                _cells[row, col] = '0';
            }
        }
    }

    public char this[int row, int col]
    {
        get => _cells[row, col];
        set => _cells[row, col] = value;
    }

    /// <summary>
    /// True if no cell is '0'
    /// </summary>
    public bool IsComplete()
    {
        for (int row = 0; row < BoardSize; row++)
        {
            for (int col = 0; col < BoardSize; col++)
            {
                if (_cells[row, col] == '0')
                {
                    return false;
                }
            }
        }
        return true;
    }
}
