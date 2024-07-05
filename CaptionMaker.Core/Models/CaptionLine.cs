namespace CaptionMaker.Core.Models;

public class CaptionLine
{
    public required string Text { get; set; }
    public TimeSpan Start { get; init; }
    public TimeSpan End { get; init; }
}