namespace ArielSudoku.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void SolveSimpleSudoku1()
        {
            string puzzle = "100000027000304015500170683430962001900007256006810000040600030012043500058001000";
            string expectedSolution = "193586427867324915524179683435962871981437256276815349749658132612743598358291764";

            string actualSolution = SudokuEngine.SolveSudoku(puzzle);

            Assert.Equal(expectedSolution, actualSolution);
        }
    }
}
