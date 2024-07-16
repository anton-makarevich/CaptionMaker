using CaptionMaker.Core.Models;

namespace CaptionMaker.Core.Services;

public interface ICaptionsPostProcessor
{
    public Task<CaptionResult> ProcessCaptions(List<CaptionLine> captions);
    
    public string Name { get; }
}