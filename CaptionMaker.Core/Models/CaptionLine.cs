namespace CaptionMaker.Core.Models;

public class CaptionLine
{
    public required string Text { get; set; }
    public TimeSpan Start { get; set; }
    public TimeSpan End { get; set; }
}