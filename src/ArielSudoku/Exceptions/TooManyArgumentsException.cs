namespace ArielSudoku.Exceptions;
/// <summary>
/// 
/// </summary>
/// <param name="message"/>
public sealed class TooManyArgumentsException(string message) : SudokuException(message)
{
}
