namespace ArielSudoku.Exceptions;
/// <summary>
/// 
/// </summary>
/// <param name="message"/>
public sealed class SudokuInvalidFlagException(string message) : SudokuException(message)
{
}
