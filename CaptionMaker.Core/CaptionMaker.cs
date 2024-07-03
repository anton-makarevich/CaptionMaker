using CaptionMaker.Core.Models;
using CaptionMaker.Core.Services;

namespace CaptionMaker.Core;

public class CaptionMaker
{
    private readonly ModelProvider _modelProvider;
    private readonly SpeechRecognizer _speechRecognizer;
    private readonly CaptionStructuriser _captionStructuriser;

    public CaptionMaker(
        ModelProvider modelProvider,
        SpeechRecognizer speechRecognizer,
        CaptionStructuriser captionStructuriser)
    {
        _modelProvider = modelProvider;
        _speechRecognizer = speechRecognizer;
        _captionStructuriser = captionStructuriser;
    }
    
    public async Task<List<CaptionLine>> CreateCaption(CaptionParameters parameters)
    {
        var model = await _modelProvider.CheckModel(parameters.ModelType);
        var audioFilePath = parameters.AudioFilePath;
        var captions = await _speechRecognizer.Recognize(audioFilePath, model, parameters.Language);
        foreach (var captionLine in captions)
        {
            Console.WriteLine($"{captionLine.Start} -> {captionLine.End}: {captionLine.Text}");
        }
        captions = _captionStructuriser.ProcessCaptions(captions);
        return captions;
    }
    
}