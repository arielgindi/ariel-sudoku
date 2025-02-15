﻿using ArielSudoku.Exceptions;
using ArielSudoku.Models;
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
    /// For example: If mask is 1100 and digit is 2 the result will be 1000
    /// </summary>
    public static int ClearBit(int mask, int digit) => mask & ~GetMaskForDigit(digit);

    /// <summary>
    /// Counts the number of bits in integer (how many digits are possible).
    /// </summary>
    /// <param name="mask"/>
    /// <returns>The number of bits inside the mask</returns>
    public static int CountBits(int mask) => BitOperations.PopCount((uint)mask);


    /// <summary>
    /// Convert digitBit for example: 1000 into digit 3 (because its index 3)
    /// </summary>
    /// <param name="digitAsBit">Bitmask with exactly one bit!</param>
    public static int BitToDigit(int digitAsBit) => BitOperations.TrailingZeroCount((uint)digitAsBit);

    public static bool HasBitSet(int mask, int digit) => (mask & GetMaskForDigit(digit)) != 0;

    /// <summary>
    /// Format milliseconds time into the correct unit
    /// For example: 12345 ms, will be converted into '12.345 sec'
    /// </summary>
    /// <param name="timeInMs">Time in milliseconds</param>
    /// <returns>String displying time in correct unit</returns>
    public static string GetFormattedTime(double timeInMs)
    {
        if (timeInMs >= 1000)
            // 1000 ms or more => seconds
            return $"{timeInMs / 1000:F3} sec";
        if (timeInMs < 1)
            // Less than 1 ms => microseconds
            return $"{timeInMs * 1000:F3} μs";
        // Between 1 and 999.999 ms => milliseconds
        return $"{timeInMs:F3} ms";
    }


    public static int CalculateBoxSize(int puzzleLength)
    {
        return puzzleLength switch
        {
            1 => 1,    // 1×1
            16 => 2,   // 4×4
            81 => 3,   // 9×9
            256 => 4,  // 16×16
            625 => 5,  // 25×25
            _ => throw new InputInvalidLengthException(
                $"Puzzle length {puzzleLength} is not one of the recognized sizes (1,16,81,256,625)"
            )
        };
    }

    /// <summary>
    /// Validate if a puzzle was solved correctly
    /// Check if a given solved puzzle match the given puzzle 
    /// And if the solved puzzle is valid
    /// </summary>
    /// <param name="solvedPuzzle">String represent the puzzle solution</param>
    /// <param name="givenPuzzle">String represent the given unsolved puzzle</param>
    /// <exception cref="MismatchSolutionException">Thrown when solved puzzle doesnt match the given puzzle</exception>
    /// <exception cref="SudokuInvalidBoardException">Thrown when solved puzzle contain conflicts</exception>
    public static void IsValidSolution(string solvedPuzzle, string givenPuzzle)
    {
        if (solvedPuzzle.Length != givenPuzzle.Length) 
            throw new MismatchSolutionException("The given and solved puzzle aren't the same length");

        int boxSize = CalculateBoxSize(solvedPuzzle.Length);
        Constants constants = ConstantsManager.GetOrCreateConstants(boxSize);

        bool isEmptyCell(int cellIndex) => givenPuzzle[cellIndex] == '0' || givenPuzzle[cellIndex] == '.';

        // Check if there is not a match between the given and solved puzzle
        for (int cellIndex = 0; cellIndex < constants.CellCount; cellIndex++)
        {
            if (!isEmptyCell(cellIndex) && solvedPuzzle[cellIndex] != givenPuzzle[cellIndex])
            {
                throw new MismatchSolutionException(
                    $"The given and solved puzzle differentiate at cellIndex: {cellIndex}." +
                    " Expected {givenPuzzle[cellIndex]}");
            }
        }

        try
        {
            // If there are any conflicts, in the existing solvedPuzzle, SudokuBoard will automaticly throw "SudokuInvalidBoardException" 
            // For example if cell index 5 and 6 both contain the number 9, SudokuInvalidBoardException will be thrown
            SudokuEngine.SolveSudoku(solvedPuzzle);
        }
        catch (SudokuInvalidBoardException)
        {
            throw;
        }
    }
}