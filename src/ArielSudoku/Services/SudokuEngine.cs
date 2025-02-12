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
        SudokuBoard firstBoard = new(puzzleString, true);
        SudokuSolver firstSolver = new(firstBoard);

        SudokuBoard secondBoard = new(puzzleString, false);
        SudokuSolver secondSolver = new(secondBoard);

        Task task1 = Task.Run(firstSolver.Solve);
        Task task2 = Task.Run(secondSolver.Solve);


        int firstSolvedIndex = Task.WaitAny(task1, task2);

        if (task1.IsFaulted)
        {
            throw task1.Exception.GetBaseException();
        }

        if (task2.IsFaulted)
        {
            throw task2.Exception.GetBaseException();
        }


        SudokuBoard solvedBoard = firstSolvedIndex == 0 ? firstBoard : secondBoard;

        // Convert the solved board back to string.
        string solvedPuzzle = solvedBoard.ToString();
        RuntimeStatistics runtimeStats = solvedBoard.runtimeStats;
        return (solvedPuzzle, runtimeStats);
    }
}
