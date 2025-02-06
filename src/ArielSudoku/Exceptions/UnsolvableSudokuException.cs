namespace ArielSudoku.Exceptions;
/// <summary>
/// Thrown if there is no valid solution for a given puzzle.
/// Note that if the board has conflicts at the start, use <see cref="SudokuInvalidBoardException"/> instead
/// </summary>
/// <param name="message">Explanation about what caused this exception.</param>
public sealed class UnsolvableSudokuException(string message) : SudokuException(message)
{
}
