using ArielSudoku.Models;

public static class SudokuEngine
{
    /// <summary>
    /// Solves a Sudoku puzzle in one call.
    /// </summary>
    /// <param name="puzzleString">
    /// 81-character puzzle string where '0' indicates an empty cell.
    /// </param>
    /// <returns>
    /// The solved Sudoku puzzle as an 81-character string.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="puzzleString"/> is not 81 characters long.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the puzzle is unsolvable.
    /// </exception>
    public static string SolveSudoku(string puzzleString)
    {
        // 1. Parse into a SudokuBoard.
        SudokuBoard board = new(puzzleString);

        // 2. Solve the board.
        SudokuSolver solver = new(board);
        solver.Solve();

        // 3. Convert the solved board back to string.
        string solvedPuzzle = board.ToString();
        return solvedPuzzle;
    }
}
