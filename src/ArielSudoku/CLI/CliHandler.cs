﻿using ArielSudoku.Common;
using ArielSudoku.Exceptions;
using ArielSudoku.IO;
using System.Diagnostics;
using System.Text;

namespace ArielSudoku.CLI;

/// <summary>
/// Handles CLI input for the Sudoku solver
/// </summary>
internal static class CliHandler
{
    // Normal colors
    private const string RED = "\x1B[31m";
    private const string GREEN = "\x1B[32m";
    private const string CYAN = "\x1B[36m";
    private const string YELLOW = "\x1B[33m";

    // Used to print the print puzzle
    private const string LIGHT_GRAY = "\x1B[38;2;153;153;153m";
    private const string HEAVY_BLUE = "\x1B[34m";
    private const string LIGHT_BLUE = "\x1B[94m";

    private const string BOLD = "\x1B[1m";
    private const string RESET = "\x1B[0m";

    /// <summary>
    /// Print a short welcome message
    /// </summary>
    private static bool _shouldExit = false;


   
    private static int _totalPuzzlesProcessed = 0; 

   
    private static DateTime _appStartTime = DateTime.Now;


    private static void PrintWelcome()
    {
        Console.WriteLine($"{CYAN}================================={RESET}");
        Console.WriteLine($"{CYAN}{BOLD}          Gindi Sudoku{RESET}");
        Console.WriteLine($"{CYAN}================================={RESET}");
        Console.WriteLine($"Use flags: {BOLD}{CYAN}-m{RESET} or {BOLD}{CYAN}--more{RESET} for more info");
        Console.WriteLine($"Available Commands: {BOLD}{CYAN}exit{RESET}, {BOLD}{CYAN}clear{RESET}, {BOLD}{CYAN}read{RESET}");
        Console.WriteLine();
    }

    /// <summary>
    /// Clear the console and print welcome message again
    /// </summary>
    private static void ClearScreenAndShowWelcome()
    {
        Console.Clear();
        PrintWelcome();
    }

