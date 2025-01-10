internal class SudokuSolver
{

    private SudokuBoard board; // Saved reference to the board
    private bool[,] rowUsed; // -
    private bool[,] colUsed; // |
    private bool[,] boxUsed;
    private const int BoardSize = 9;
    private const int BoxSize = 3;

    public void Solve(SudokuBoard sudokuBoard)
    {
        board = sudokuBoard; // Save the board reference
        InitilazeUsedCells();
        // Add logic to solve the board
    }

    private void InitilazeUsedCells()
    {
        rowUsed = new bool[BoardSize, BoardSize + 1];
        colUsed = new bool[BoardSize, BoardSize + 1];
        boxUsed = new bool[BoardSize, BoardSize + 1];

        for (int row = 0; row < BoardSize; row++)
        {
            for (int col = 0; col < BoardSize; col++)
            {
                char cell = board[row, col];
                if (cell != '0')
                {
                    int digit = cell - '0';

                    int boxIndex = GetBoxIndex(row, col);

                    // TODO: throw an error here if the row col or box is already true

                    rowUsed[row, digit] = true;
                    colUsed[row, digit] = true;
                    boxUsed[boxIndex, digit] = true;
                }
            }
        }
    }

    private bool IsSafeCell(int row, int col, int digit)
    {
        int boxIndex = GetBoxIndex(row, col);

        return !rowUsed[row, digit] && !colUsed[col, digit] && !boxUsed[boxIndex, digit];
    }

    private static int GetBoxIndex(int row, int col) => (row / BoxSize) * BoxSize + (col / BoxSize);
}