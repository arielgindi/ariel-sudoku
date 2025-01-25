﻿using System.IO;
using Xunit;

namespace ArielSudoku.Tests
{
    public class BulkHardSudokuTests : SudokuTestsBase
    {
        [Fact]
        public void SolveAllHardSudokusFromFile()
        {
            string testOutputFolder = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(testOutputFolder, "Bulk", "Puzzles", "EvilSudokusList.txt");

            Assert.True(File.Exists(filePath), $"Missing file: {filePath}");

            string[] puzzleLines = File.ReadAllLines(filePath);

            // Go through each puzzle and try to solve it
            for (int i = 0; i < puzzleLines.Length; i++)
            {
                string puzzle = puzzleLines[i].Trim();

                try
                {
                    (string solvedPuzzle, int backtrackCount) = SudokuEngine.SolveSudoku(puzzle);
                }
                catch (Exception ex)
                {
                    // Fail the test and report which line/puzzle broke
                    Assert.True(false, $"Puzzle at line {i + 1} failed with error: {ex.Message}");
                }
            }
        }
    }
}
