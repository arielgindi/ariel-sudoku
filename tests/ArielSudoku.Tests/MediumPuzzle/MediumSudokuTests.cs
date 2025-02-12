using static ArielSudoku.Tests.SudokuTestsBase;
namespace ArielSudoku.Tests.MediumPuzzle;
public class MediumSudokuTests
{
    [Fact]
    public void SolveMediumSudoku1()
    {
        string puzzle = "006790050350000010000030004000908007504000902800502000600020000020000078010087300";
        CheckPuzzleSolution(puzzle);
    }

    [Fact]
    public void SolveMediumSudoku2()
    {
        string puzzle = "082000560000000000000753000050000030400208001800000006509000302000000000036070140";
        CheckPuzzleSolution(puzzle);
    }


    [Fact]
    public void SolveMediumSudoku3()
    {
        string puzzle = "000700063000310507600000900016500000000279000000004390009000004205037000170002000";
        CheckPuzzleSolution(puzzle);
    }


    [Fact]
    public void SolveMediumSudoku4()
    {
        string puzzle = "003000560008030002700500940000600000005273600000009000064002007300060100017000200";
        CheckPuzzleSolution(puzzle);
    }


    [Fact]
    public void SolveMediumSudoku5()
    {
        string puzzle = "021000007040007600000082500800000370000351000063000002002160000006800010100000490";
        CheckPuzzleSolution(puzzle);
    }
}