    /// <summary>
    /// Main loop: wait for input, check length, and print the result.
    /// </summary>
    /// <exception cref="FormatException">Thrown when sudoku board string is not in his correct size</exception>
    public static void Run()
    {
        Console.CancelKeyPress += OnCancelKeyPress;

        PrintWelcome();

        while (!_shouldExit)
        {
            Console.Write($"{CYAN}>> {RESET}");

            string? userInput = Console.ReadLine();

            // If user pressed Ctrl+C or userInput is null, break
            if (_shouldExit || userInput == null)
            {
                break;
            }

            userInput = userInput.Trim();

            try
            {
                (string givenPuzzle, bool showMore) = ParseInput(userInput ?? "");

                if (string.IsNullOrWhiteSpace(givenPuzzle))
                {
                    continue;
                }

                _totalPuzzlesProcessed++;

                Stopwatch stopwatch = Stopwatch.StartNew();
                (string solvedPuzzle, int backtrackCallAmount) = SudokuEngine.SolveSudoku(givenPuzzle);
                stopwatch.Stop();

                Console.WriteLine($"{GREEN}Result{RESET}: {YELLOW}{solvedPuzzle}{RESET} ({SudokuHelpers.GetFormattedTime(stopwatch.Elapsed.TotalMilliseconds)})");
                PrintPuzzle(solvedPuzzle, givenPuzzle);

                if (showMore)
                {
                    Console.WriteLine($"{GREEN}backtraking steps: {RESET}{backtrackCallAmount}{RESET}{CYAN}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{RED}Error: {ex.Message}{RESET}");
            }
        }

        PrintExitMessage();
    }


    /// <summary>
    /// Print exit message
    /// </summary>
    private static void PrintExitMessage()
    {
        TimeSpan totalTime = DateTime.Now - _appStartTime;
        Log();
        Log($"{CYAN}============== {CYAN}{BOLD}Runtime summary{RESET}{CYAN} =============={RESET}");
        Log($"{GREEN}Total time spent        {RESET}: {SudokuHelpers.GetFormattedTime(totalTime.TotalMilliseconds)}");
        Log($"{GREEN}Total puzzles proccesed {RESET}: {_totalPuzzlesProcessed}");
        Log($"{CYAN}============================================={RESET}");
        Log($"{CYAN}{BOLD}Thanks for trying Gindi Sudoku!{RESET}");
        Log($"{CYAN}{BOLD}Exiting...{RESET}");
    }


    /// <summary>
    /// Handles Ctrl+C so we can exit cleanly
    /// </summary>
    private static void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
    {
        e.Cancel = true;
        _shouldExit = true;
    }

    /// <summary>
    /// Proccess the user input and return
    /// </summary>
    /// <param name="userInput">Plain text from the input</param>
    /// <returns>The sudoku string, and the flags if exist</returns>
    /// <exception cref="SudokuInvalidFlagException">Thrown if Flag doesnt match valid flags</exception>
    /// <exception cref="TooManyArgumentsException">Thrown if too mang flags are used</exception>
    static (string puzzleString, bool showMore) ParseInput(string userInput)
    {
        string[] parts = userInput?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? [];

        if (parts.Length == 0)
        {
            return ("", false);
        }

        if (parts[0].Equals("exit", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Goodbye!");
            _shouldExit = true;
            return ("", false);
        }

        // Clear the screen
        if (parts[0].Equals("clear", StringComparison.OrdinalIgnoreCase))
        {
            ClearScreenAndShowWelcome();
            return ("", false);
        }

        if (parts[0] == "read")
        {
            if (parts.Length < 2)
                throw new MissingFilePathException(
                    "Missing file path! after 'read' command you have to write your'e file path " +
                    "for example 'read SudokuPuzzles.txt'");

            ProcessFileFromCLI(parts[1]);
            return ("", false);
        }

        if (parts.Length == 1)
        {
            return (parts[0], false);
        }

        if (parts.Length == 2)
        {
            if (parts[1] == "-m" || parts[1] == "--more")
                return (parts[0], true);
            throw new SudokuInvalidFlagException("Invalid flag. do you mean '--more'?");
        }

        throw new TooManyArgumentsException("Too many arguments. Provide a puzzle and optionally the '--more' flag.");
    }

    private static void ProcessFileFromCLI(string filePath)
    {
        Stopwatch totalStopwatch = new();
        totalStopwatch.Start();

        SudokuFileHandler fileHandler = new(filePath);
        _totalPuzzlesProcessed += fileHandler.TotalPuzzles;
        totalStopwatch.Stop();
        Log();
        Log($"{CYAN}========== {CYAN}{BOLD}File processing summary{RESET}{CYAN} =========={RESET}");
        Log($"{GREEN}File input path        :{RESET} {filePath}");
        Log($"{GREEN}Output file            :{RESET} {fileHandler.OutputPath}");
        Log($"{GREEN}Number of puzzles      :{RESET} {fileHandler.TotalPuzzles}");
        Log($"{GREEN}Avg time to solve      :{RESET} {SudokuHelpers.GetFormattedTime(fileHandler.AvgTimeMs)}");
        Log($"{GREEN}Longest time to solve  :{RESET} {SudokuHelpers.GetFormattedTime(fileHandler.MaxTimeMs)} (puzzle #{fileHandler.MaxTimePuzzleIndex})");
        Log($"{GREEN}Max backtracking calls :{RESET} {fileHandler.MaxBacktrackCalls} (puzzle #{fileHandler.MaxBacktrackCallsIndex})");
        Log($"{GREEN}Avg backtraking calls  :{RESET} {fileHandler.AvgBacktrackingCalls:F3}");
        Log($"{GREEN}Total time taken       :{RESET} {SudokuHelpers.GetFormattedTime(totalStopwatch.Elapsed.TotalMilliseconds)}");
        Log($"{CYAN}============================================={RESET}");
        Log();
    }

    private static void Log(string? message = "") => Console.WriteLine(message);

    public static void PrintPuzzle(string solvedPuzzle, string givenPuzzle)
    {
        int boxSize = SudokuHelpers.CalculateBoxSize(solvedPuzzle.Length);
        Constants constants = ConstantsManager.GetOrCreateConstants(boxSize);
        int width = 2 * constants.BoardSize + 2 * (boxSize - 1) + 1;

        StringBuilder formattedPuzzle = new();

        void AppendRow(char left, char between, char right)
        {
            formattedPuzzle.AppendLine($"{LIGHT_GRAY}{left}{new string(between, width)}{right}{RESET}");
        }

        void AppendCell(int index)
        {
            if (solvedPuzzle[index] == givenPuzzle[index])
            {
                formattedPuzzle.Append(HEAVY_BLUE + givenPuzzle[index] + RESET);
            }
            else
            {
                formattedPuzzle.Append(LIGHT_BLUE + solvedPuzzle[index] + RESET);
            }
        }

        // Top row
        AppendRow('╔', '═', '╗');
        for (int row = 0; row < constants.BoardSize; row++)
        {
            // Mid row
            if (row > 0 && row % boxSize == 0) AppendRow('╠', '═', '╣');

            // Start each row with a left border
            formattedPuzzle.Append(LIGHT_GRAY + '║' + RESET);

            for (int col = 0; col < constants.BoardSize; col++)
            {
                formattedPuzzle.Append(' ');

                int index = row * constants.BoardSize + col;
                AppendCell(index);

                if (col < constants.BoardSize - 1 && (col + 1) % boxSize == 0)
                {
                    // Add seperator
                    formattedPuzzle.Append(LIGHT_GRAY + " ║" + RESET);
                }
            }

            // End the row with a space and right border
            formattedPuzzle.Append(' ');
            formattedPuzzle.AppendLine(LIGHT_GRAY + '║' + RESET);
        }

        // Lowest row
        AppendRow('╚', '═', '╝');

        Console.WriteLine(formattedPuzzle);
    }
}
