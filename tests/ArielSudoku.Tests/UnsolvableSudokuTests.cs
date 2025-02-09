using ArielSudoku.Exceptions;

namespace ArielSudoku.Tests;
/// <summary>
/// Unsolvable sudoku expect <exception cref="UnsolvableSudokuException" /> 
/// if he will not get that the test will be failed
/// </summary>
public class UnsolvableSudokuTests
{

    [Fact]
    public void UnsolvableSudoku1()
    {
        string puzzle = "005300000800000020070010500400005300010070006003200009060500040000000700000003002";
        Assert.Throws<UnsolvableSudokuException>(() => SudokuEngine.SolveSudoku(puzzle));
    }

    [Fact]
    public void UnsolvableSudoku2()
    {
        string puzzle = "000030000060000400007050800000406000000900000050010300400000020000300000000000000";
        Assert.Throws<UnsolvableSudokuException>(() => SudokuEngine.SolveSudoku(puzzle));
    }

    [Fact]
    public void UnsolvableSudoku3()
    {
        string puzzle = "000005080000601043000000000010500000000106000300000005530000061000000004000000000";
        Assert.Throws<UnsolvableSudokuException>(() => SudokuEngine.SolveSudoku(puzzle));
    }
}
