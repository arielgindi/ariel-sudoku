public static class BoardParser
{
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
}
