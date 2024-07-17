using CaptionMaker.Core.Models;
using CaptionMaker.Core.Services;
using Whisper.net.Ggml;

string mediaPath = "";

do 
{
    Console.WriteLine("Enter media path:");
    mediaPath = Console.ReadLine() ?? string.Empty;
} while (!File.Exists(mediaPath));

var modelProvider = new ModelProvider();
var speechRecognizer = new SpeechRecognizer();
var structuriser = new CaptionStructuriser();
var spellingChecker = new SpellingChecker();
var postProcessors = new List<ICaptionsPostProcessor> ();

Console.WriteLine("Improve caption structure? (y/n) (recommended)");
if (Console.ReadLine()?.ToLower() == "y")
{
    postProcessors.Add(structuriser);
}

Console.WriteLine("Try and correct spelling? (y/n) (not recommended)");
if (Console.ReadLine()?.ToLower() == "y")
{
    postProcessors.Add(spellingChecker);
}

var captionMaker = new CaptionMaker.Core.CaptionMaker(modelProvider, speechRecognizer);

var parameters = new CaptionParameters
{
    AudioFilePath = mediaPath,
    Language = "be",
    ModelType = GgmlType.LargeV2
};

await captionMaker.CreateCaption(parameters, postProcessors);

Console.WriteLine();
var captionIndex = 1;

foreach (var captionResult in captionMaker.ProcessedCaptions)
{
    Console.WriteLine($"Caption #{captionIndex}, {captionResult.Name}");
    foreach (var caption in captionResult.Captions)
    {
        Console.WriteLine($"{caption.Start} -> {caption.End}: {caption.Text}");
    }

    Console.WriteLine();

    captionIndex++;
}

int resultIndex = 0;
Console.WriteLine("Select caption number:");

do
{
    int.TryParse(Console.ReadLine(), out resultIndex);
} while (resultIndex < 1 || resultIndex > captionMaker.ProcessedCaptions.Count);

var selectedCaption = captionMaker.ProcessedCaptions[resultIndex - 1];
Console.WriteLine($"Selected caption: {selectedCaption.Name}");

var srtFile = Path.ChangeExtension(mediaPath, ".srt");

var srtWriter = new SrtWriter();
await srtWriter.WriteSrt(srtFile, selectedCaption.Captions);

Console.WriteLine($"Saved to srt file {srtFile}");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
