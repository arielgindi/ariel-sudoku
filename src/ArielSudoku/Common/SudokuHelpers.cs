namespace ArielSudoku;

public static class SudokuHelpers
{
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