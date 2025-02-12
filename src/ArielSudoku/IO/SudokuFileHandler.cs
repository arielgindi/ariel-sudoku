using ArielSudoku.CLI;
using System.Diagnostics;

namespace ArielSudoku.IO;

public class SudokuFileHandler
{
    // Arrays containing puzzles from input and output
    private readonly string[] _inputPuzzles;
    private readonly string[] _solvedPuzzles;


    private double _totalProcessingTimeMs = 0;
    private int _totalBacktrackingCalls = 0;

    
    // Used for statistics
    public string OutputPath { get; private set; } = string.Empty;
    public int TotalPuzzles { get; private set; }
    public double MaxTimeMs { get; private set; }
    public int MaxTimePuzzleIndex { get; private set; }
    public int MaxBacktrackCalls { get; private set; }
    public int MaxBacktrackCallsIndex { get; private set; }
    public double AvgTimeMs => TotalPuzzles > 0 ? _totalProcessingTimeMs / TotalPuzzles : 0;
    public double AvgBacktrackingCalls => TotalPuzzles > 0 ? (double)_totalBacktrackingCalls / TotalPuzzles : 0;


    /// <summary>
    /// Solve from a given file all sudokus puzzles, 
    /// And keep the result inside a new output file
    /// </summary>
    /// <param name="filePath" />
    /// <exception cref="FileNotFoundException"/>
    /// <exception cref="Exception">Thrown if the file does not contain any puzzles</exception>
    public SudokuFileHandler(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("File not found: " + filePath);
        }

        _inputPuzzles = File.ReadAllLines(filePath);
        TotalPuzzles = _inputPuzzles.Length;
        _solvedPuzzles = new string[TotalPuzzles];

        if (TotalPuzzles == 0)
        {
            throw new Exception("File cannot contain zero puzzles");
        }

        ProcessSudokuFile();
    }

    /// <summary>
    /// Solve all puzzles from the file
    /// </summary>
    private void ProcessSudokuFile()
    {
        for (int puzzleIndex = 0; puzzleIndex < TotalPuzzles; puzzleIndex++)
        {
            ProcessSinglePuzzle(puzzleIndex);
        }

        CreateOutputFile();
    }

    /// <summary>
    /// Solve a single puzzle, and update the tracking status
    /// </summary>
    /// <param name="puzzleIndex">Index of puzzle to solve (in _solvedPuzzles array)</param>
    private void ProcessSinglePuzzle(int puzzleIndex)
    {
        string puzzleString = _inputPuzzles[puzzleIndex].Trim();

        Stopwatch watch = new();
        watch.Start();

        (string solvedPuzzle, RuntimeStatistics runtimeStats) = SudokuEngine.SolveSudoku(puzzleString);

        watch.Stop();
        UpdateStatistics(puzzleIndex, solvedPuzzle, runtimeStats, watch.Elapsed.TotalMilliseconds);
    }


    private void UpdateStatistics(int puzzleIndex, string solvedPuzzle, RuntimeStatistics runtimeStats, double processingTimeMs)
    {
        _solvedPuzzles[puzzleIndex] = solvedPuzzle;
        _totalProcessingTimeMs += processingTimeMs;
        // TODO: fix this
        //_totalBacktrackingCalls += backtrackCalls;

        if (processingTimeMs > MaxTimeMs)
        {
            MaxTimeMs = processingTimeMs;
            MaxTimePuzzleIndex = puzzleIndex + 1;
        }
        // TODO: fix this
        //if (backtrackCalls > MaxBacktrackCalls)
        //{
        //    MaxBacktrackCalls = backtrackCalls;
        //    MaxBacktrackCallsIndex = puzzleIndex + 1;
        //}
    }

    /// <summary>
    /// Write the solved puzzle into a new file
    /// </summary>
    /// <returns>New file path</returns>
    private void CreateOutputFile()
    {
        string outputFolder = Path.Combine(Environment.CurrentDirectory, "sudoku-answers");
        Directory.CreateDirectory(outputFolder);

        OutputPath = Path.Combine(outputFolder, "output.txt");
        File.WriteAllLines(OutputPath, _solvedPuzzles);
    }
}
