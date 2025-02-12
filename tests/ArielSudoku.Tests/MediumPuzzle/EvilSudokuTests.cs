using static ArielSudoku.Tests.SudokuTestsBase;
namespace ArielSudoku.Tests.MediumPuzzle;
public class EvilSudokuTests
{
    [Fact]
    public void SolveEvilSudoku1()
    {
        string puzzle = "098000470001304200200000005000000000005293700060000040050060030100807004300409007";
        CheckPuzzleSolution(puzzle);
    }

    [Fact]
    public void SolveEvilSudoku2()
    {
        string puzzle = "060000030208000504700000002900605003600429001000080000000060000096000720540208096";
        CheckPuzzleSolution(puzzle);
    }

    [Fact]
    public void SolveEvilSudoku3()
    {
        string puzzle = "000206000601040020720003054000002005072000360500700000250300017030070209000401000";
        CheckPuzzleSolution(puzzle);
    }


    [Fact]
    public void SolveEvilSudoku4()
    {
        string puzzle = "006008000300050001402090005040002106000901000601400020500010803800040002000800900";
        CheckPuzzleSolution(puzzle);
    }

    [Fact]
    public void SolveEvilSudoku5()
    {
        string puzzle = "050908600800006007006020000009000070203000809010000400000030700900800004005604030";
        CheckPuzzleSolution(puzzle);
    }
}