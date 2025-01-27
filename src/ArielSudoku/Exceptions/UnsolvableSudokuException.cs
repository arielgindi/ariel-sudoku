namespace ArielSudoku.Exceptions;
/// <summary>
/// Thrown if there is no valid solution for a given puzzle
/// </summary>
/// <param name="message">Explanation about what caused this exception.</param>
public sealed class UnsolvableSudokuException(string message) : SudokuException(message)
{
}
