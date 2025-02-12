namespace ArielSudoku.CLI;

public class RuntimeStatistics
{
    public int GuessCount { get; set; } = 0;
    public int PlaceDigitCount { get; set; } = 0;
    public int HiddenSinlgesCount { get; set; } = 0;
    public int NakedSinglesCount { get; set; } = 0;
    public int FoundDeadEndCount { get; set; } = 0;
}
