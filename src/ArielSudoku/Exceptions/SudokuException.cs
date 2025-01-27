namespace ArielSudoku.Exceptions;
/// <summary>
/// Base exception for Sudoku errors. 
/// </summary>
/// <param name="message">Explanation about what caused this exception.</param>
public abstract class SudokuException(string message) : Exception(message)
{
}
