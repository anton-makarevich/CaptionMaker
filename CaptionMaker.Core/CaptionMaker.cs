using CaptionMaker.Core.Models;
using CaptionMaker.Core.Services;

namespace CaptionMaker.Core;

public class CaptionMaker
{
    private readonly ModelProvider _modelProvider;
    private readonly SpeechRecognizer _speechRecognizer;

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
        var captions = await _speechRecognizer.Recognize(audioFilePath, model, parameters.Language);
        
        foreach (var processor in processors)
        {
            captions = await processor.ProcessCaptions(captions);
            foreach (var captionLine in captions)
            {
                Console.WriteLine($"{captionLine.Start} -> {captionLine.End}: {captionLine.Text}");
            }
        }

        return captions;
    }
}