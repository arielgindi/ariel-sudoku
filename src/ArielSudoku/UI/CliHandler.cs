using ArielSudoku.Exceptions;
using System.Diagnostics;

namespace ArielSudoku.UI;

/// <summary>
/// Handles CLI input for the Sudoku solver.
/// </summary>
internal static class CliHandler
{
    private const string RED = "\x1B[31m";
    private const string GREEN = "\x1B[32m";
    private const string CYAN = "\x1B[36m";
    private const string BOLD = "\x1B[1m";
    private const string RESET = "\x1B[0m";
    private const string YELLOW = "\x1B[33m";

    /// <summary>
    /// Prints a short welcome message.
    /// </summary>
    private static void PrintWelcome()
    {
        Console.WriteLine($"{CYAN}================================={RESET}");
        Console.WriteLine($"{CYAN}{BOLD}          Gindi Sudoku{RESET}");
        Console.WriteLine($"{CYAN}================================={RESET}");
        Console.WriteLine($"Use flags: {BOLD}{CYAN}--m{RESET} or {BOLD}{CYAN}--more{RESET} for more info");
        Console.WriteLine($"Available Commands: {BOLD}{CYAN}exit{RESET}, {BOLD}{CYAN}clear{RESET}");
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
        PrintWelcome();

        while (true)
        {
            Console.Write($"{CYAN}>> {RESET}");


            try
            {
                string? userInput = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    continue;
                }

                // Clear the screen on 'clear'
                if (userInput.Equals("clear", StringComparison.OrdinalIgnoreCase))
                {
                    ClearScreenAndShowWelcome();
                    continue;
                }

                (string sudokuPuzzle, bool showMore) = ParseInput(userInput);

                if (sudokuPuzzle.Length != CellCount)
                {
                    throw new InputInvalidLengthException($"Input must be {CellCount} characters, but it is {sudokuPuzzle.Length}.");
                }

                Stopwatch stopwatch = Stopwatch.StartNew();
                (string solvedPuzzle, int backtrackCallAmount) = SudokuEngine.SolveSudoku(sudokuPuzzle);
                stopwatch.Stop();

                Console.WriteLine($"{GREEN}Result{RESET}: {YELLOW}{solvedPuzzle}{RESET} ({stopwatch.Elapsed.TotalSeconds:F3}s)");

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
    }

    /// <summary>
    /// Proccess the user input and return
    /// </summary>
    /// <param name="userInput">Plain text from the input</param>
    /// <returns>The sudoku string, and the flags if exist</returns>
    /// <exception cref="SudokuInvalidFlagException">Thrown if Flag doesnt match valid flags</exception>
    /// <exception cref="TooManyArgumentsException">Thrown if too mang flags are used</exception>
    static (string, bool) ParseInput(string userInput)
    {
        string[] parts = userInput?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? [];


        if (parts.Length == 0)
        {
            return ("", false);
        }

        if (parts.Length == 1)
        {
            return (parts[0], false);
        }

        if (parts.Length == 2)
        {
            if (parts[1] == "--m" || parts[1] == "--more")
                return (parts[0], true);
            throw new SudokuInvalidFlagException("Invalid flag. do you mean '--more'?");
        }

        throw new TooManyArgumentsException("Too many arguments. Provide a puzzle and optionally the '--more' flag.");
    }
}
