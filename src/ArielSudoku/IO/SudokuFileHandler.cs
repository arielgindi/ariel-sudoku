using System.Diagnostics;

namespace ArielSudoku.IO
{
    public class SudokuFileHandler
    {
        // Tracking status
        public int TotalPuzzles { get; private set; }
        public double MaxTimeMs { get; private set; }
        public int MaxTimePuzzleIndex { get; private set; }
        public string OutputPath { get; private set; } = "";



        // Array containing all puzzles from the input file
        private readonly string[] _inputPuzzles;
        // Array containing all result from the input file
        private readonly string[] _solvedPuzzles;

        /// <summary>
        /// Solve from a given file all sudokus puzzles, 
        /// And keep the result inside a new output file
        /// </summary>
        /// <param name="filePath"></param>
        /// <exception cref="FileNotFoundException"></exception>
        public SudokuFileHandler(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found: " + filePath);
            }

            _inputPuzzles = File.ReadAllLines(filePath);
            TotalPuzzles = _inputPuzzles.Length;
            _solvedPuzzles = new string[TotalPuzzles];
            ProcessSudokuFile();
        }

        /// <summary>
        /// Solve all puzzles from the file and 
        /// </summary>
        /// <exception cref="Exception">Thrown if file does not contain any puzzles</exception>
        private void ProcessSudokuFile()
        {
            if (TotalPuzzles == 0)
            {
                throw new Exception("File cannot contain zero puzzles");
            }

            Console.WriteLine("Start processing all puzzles.....");
            for (int puzzleIndex = 0; puzzleIndex < TotalPuzzles; puzzleIndex++)
            {
                ProcessSinglePuzzle(puzzleIndex);
            }

            CreateOutputFile();
            Console.WriteLine($"Processing puzzles file completed successfully, output file location: ${OutputPath} ");
        }

        /// <summary>
        /// Solve a single puzzle, and update the traking stats
        /// </summary>
        /// <param name="puzzleIndex">Index of puzzle to solve (in _solvedPuzzles array)</param>
        private void ProcessSinglePuzzle(int puzzleIndex)
        {
            string puzzleString = _inputPuzzles[puzzleIndex].Trim();

            Stopwatch singlePuzzleWatch = new();
            singlePuzzleWatch.Start();

            (string solvedPuzzle, int _) = SudokuEngine.SolveSudoku(puzzleString);

            singlePuzzleWatch.Stop();
            _solvedPuzzles[puzzleIndex] = solvedPuzzle;

            double currentPuzzleMs = singlePuzzleWatch.Elapsed.TotalMilliseconds;
            if (currentPuzzleMs > MaxTimeMs)
            {
                MaxTimeMs = currentPuzzleMs;
                MaxTimePuzzleIndex = puzzleIndex + 1;
            }
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
}
