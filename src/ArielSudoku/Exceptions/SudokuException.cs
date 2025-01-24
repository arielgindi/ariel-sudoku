namespace ArielSudoku.Exceptions;
/// <summary>
/// 
/// </summary>
/// <param name="message"/>
public abstract class SudokuException(string message) : Exception(message)
{
}
