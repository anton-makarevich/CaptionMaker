using CaptionMaker.Core.Models;
using CaptionMaker.Core.Services;
using Whisper.net.Ggml;

var modelProvider = new ModelProvider();
var speechRecognizer = new SpeechRecognizer();
var structuriser = new CaptionStructuriser();
var captionMaker = new CaptionMaker.Core.CaptionMaker(modelProvider, speechRecognizer, structuriser);

var parameters = new CaptionParameters
{
    AudioFilePath = "intro.wav",
    Language = "be",
    ModelType = GgmlType.LargeV2
};

var captions = await captionMaker.CreateCaption(parameters);

foreach (var captionLine in captions)
{
    Console.WriteLine($"{captionLine.Start} -> {captionLine.End}: {captionLine.Text}");
}
