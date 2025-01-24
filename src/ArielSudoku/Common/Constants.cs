namespace ArielSudoku.Common;
/// <summary>
/// Global static class that shares constants globaly
/// </summary>
public static class Constants
{
    // Size of each sub-box, for example 3x3 for a 9x9 Sudoku
    // 3
    public const int BoxSize = 3;

    // Length of each rows and columns, for example it will be 9 if BoxLen is 3
    // 9
    public const int BoardSize = BoxSize * BoxSize;

    // Total cells in the board, for example 81 for if BoxLen is 3
    // 81
    public const int CellCount = BoardSize * BoardSize;

    // Holds the (row, col, box) per each cell so no recomputing every time for fast access
    public static readonly (int row, int col, int box)[] CellCoordinates;

    /// <summary>
    /// Static constractor that only run one before access any thing constant inside this file
    /// It fills the CellCoordinantes once in the runtine lifetime, so less total calculations
    /// </summary>
    static Constants()
    {
        CellCoordinates = new (int, int, int)[CellCount];
        for (int cellIndex = 0; cellIndex < CellCount; cellIndex++)
        {
            int row = cellIndex / BoardSize;
            int col = cellIndex % BoardSize;
            int box = (row / BoxSize) * BoxSize + (col / BoxSize);
            CellCoordinates[cellIndex] = (row, col, box);
        }
    }

   
}
