namespace ArielSudoku.Common;
/// <summary>
/// Global static class that shares constants globaly
/// </summary>
public static class Constants
{
    /// <summary>
    /// Is search made progress, did nothing, or found dead end
    /// </summary>
    public enum SearchStatus
    {
        /// <summary>
        /// Mean placed at least one digit (good!)
        /// </summary>
        Found,   
        /// <summary>
        /// Mean no digits were placed
        /// </summary>
        NotFound,
        /// <summary>
        /// Mean there is a dead end, need to undo human tactics
        /// and try another guess on backtracking, or throw if board has no solution 
        /// </summary>
        DeadEnd
    }

    // Size of each sub-box, for example 3x3 for a 9x9 Sudoku
    // 3
    public static readonly int BoxSize = 3;

    // Length of each rows and columns, for example it will be 9 if BoxLen is 3
    // 9
    public static readonly int BoardSize = BoxSize * BoxSize;

    // Total cells in the board, for example 81 for if BoxLen is 3
    // 81
    public static readonly int CellCount = BoardSize * BoardSize;

    // Holds the (row, col, box) per each cell so no recomputing every time for fast access
    public static readonly (int row, int col, int box)[] CellCoordinates;


    // Arrays that holds all cells in a specific row, column, or box
    // For example: if you CellsInBox[5] (which is box number 4), you will get
    // An array containing all of the cellIndexes inside that box
    public static readonly int[][] CellsInRow = new int[BoardSize][];
    public static readonly int[][] CellsInCol = new int[BoardSize][];
    public static readonly int[][] CellsInBox = new int[BoardSize][];

    // Bitmask that store as int an mask that is the same as if the cell
    // Was the same if cell could contain any digit
    // For example: if BoardSize == 4 than AllPossibiliteDigitsMask is 00011110
    public static readonly int AllPossibleDigitsMask = ((1 << BoardSize) - 1) << 1;
   
    /// <summary>
    /// Store all cells that share the same (row, col, box) with each cell.
    /// </summary>

    public static readonly int[][] CellNeighbors = new int[CellCount][];

    /// <summary>
    /// Static constractor that only run one before access any constant inside this file
    /// It fills the CellCoordinantes once in the runtine lifetime, so less total calculations
    /// </summary>
    static Constants()
    {
        CellCoordinates = new (int, int, int)[CellCount];

        for (int i = 0; i < BoardSize; i++)
        {
            CellsInRow[i] = new int[BoardSize];
            CellsInCol[i] = new int[BoardSize];
            CellsInBox[i] = new int[BoardSize];
        }

        for (int cellIndex = 0; cellIndex < CellCount; cellIndex++)
        {
            // calculate in which (row, col, box) is every cellIndex
            int row = cellIndex / BoardSize;
            int col = cellIndex % BoardSize;
            int box = (row / BoxSize) * BoxSize + (col / BoxSize);

            // Save the (row, col, box) for this cell
            CellCoordinates[cellIndex] = (row, col, box);

            // Figure out where in the box array this cell goes
            int indexInsideBox = (row % BoxSize) * BoxSize + (col % BoxSize);

            // Save the cell index in the right place for (row, col, box)
            CellsInRow[row][col] = cellIndex;
            CellsInCol[col][row] = cellIndex;
            CellsInBox[box][indexInsideBox] = cellIndex;
        }

        for (int cellIndex = 0; cellIndex < CellCount; cellIndex++)
        {
            (int row, int col, int box) = CellCoordinates[cellIndex];

            HashSet<int> neighbors =
            [                
                // Add (row, col, box) neighbors
                .. CellsInRow[row],
                .. CellsInCol[col],
                .. CellsInBox[box],
            ];

            // Remove the cell itself, because he is not his neighbors
            neighbors.Remove(cellIndex);

            // Store the peer set as an array
            CellNeighbors[cellIndex] = [.. neighbors];
        }
    }
}

