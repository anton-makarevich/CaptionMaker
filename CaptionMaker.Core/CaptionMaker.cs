using CaptionMaker.Core.Models;
using CaptionMaker.Core.Services;

namespace CaptionMaker.Core;

public class CaptionMaker
{
    private readonly ModelProvider _modelProvider;
    private readonly SpeechRecognizer _speechRecognizer;
    
    public List<CaptionResult> ProcessedCaptions { get; set; } = [];

    public CaptionMaker(
        ModelProvider modelProvider,
        SpeechRecognizer speechRecognizer)
    {
        _modelProvider = modelProvider;
        _speechRecognizer = speechRecognizer;
    }

    public async Task<List<CaptionLine>> CreateCaption(CaptionParameters parameters,
        List<ICaptionsPostProcessor> processors)
    {
        var model = await _modelProvider.CheckModel(parameters.ModelType);
        var audioFilePath = parameters.AudioFilePath;

        ProcessedCaptions = new List<CaptionResult>();
        
        var captions = await _speechRecognizer.Recognize(audioFilePath, model, parameters.Language);
        ProcessedCaptions.Add(new CaptionResult("Raw", captions));
        
        foreach (var processor in processors)
        {
            var result = await processor.ProcessCaptions(captions);
            ProcessedCaptions.Add(result);
            captions = result.Captions;
        }

        return captions;
    }
}