using System.Diagnostics;

namespace ArielSudoku
{
    /// <summary>
    /// Handles CLI input for a fake Sudoku demo.
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
            Console.WriteLine("Enter exactly 81 characters to mimic a Sudoku input.");
            Console.WriteLine();
        }

        /// <summary>
        /// Main loop: waits for input, checks length, returns dummy result.
        /// </summary>
        public static void Run()
        {
            PrintWelcome();

            while (true)
            {
                Console.Write($"{CYAN}>> {RESET}");
                string? userInput = Console.ReadLine()?.Trim();

                if (userInput == null)
                {
                    Console.WriteLine("\nGoodbye!");
                    break;
                }
                try
                {
                    if (userInput.Length != 81)
                    {
                        throw new FormatException($"Input must be 81 characters, but it is {userInput.Length}.");
                    }

                    string fakeSolution = new('9', 81);

                    Console.WriteLine($"{GREEN}Result: {fakeSolution}{RESET} (0.024s)");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{RED}Error: {ex.Message}{RESET} (0.003s)");
                }
            }
        }
    }
}
