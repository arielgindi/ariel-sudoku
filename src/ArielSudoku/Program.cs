using System;

namespace ArielSudoku
{
    internal class Program
    {
        /// <summary>
        /// Main entry point of the application.
        /// </summary>
        static void Main(string[] args)
        {
            // Hand off control to the CLI handler
            CliHandler.Run();
        }
    }
}
