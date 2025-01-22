namespace ArielSudoku.Tests
{
    public class SudokuTests
    {
        [Fact]
        public void SolveSimpleSudoku1()
        {
            string puzzle = "100000027000304015500170683430962001900007256006810000040600030012043500058001000";
            string expectedSolution = "193586427867324915524179683435962871981437256276815349749658132612743598358291764";

            string actualSolution = SudokuEngine.SolveSudoku(puzzle);

            Assert.Equal(expectedSolution, actualSolution);
        }

        [Fact]
        public void SolveSimpleSudoku2()
        {
            string puzzle = "800000070006010053040600000000080400003000700020005038000000800004050061900002000";
            string expectedSolution = "831529674796814253542637189159783426483296715627145938365471892274958361918362547";

            string actualSolution = SudokuEngine.SolveSudoku(puzzle);

            Assert.Equal(expectedSolution, actualSolution);
        }
    }
}
