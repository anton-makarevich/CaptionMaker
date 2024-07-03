using CaptionMaker.Core.Models;
using Whisper.net;

namespace CaptionMaker.Core.Services;

public class SpeechRecognizer
{
    public async Task<List<CaptionLine>> Recognize(string audioFilePath, string model, string language)
    {
        using var whisperFactory = WhisperFactory.FromPath(model);

        await using var processor = whisperFactory.CreateBuilder()
            .WithLanguage(language)
            .Build();

        await using var fileStream = File.OpenRead(audioFilePath);

        var captions = new List<CaptionLine>();
        
        await foreach (var result in processor.ProcessAsync(fileStream))
        {
            captions.Add(new CaptionLine
            {
                Text = result.Text,
                Start = result.Start,
                End = result.End
            });
            Console.WriteLine($"{result.End}");
        }
        return captions;
    }
}