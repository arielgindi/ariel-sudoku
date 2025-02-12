namespace ArielSudoku.Exceptions;
public sealed class MismatchSolutionException(string message) : SudokuException(message)
{
}