internal class SudokuSolver
{
    private SudokuBoard board;
    private const int BoardSize = 9;

    public void Solve(SudokuBoard sudokuBoard)
    {
        board = sudokuBoard;
        Backtrack();
    }

    private bool Backtrack()
    {
        for (int row = 0; row < BoardSize; row++)
        {
            for (int col = 0; col < BoardSize; col++)
            {
                // Look for an empty cell
                if (board[row, col] == '0')
                {
                    // Try digits 1..9
                    for (int digit = 1; digit <= 9; digit++)
                    {
                        if (board.IsSafeCell(row, col, digit))
                        {
                            board.PlaceDigit(row, col, digit);

                            if (Backtrack())
                            {
                                return true;
                            }

                            board.RemoveDigit(row, col, digit);
                        }
                    }
                    // If no digit fits, we need to backtrack
                    return false;
                }
            }
        }
        // No empty cell => puzzle is solved
        return true;
    }
}
