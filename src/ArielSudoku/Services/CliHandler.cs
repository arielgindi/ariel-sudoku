using System;
using System.Diagnostics;

namespace ArielSudoku
{
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

        /// <summary>
        /// Prints a short welcome message.
        /// </summary>
        private static void PrintWelcome()
        {
            Console.WriteLine($"{CYAN}====================================={RESET}");
            Console.WriteLine($"{CYAN}{BOLD}          Gindi Calculator{RESET}");
            Console.WriteLine($"{CYAN}====================================={RESET}");
            Console.WriteLine("Enter exactly 81 characters to input a Sudoku puzzle.");
            Console.WriteLine("Type 'exit' to quit.");
            Console.WriteLine();
        }

        /// <summary>
        /// Main loop: waits for input, checks length, and processes the Sudoku puzzle.
        /// </summary>
        public static void Run()
        {
            PrintWelcome();

            while (true)
            {
                Console.Write($"{CYAN}>> {RESET}");
                string? userInput = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine($"{RED}Error: Input cannot be empty.{RESET}");
                    continue;
                }

                try
                {
                    if (userInput.Length != 81)
                    {
                        throw new FormatException($"Input must be 81 characters, but it is {userInput.Length}.");
                    }

                    Stopwatch stopwatch = Stopwatch.StartNew();
                    string solvedPuzzle = SudokuEngine.SolveSudoku(userInput);
                    stopwatch.Stop();

                    Console.WriteLine($"{GREEN}Solved Sudoku:{RESET} {solvedPuzzle}");
                    Console.WriteLine($"(Solved in {stopwatch.Elapsed.TotalSeconds:F3}s)");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{RED}Error: {ex.Message}{RESET}");
                }
            }
        }
    }
}
