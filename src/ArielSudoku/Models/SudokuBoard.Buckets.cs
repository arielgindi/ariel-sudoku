namespace ArielSudoku.Models;

using static ArielSudoku.Common.Constants;

public sealed partial class SudokuBoard
{
    public List<int> EmptyCells { get; } = [];

    private void InitializeEmptyCellsList()
    {
        EmptyCells.Clear();
        for (int cellNumber = 0; cellNumber < CellCount; cellNumber++)
        {
            if (this[cellNumber] == '0')
            {
                EmptyCells.Add(cellNumber);
            }
        }
    }
}
