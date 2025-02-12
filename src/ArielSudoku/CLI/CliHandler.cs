using ArielSudoku.Common;
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

    // Used to print the puzzle
    private const string LIGHT_GRAY = "\x1B[38;2;153;153;153m";
    private const string PURPLE = "\u001b[1m\u001b[38;2;155;70;235m";
    private const string ORANGE = "\u001b[38;5;208m";

    private const string BOLD = "\x1B[1m";
    private const string RESET = "\x1B[0m";
    private static bool _printLetters = false;

    private static bool _shouldExit = false;
    private static bool _showMore = false;
    private static int _totalPuzzlesProcessed = 0;
    private static readonly DateTime _appStartTime = DateTime.Now;
    private static readonly string[] availableCommands = ["exit", "clear", "read", "help"];
    private static readonly string[] availableFlags = ["-m", "--more", "--letters"];


    /// <summary>
    /// Print a short welcome message
    /// </summary>
    private static void PrintWelcome()
    {
        string commands = string.Join(", ", availableCommands.Select(cmd => $"{BOLD}{CYAN}{cmd}{RESET}"));
        string flags = string.Join(", ", availableFlags.Select(cmd => $"{BOLD}{CYAN}{cmd}{RESET}"));
        Log();
        Log($"{CYAN}================================={RESET}");
        Log($"{CYAN}{BOLD}          Gindi Sudoku{RESET}");
        Log($"{CYAN}================================={RESET}");
        Log($"Available flags: {flags}");
        Log($"Available Commands: {commands}");
        Log();
    }

    /// <summary>
    /// Shows a help message with quick examples.
    /// </summary>
    private static void PrintHelp()
    {
        Log($"{CYAN}============ {BOLD}CLI Usage{RESET}{CYAN} ============{RESET}");
        Log($"{CYAN}{BOLD}puzzle{RESET} [{CYAN}{BOLD}flag/s{RESET}] : Solve one puzzle");
        Log($"                For example:  '{GREEN}0001020004000000 --more --letters{RESET}'");
        Log($"{CYAN}{BOLD}read{RESET} <{CYAN}{BOLD}path{RESET}>   : Solve puzzles from file");
        Log($"                For example:  '{GREEN}read C:\\data\\49158.txt{RESET}'");
        Log($"{CYAN}{BOLD}clear{RESET}         : Clear the screen");
        Log($"{CYAN}{BOLD}help{RESET}          : Show help info");
        Log($"{CYAN}{BOLD}exit{RESET}          : Quit the program");
        Log($"{CYAN}============================================{RESET}");
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
    /// <exception cref="FormatException">Thrown when sudoku board string is not the correct size</exception>
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
                string givenPuzzle = ParseInput(userInput ?? "");

                if (string.IsNullOrWhiteSpace(givenPuzzle))
                {
                    continue;
                }

                _totalPuzzlesProcessed++;

                Stopwatch stopwatch = Stopwatch.StartNew();
                (string solvedPuzzle, RuntimeStatistics runtimeStats) = SudokuEngine.SolveSudoku(givenPuzzle);
                stopwatch.Stop();

                Console.WriteLine($"{GREEN}Result{RESET}: {YELLOW}{solvedPuzzle}{RESET} ({SudokuHelpers.GetFormattedTime(stopwatch.Elapsed.TotalMilliseconds)})");
                PrintPuzzle(solvedPuzzle, givenPuzzle);

                if (_showMore)
                {
                    Console.WriteLine($"{GREEN}Guesses count               : {RESET}{runtimeStats.GuessCount}{RESET}{CYAN}");
                    Console.WriteLine($"{GREEN}Found dead end count        : {RESET}{runtimeStats.FoundDeadEndCount}{RESET}{CYAN}");
                    Console.WriteLine($"{GREEN}Placed naked sinlges count  : {RESET}{runtimeStats.NakedSinglesCount}{RESET}{CYAN}");
                    Console.WriteLine($"{GREEN}Placed hidden singles count : {RESET}{runtimeStats.HiddenSinlgesCount}{RESET}{CYAN}");
                    Console.WriteLine($"{GREEN}Placed digit count          : {RESET}{runtimeStats.PlaceDigitCount}{RESET}{CYAN}");
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
        Log($"{GREEN}Total puzzles processed {RESET}: {_totalPuzzlesProcessed}");
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
    /// Process the user input and return
    /// </summary>
    /// <param name="userInput">Plain text from the input</param>
    /// <returns>The sudoku string, and the flags if exist</returns>
    /// <exception cref="SudokuInvalidFlagException">Thrown if Flag doesnt match valid flags</exception>
    /// <exception cref="TooManyArgumentsException">Thrown if too mang flags are used</exception>
    static string ParseInput(string userInput)
    {
        // Set flag to thier defualt value
        _shouldExit = false;
        _printLetters = false;
        _showMore = false;
        string[] parts = userInput?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? [];

        if (parts.Length == 0)
        {
            return "";
        }

        if (parts[0].Equals("exit", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Goodbye!");
            _shouldExit = true;
            return "";
        }

        // Clear the screen
        if (parts[0].Equals("clear", StringComparison.OrdinalIgnoreCase))
        {
            ClearScreenAndShowWelcome();
            return "";
        }

        if (parts[0].Equals("help", StringComparison.OrdinalIgnoreCase))
        {
            PrintHelp();
            return "";
        }

        if (parts[0] == "read")
        {
            if (parts.Length < 2)
                throw new MissingFilePathException(
                    "Missing file path! after 'read' command you have to write your file path " +
                    "for example 'read SudokuPuzzles.txt'");

            ProcessFileFromCLI(parts[1]);
            return "";
        }

        if (parts.Length == 1)
        {
            return parts[0];
        }

        if (parts.Length == 2 || parts.Length == 3)
        {
            for (int i = 1; i < parts.Length; i++)
            {
                if (parts[i] == "-m" || parts[i] == "--more")
                {
                    if (_showMore)
                    {
                        throw new SudokuInvalidFlagException("Cannot use '-m, --more' flag more than once!");
                    }
                    _showMore = true;
                }

                else if (parts[i] == "--letters")
                {
                    if (_printLetters)
                    {
                        throw new SudokuInvalidFlagException("Cannot use '--letters' flag more than once!");
                    }

                    _printLetters = true;
                }
                else
                    throw new SudokuInvalidFlagException("Invalid flag. do you mean '--more' or '--letters'?");
            }

            return parts[0];
        }

        throw new TooManyArgumentsException("Too many arguments. Type 'help' or try again");
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
        Log($"{GREEN}File input path       :{RESET} {filePath}");
        Log($"{GREEN}Output file           :{RESET} {fileHandler.OutputPath}");
        Log($"{GREEN}Number of puzzles     :{RESET} {fileHandler.TotalPuzzles}");
        Log($"{GREEN}Avg time to solve     :{RESET} {SudokuHelpers.GetFormattedTime(fileHandler.AvgTimeMs)}");
        Log($"{GREEN}Longest time to solve :{RESET} {SudokuHelpers.GetFormattedTime(fileHandler.MaxTimeMs)} (puzzle #{fileHandler.MaxTimePuzzleIndex})");
        Log($"{GREEN}Max guesses           :{RESET} {fileHandler.MaxBacktrackCalls} (puzzle #{fileHandler.MaxBacktrackCallsIndex})");
        Log($"{GREEN}Avg guesses           :{RESET} {fileHandler.AvgBacktrackingCalls:F3}");
        Log($"{GREEN}Total time taken      :{RESET} {SudokuHelpers.GetFormattedTime(totalStopwatch.Elapsed.TotalMilliseconds)}");
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
            bool isOriginalCell = solvedPuzzle[index] == givenPuzzle[index];
            
            string color = isOriginalCell ? PURPLE : ORANGE;
            char cellValue = solvedPuzzle[index];

            if (_printLetters)
            {
                cellValue += (char)('A' - '1');
            }

            formattedPuzzle.Append(color + cellValue + RESET);
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
