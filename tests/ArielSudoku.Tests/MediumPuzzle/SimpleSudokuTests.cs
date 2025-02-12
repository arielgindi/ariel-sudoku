using static ArielSudoku.Tests.SudokuTestsBase;
namespace ArielSudoku.Tests.MediumPuzzle;
public class EasySudokuTests 
{
    [Fact]
    public void SolveEasySudoku1()
    {
        string puzzle = "100000027000304015500170683430962001900007256006810000040600030012043500058001000";
        CheckPuzzleSolution(puzzle);
    }

    [Fact]
    public void SolveEasySudoku2()
    {
        string puzzle = "800000070006010053040600000000080400003000700020005038000000800004050061900002000";
        CheckPuzzleSolution(puzzle);
    }

    [Fact]
    public void SolveEasySudoku3()
    {
        string puzzle = "810006050006290000000000004900000320003050100028000005600000000000039400050400037";
        CheckPuzzleSolution(puzzle);
    }

    [Fact]
    public void SolveEasySudoku4()
    {
        string puzzle = "150369028000000000003000500040000060600701004007050800000090000020405030700000006";
        CheckPuzzleSolution(puzzle);
    }

    [Fact]
    public void SolveEasySudoku5()
    {
        string puzzle = "200305008000268000090104030000040000015806740080000050600702001900000003040050020";
        CheckPuzzleSolution(puzzle);
    }
}
