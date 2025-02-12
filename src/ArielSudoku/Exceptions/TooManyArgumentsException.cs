namespace ArielSudoku.Exceptions;
/// <summary>
/// Thrown if there are too many arguments than expected
/// For example this input is invalid: '{puzzleString} --m --more' (expected 2)
/// </summary>
/// <param name="message">Explanation about what caused this exception.</param>
public sealed class TooManyArgumentsException(string message) : SudokuException(message)
{
}
