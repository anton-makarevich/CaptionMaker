using CaptionMaker.Core.Models;
using WeCantSpell.Hunspell;

namespace CaptionMaker.Core.Services;

public class SpellingChecker:ICaptionsPostProcessor
{
    public async Task<CaptionResult> ProcessCaptions(List<CaptionLine> captions)
    {
        await using var dictionaryStream = File.OpenRead("be.dic");
        await using var affixStream = File.OpenRead("be.aff");
        var dictionary = await WordList.CreateFromStreamsAsync(dictionaryStream, affixStream);
        var processed = new List<CaptionLine>();
        foreach (var captionLine in captions)
        {
            var processedCaptionLine = new CaptionLine{Text = captionLine.Text, Start = captionLine.Start, End = captionLine.End};
            var words = captionLine.Text.Split(" ");
            foreach (var word in words)
            {
                var trimmedWord = word.Trim();
                Console.WriteLine($"checking {trimmedWord}");
                var result = dictionary.CheckDetails(trimmedWord);
                Console.WriteLine($"result for {trimmedWord} is {result.Correct}");
                Console.WriteLine($"{result.Root}");
                Console.WriteLine($"{result.Info.ToString()}");
                if (!result.Correct)
                {
                    var suggestions = dictionary.Suggest(trimmedWord).ToList();
                    if (suggestions.Count == 0) continue;
                    Console.WriteLine($"suggestions for {trimmedWord} are {string.Join(", ", suggestions)}");
                    processedCaptionLine.Text=processedCaptionLine.Text.Replace(trimmedWord, suggestions.First());
                }
            }
            processed.Add(processedCaptionLine);
        }

        return new CaptionResult(Name, processed);
    }

    public string Name { get; } = "Hunspell SpellChecker";
}