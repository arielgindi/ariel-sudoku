namespace ArielSudoku.Models;

public sealed partial class SudokuBoard
{
    /// <summary>
    /// A lightweight class that store all usage as a snapshot
    /// </summary>
    public class SudokuSnapshot
    {
        public int[] Cells;
        public int[] RowMask;
        public int[] ColMask;
        public int[] BoxMask;
        public int[] CellMasks;
        public int[] PossPerCell;
        public List<int> EmptyCells;

        public SudokuSnapshot(
            int[] cells,
            int[] rowMask,
            int[] colMask,
            int[] boxMask,
            int[] cellMasks,
            int[] possPerCell,
            List<int> emptyCells)
        {
            // Make a copy of each usage variable
            Cells = (int[])cells.Clone();
            RowMask = (int[])rowMask.Clone();
            ColMask = (int[])colMask.Clone();
            BoxMask = (int[])boxMask.Clone();
            CellMasks = (int[])cellMasks.Clone();
            PossPerCell = (int[])possPerCell.Clone();
            EmptyCells = new List<int>(emptyCells);
        }
    }

    public SudokuSnapshot TakeSnapshot()
    {
        return new SudokuSnapshot(
            _cells,
            _rowMask,
            _colMask,
            _boxMask,
            _cellMasks,
            _possPerCell,
            _emptyCells
        );
    }

    /// <summary>
    /// Restore the board and tracking arrays to the old snapshot
    /// </summary>
    public void RestoreSnapshot(SudokuSnapshot snap)
    {
        snap.Cells.CopyTo(_cells, 0);
        snap.RowMask.CopyTo(_rowMask, 0);
        snap.ColMask.CopyTo(_colMask, 0);
        snap.BoxMask.CopyTo(_boxMask, 0);
        snap.CellMasks.CopyTo(_cellMasks, 0);
        snap.PossPerCell.CopyTo(_possPerCell, 0);

        _emptyCells.Clear();
        _emptyCells.AddRange(snap.EmptyCells);
    }
}
