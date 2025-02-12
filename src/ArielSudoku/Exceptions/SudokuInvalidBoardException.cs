namespace ArielSudoku.Exceptions;
/// <summary>
/// Thrown it when the given sudoku puzzle is invalid from the start
/// </summary>
/// <param name="message">Explanation about what caused this exception.</param>
public sealed class SudokuInvalidBoardException(string message) : SudokuException(message)
{
}
