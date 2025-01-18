namespace ArielSudoku;

public static class SudokuHelpers
{
    /// <summary>
    /// Returns the (row, col, box) coordinates of a given cell index.
    /// </summary>
    public static (int row, int col, int box) GetCellCoordinates(int cellNumber)
    {
        // ***REMOVED***
        int row = cellNumber / BoardSize;
        int col = cellNumber % BoardSize;
        int box = GetBoxIndex(row, col);
        return (row, col, box);
    }

    /// <summary>
    /// Returns the index of the sub-box (0..8) for a given row/col.
    /// </summary>
    public static int GetBoxIndex(int row, int col)
    {
        return (row / BoxLen) * BoxLen + (col / BoxLen);
    }
}
