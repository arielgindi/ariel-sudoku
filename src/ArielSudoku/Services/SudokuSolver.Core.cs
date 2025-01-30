using ArielSudoku.Exceptions;
using ArielSudoku.Models;
using System.Diagnostics;
namespace ArielSudoku.Services;

public sealed partial class SudokuSolver
{
    public int BacktrackCallAmount { get; private set; }
    private readonly SudokuBoard _board;
    private readonly Stopwatch _stopwatch;
    private const int _TimeLimitMilliseconds = 1000;
    private const int _CheckFrequency = 1000;

    public SudokuSolver(SudokuBoard sudokuBoard)
    {
        _board = sudokuBoard;
        _stopwatch = new Stopwatch();
    }

    /// <summary>
    /// Try to solve the board up to 1 sec
    /// </summary>
    /// <exception cref="UnsolvableSudokuException">Thrown if puzzle cannot be solve</exception>
    public void Solve()
    {
        _stopwatch.Start();

        ApplyHumanTactics(null);

        bool solved = Backtrack();
        if (!solved)
        {
            throw new UnsolvableSudokuException("Puzzle is unsolvable");
        }
    }
}
