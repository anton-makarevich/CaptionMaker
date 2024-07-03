using Whisper.net.Ggml;

namespace CaptionMaker.Core.Models;

public class CaptionParameters
{
    public string AudioFilePath { get; set; }
    public string Language { get; set; }
    public GgmlType ModelType { get; set; }
}