using static ArielSudoku.Common.SudokuHelpers;

namespace ArielSudoku.Tests;
public static class SudokuTestsBase
{
    public static void CheckPuzzleSolution(string givenPuzzle)
    {
        (string solvedPuzzle, _) = SudokuEngine.SolveSudoku(givenPuzzle);
        IsValidSolution(solvedPuzzle, givenPuzzle);
    }
}
