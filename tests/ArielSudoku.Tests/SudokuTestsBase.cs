namespace ArielSudoku.Tests;
public abstract class SudokuTestsBase
{
    protected void CheckPuzzleSolution(string puzzle, string expectedSolution)
    {
        (string actualSolution, _) = SudokuEngine.SolveSudoku(puzzle);
        Assert.Equal(expectedSolution, actualSolution);
    }
}
