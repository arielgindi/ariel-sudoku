namespace ArielSudoku.Exceptions;
/// <summary>
/// 
/// </summary>
/// <param name="message"/>
public sealed class InputInvalidSizeException(string message) : SudokuException(message)
{
}
