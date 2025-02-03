﻿namespace ArielSudoku.Services;

public sealed partial class SudokuSolver
{
    private void ApplyHumanTactics(Stack<(int cellIndex, int digit)>? humanTacticsStack)
    {
        while (ApplyNakedSingles(humanTacticsStack) || ApplyHiddenSingles(humanTacticsStack)) { }
    }

    /// <summary>
    /// Try to place digits in cells that have exactly one possibility
    /// </summary>
    /// <param name="humanTacticsStack"></param>
    /// <returns></returns>
    private bool ApplyNakedSingles(Stack<(int cellIndex, int digit)>? humanTacticsStack)
    {
        bool isChanged = false;
        for (int cellIndex = 0; cellIndex < CellCount; cellIndex++)
        {
            if (_board[cellIndex] == 0 && _board.HasSingleOption(cellIndex))
            {
                int digit = _board.GetOnlyPossibleDigit(cellIndex);
                _board.PlaceDigit(cellIndex, digit);
                isChanged = true;

                // Push if not null
                humanTacticsStack?.Push((cellIndex, digit));
            }
        }
        return isChanged;
    }

    /// <summary>
    /// Search for cells that there is only one
    /// possiblie solution it his same (row, col or box)
    /// For example: if digit 6 is the only possibile digit in that row, it place it there
    /// </summary>
    /// <param name="humanTacticsStack"></param>
    /// <returns></returns>
    private bool ApplyHiddenSingles(Stack<(int cellIndex, int digit)>? humanTacticsStack)
    {
        bool isChanged = false;

        for (int unitIndex = 0; unitIndex < BoardSize; unitIndex++)
        {
            isChanged |= _board.FindHiddenSinglesInUnit(CellsInRow[unitIndex], humanTacticsStack);
            isChanged |= _board.FindHiddenSinglesInUnit(CellsInCol[unitIndex], humanTacticsStack);
            isChanged |= _board.FindHiddenSinglesInUnit(CellsInBox[unitIndex], humanTacticsStack);
        }

        return isChanged;
    }



    private void UndoHumanTacticsMoves(Stack<(int cellIndex, int digit)> humanTacticsStack)
    {
        while (humanTacticsStack.Count > 0)
        {
            (int cellIndex, int digit) = humanTacticsStack.Pop();
            _board.RemoveDigit(cellIndex, digit);
        }
    }
}