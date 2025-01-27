namespace ArielSudoku.Exceptions;
/// <summary>
/// Throw it when the sudoku digit has character that is not between '0'-'BoardSize'.
/// </summary>
/// <param name="message">Explanation about what caused this exception.</param>
public sealed class SudokuInvalidDigitException(string message) : SudokuException(message)
{
}
