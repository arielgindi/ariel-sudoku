using ArielSudoku.Exceptions;
namespace ArielSudoku.Common;

public static class ConstantsManager
{
    // A dictionary where the key is the boxSize
    // For example: 3 for 9x9 or 5 for 25x25
    private static readonly Dictionary<int, Constants> _constantsByBoxSize = [];

    /// <summary>
    /// Return a Constants address for a given boxSize
    /// It's cached, so if already exists reused, else create a new one.
    /// Valid box sizes: (1,2,3,4,5)
    /// </summary>
    /// <param name="boxSize">Size of the box, For example: 3 for 9x9 puzzle</param>
    /// <returns>Return a Constants address for a given boxSize</returns>
    /// <exception cref="InputInvalidLengthException">Thrown if invalid box size is given</exception>
    public static Constants GetOrCreateConstants(int boxSize)
    {
        if (boxSize < 1 || boxSize > 5)
        {
            throw new InputInvalidLengthException("Invalid size. Valid box sizes: (1,2,3,4,5)");
        }

        if (_constantsByBoxSize.TryGetValue(boxSize, out Constants? cachedConstants))
        {
            return cachedConstants!;
        }

        Constants newConstants = new(boxSize);
        _constantsByBoxSize[boxSize] = newConstants;
        return newConstants;
    }
}