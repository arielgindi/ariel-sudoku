namespace ArielSudoku.Exceptions;
/// <summary>
/// Thrown if sudoku puzzle string isn't the expected length.
/// </summary>
/// <param name="message"/>
public sealed class InputInvalidLengthException(string message) : SudokuException(message)
{
}
