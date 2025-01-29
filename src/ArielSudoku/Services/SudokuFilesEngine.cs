using System.Diagnostics;
using System.Diagnostics;

namespace ArielSudoku.Services;

public static class SudokuFilesEngine
{
    public static string ProccessSudokuFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new Exception($"File at path {filePath} was not found");
        }

        string[] puzzleLines = File.ReadAllLines(filePath);
        string[] outPutPuzzles = new string[puzzleLines.Length];

        // Go through each puzzle and try to solve it
        for (int i = 0; i < puzzleLines.Length; i++)
        {
            // remove spaces from right and left
            string puzzle = puzzleLines[i].Trim();

            try
            {
                (string solvedPuzzle, int _) = SudokuEngine.SolveSudoku(puzzle);
                outPutPuzzles[i] = solvedPuzzle + '\n';
            }
            catch (Exception ex)
            {
                throw new Exception($"Puzzle at line {i + 1} failed with error: {ex.Message}");
            }
        }

        string outputPath = "C:/sudoku-answers/output.txt";
        string fileContent = string.Concat(outPutPuzzles);
        File.WriteAllText(outputPath, fileContent);

        Console.WriteLine($"Proccessing sudoku file completed successfully, output file location: ${outputPath} ");

        // return output file path
        return outputPath;
    }
  }
}