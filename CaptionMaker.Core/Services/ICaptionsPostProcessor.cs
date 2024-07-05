using CaptionMaker.Core.Models;

namespace CaptionMaker.Core.Services;

public interface ICaptionsPostProcessor
{
    public Task<List<CaptionLine> > ProcessCaptions(List<CaptionLine> captions);
}