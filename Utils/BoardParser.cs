public static class BoardParser
{
    /// <summary>
    /// Parses an 81-character string into a SudokuBoard.
    /// '0' is an empty cell.
    /// </summary>
    /// <param name="input">81-character puzzle string.</param>
    /// <returns>A SudokuBoard instance.</returns>
    public static SudokuBoard Parse(string input)
    {
        SudokuBoard board = new SudokuBoard();
        for (int i = 0; i < 81; i++)
        {
            int row = i / 9;
            int col = i % 9;
            board[row, col] = input[i];
        }

        return board;
    }

    /// <summary>
    /// Converts a SudokuBoard to an 81-character string.
    /// </summary>
    /// <param name="board">The SudokuBoard to convert.</param>
    /// <returns>An 81-character string of the board.</returns>
    public static string ConvertBoardToString(SudokuBoard board)
    {
        char[] chars = new char[81];
        for (int i = 0; i < 81; i++)
        {
            int row = i / 9;
            int col = i % 9;
            chars[i] = board[row, col];
        }
        return new string(chars);
    }
}
