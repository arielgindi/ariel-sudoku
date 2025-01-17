namespace ArielSudoku;

public static class Constants
{
    // Size of each sub-box, for example 3x3 for a 9x9 Sudoku
    // 3
    public const int BoxLen = 3;

    // Length of each rows and columns, for example it will be 9 if BoxLen is 3
    // 9
    public const int BoardSize = BoxLen * BoxLen;

    // Total cells in the board, for example 81 for if BoxLen is 3
    // 81
    public const int CellCount = BoardSize * BoardSize;
}
