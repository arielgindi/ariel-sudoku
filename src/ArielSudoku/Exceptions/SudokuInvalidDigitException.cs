namespace ArielSudoku.Exceptions;
/// <summary>
/// 
/// </summary>
/// <param name="message"/>
public sealed class SudokuInvalidDigitException(string message) : SudokuException(message)
{
}
