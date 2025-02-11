using ArielSudoku.Common;
using ArielSudoku.Exceptions;
using ArielSudoku.Models;
using System.Diagnostics;
namespace ArielSudoku.Services;

public sealed partial class SudokuSolver
{
    public int GuessCount { get; private set; }

    private readonly SudokuBoard _board;
    private readonly Stopwatch _stopwatch;
    private const int _TimeLimitMilliseconds = 100000;
    private const int _CheckFrequency = 1000;
    private readonly Constants _constants;
    public SudokuSolver(SudokuBoard sudokuBoard)
    {
        _stopwatch = new Stopwatch();
        _board = sudokuBoard;
        _constants = _board._constants; 
    }

    /// <summary>
    /// Try to solve the board up to 1 sec
    /// </summary>
    /// <exception cref="UnsolvableSudokuException">Thrown if puzzle cannot be solved</exception>
    public void Solve()
    {
        _stopwatch.Start();

        Stack<(int cellIndex, int digit)> stack = new();
        ApplyHumanTactics(stack);

        if (_board.HasDeadEnd())
        {
            throw new UnsolvableSudokuException("Puzzle is unsolvable");
        }

        bool solved = Backtrack();
        if (!solved)
        {
            Console.WriteLine($"PlaceDigitAmount: {_board.PlaceDigitAmount}");
            Console.WriteLine($"Time it took: {SudokuHelpers.GetFormattedTime(_stopwatch.Elapsed.TotalMilliseconds)}");
            Console.WriteLine($"Backtracking steps: {GuessCount}");
            Console.WriteLine($"Dead end was found: {_board.HasDeadEndAmount}");
            throw new UnsolvableSudokuException("Puzzle is unsolvable");
        }

        //Console.WriteLine($"Dead end was found: {_board.HasDeadEndAmount}");
    }
}
