﻿namespace ArielSudoku.Common;
/// <summary>
/// Global static class that share constants globally
/// </summary>
public sealed class Constants
{
    // Size of each sub-box, for example 3x3 for a 9x9 Sudoku
    // 3
    public readonly int BoxSize;

    // Length of each rows and columns, for example it will be 9 if BoxLen is 3
    // 9
    public readonly int BoardSize;

    // Total cells in the board, for example 81 for if BoxLen is 3
    // 81
    public readonly int CellCount;

    // Bitmask that store as int an mask that is the same as if the cell
    // Was the same if cell could contain any digit
    // For example: if BoardSize == 4 than AllPossibleDigitsMask is 00011110
    public readonly int AllPossibleDigitsMask;


    // Holds the (row, col, box) per each cell so no recomputing every time for fast access
    public (int row, int col, int box)[] CellCoordinates { get; }


    // Arrays that holds all cells in a specific row, column, or box
    // For example: if you CellsInBox[5] (which is box number 4), you will get
    // An array containing all of the cellIndexes inside that box
    public int[][] CellsInRow { get; }
    public int[][] CellsInCol { get; }
    public int[][] CellsInBox { get; }



    /// <summary>
    /// Store all cells that share the same (row, col, box) with each cell.
    /// </summary>

    public int[][] CellNeighbors { get; }

    /// <summary>
    /// Static constructor that only run one before access any constant inside this file
    /// It fills the CellCoordinates once in the runtime lifetime, so less total calculations
    /// </summary>
    public Constants(int boxSize)
    {
        BoxSize = boxSize;

        // Precompute simple constants
        BoardSize = BoxSize * BoxSize;
        CellCount = BoardSize * BoardSize;
        AllPossibleDigitsMask = ((1 << BoardSize) - 1) << 1;

        // Assign sizes to arrays
        CellCoordinates = new (int, int, int)[CellCount];
        CellsInRow = new int[BoardSize][];
        CellsInCol = new int[BoardSize][];
        CellsInBox = new int[BoardSize][];
        CellNeighbors = new int[CellCount][];

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

            // Remove the cell itself, because he isn't his own neighbors
            neighbors.Remove(cellIndex);

            // Store the peer set as an array
            CellNeighbors[cellIndex] = [.. neighbors];
        }
    }
}

