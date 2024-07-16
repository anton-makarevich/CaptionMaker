namespace CaptionMaker.Core.Models;

public class CaptionResult
{
    public CaptionResult(string name, List<CaptionLine> captions)
    {
        Name = name;
        Captions = captions;
    }
    
    public List<CaptionLine> Captions { get; private set; }
    public string Name { get; }
}