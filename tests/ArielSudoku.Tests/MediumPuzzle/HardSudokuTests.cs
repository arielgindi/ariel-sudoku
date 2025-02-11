using static ArielSudoku.Tests.SudokuTestsBase;
namespace ArielSudoku.Tests.MediumPuzzle;
public class HardSudokuTests : SudokuTestsBase
{
    [Fact]
    public void SolveHardSudoku1()
    {
        string puzzle = "869000312020000080070108040030000090700060004001902500000836000400070003000010000";
        CheckPuzzleSolution(puzzle);
    }

    [Fact]
    public void SolveHardSudoku2()
    {
        string puzzle = "080200400570000100002300000820090005000715000700020041000006700003000018007009050";
        CheckPuzzleSolution(puzzle);
    }

    [Fact]
    public void SolveHardSudoku3()
    {
        string puzzle = "040000000086100034001500260000305840000040000058902000095008300160009450000000010";
        CheckPuzzleSolution(puzzle);
    }


    [Fact]
    public void SolveHardSudoku4()
    {
        string puzzle = "200010007000207000050000020005020400001549200300708001070804030000000000630000085";
        CheckPuzzleSolution(puzzle);
    }


    [Fact]
    public void SolveHardSudoku5()
    {
        string puzzle = "020016005000000400306000207600075040000000000080230006207000604005000000100590070";
        CheckPuzzleSolution(puzzle);
    }
}
