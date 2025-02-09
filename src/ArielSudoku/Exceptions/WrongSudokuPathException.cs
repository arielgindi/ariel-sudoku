using ArielSudoku.Exceptions;
namespace ArielSudoku.Exceptions;

public sealed class WrongSudokuPathException(string message) : SudokuException(message)
{
}
