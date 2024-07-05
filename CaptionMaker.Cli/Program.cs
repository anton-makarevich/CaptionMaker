using CaptionMaker.Core.Models;
using CaptionMaker.Core.Services;
using Whisper.net.Ggml;

var modelProvider = new ModelProvider();
var speechRecognizer = new SpeechRecognizer();
var structuriser = new CaptionStructuriser();
var spellingChecker = new SpellingChecker();
var postProcessors = new List<ICaptionsPostProcessor> { structuriser, spellingChecker };
var captionMaker = new CaptionMaker.Core.CaptionMaker(modelProvider, speechRecognizer);

var parameters = new CaptionParameters
{
    AudioFilePath = "intro.wav",
    Language = "be",
    ModelType = GgmlType.LargeV2
};

var captions = await captionMaker.CreateCaption(parameters, postProcessors);

foreach (var captionLine in captions)
{
    Console.WriteLine($"{captionLine.Start} -> {captionLine.End}: {captionLine.Text}");
}
