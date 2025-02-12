using ArielSudoku.Exceptions;
using ArielSudoku.Models;
using ArielSudoku.Services;
public static class SudokuEngine
{
    /// <summary>
    /// Solves a Sudoku puzzle in one call
    /// </summary>
    /// <param name="puzzleString">
    /// Sudoku puzzle string where '0' indicates an empty cell
    /// </param>
    /// <returns>
    /// The solved Sudoku puzzle as a string
    /// </returns>
    /// <exception cref="InputInvalidLengthException">
    /// Thrown if <paramref name="puzzleString"/> is not 81 characters long
    /// </exception>
    /// <exception cref="UnsolvableSudokuException">
    /// Thrown if the puzzle is unsolvable
    /// </exception>
    public static (string solvedPuzzle, RuntimeStatistics runtimeStats) SolveSudoku(string puzzleString)
    {
        // 1. Parse into a SudokuBoard.
        SudokuBoard board = new(puzzleString);

        // 2. Solve the board.
        SudokuSolver solver = new(board);
        solver.Solve();

        // 3. Convert the solved board back to string.
        string solvedPuzzle = board.ToString();
        RuntimeStatistics runtimeStats = board.runtimeStats;
        return (solvedPuzzle, runtimeStats);
    }
}
