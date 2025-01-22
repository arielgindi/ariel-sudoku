namespace ArielSudoku.Tests;
public abstract class SudokuTestsBase
{
    protected void CheckPuzzleSolution(string puzzle, string expectedSolution)
    {
        string actualSolution = SudokuEngine.SolveSudoku(puzzle);
        Assert.Equal(expectedSolution, actualSolution);
    }
}
