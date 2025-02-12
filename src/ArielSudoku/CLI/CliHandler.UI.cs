using ArielSudoku.Common;
using ArielSudoku.Exceptions;
using ArielSudoku.IO;
using ArielSudoku.Models;
using System.Diagnostics;
using System.Text;

namespace ArielSudoku.CLI;

internal static partial class CliHandler
{
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
