namespace ArielSudoku;

public static class SudokuHelpers
{
    /// <summary>
    /// Returns the (row, col, box) coordinates of a given cell index.
    /// </summary>
    public static (int row, int col, int box) GetCellCoordinates(int cellNumber)
    {
        int row = cellNumber / BoardSize;
        int col = cellNumber % BoardSize;
        int box = GetBoxIndex(row, col);
        return (row, col, box);
    }

    /// <summary>
    /// Returns the index of the sub-box (0..8) for a given row/col
    /// </summary>
    public static int GetBoxIndex(int row, int col)
    {
        return (row / BoxLen) * BoxLen + (col / BoxLen);
    }

    /// <summary>
    /// Convert digit to his correct bitmask value
    /// For example: 1 becomes 0001, 2 becomes 0010 etc
    /// </summary>
    public static int GetMaskForDigit(int digit) => 1 << (digit);


    /// <summary>
    /// Check if a given digit is in bitmask
    /// For example: if mask=1010 and digit=2 it return true
    /// </summary>
    public static bool IsBitSet(int mask, int digit) => (mask & (1 << (digit))) != 0;


    /// <summary>
    /// Add a digit to the mask by setting its bit
    /// For example: If mask is 1000 and digit is 2 it return 1010
    /// </summary>
    public static int SetBit(int mask, int digit) => mask | GetMaskForDigit(digit);

    /// <summary>
    /// Removes a digit from the mask by clearing its bit
    /// For Example: If mask is 1100 and digit is 2 the result will be 1000
    /// </summary>
    public static int ClearBit(int mask, int digit) => mask & ~GetMaskForDigit(digit);

}