namespace ArielSudoku.Exceptions;
/// <summary>
/// 
/// </summary>
/// <param name="message"/>
public sealed class UnsolvableSudokuException(string message) : SudokuException(message)
{
}
