namespace ArielSudoku.Exceptions;
/// <summary>
/// Thrown if unkown CLI input flag was provided
/// </summary>
/// <param name="message">Explanation about what caused this exception.</param>
public sealed class SudokuInvalidFlagException(string message) : SudokuException(message)
{
}
