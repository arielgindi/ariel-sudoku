using System.Numerics;

namespace ArielSudoku.Common;

public static class SudokuHelpers
{
    /// <summary>
    /// Convert digit to his correct bitmask value
    /// For example: 1 becomes 0001, 2 becomes 0010 etc
    /// </summary>
    public static int GetMaskForDigit(int digit) => 1 << digit;
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

    /// <summary>
    /// Counts the number of bits in integer (how many digits are possible).
    /// </summary>
    /// <param name="mask"/>
    /// <returns>The number of bits inside the mask</returns>
    public static int CountBits(int mask) => BitOperations.PopCount((uint)mask);
}