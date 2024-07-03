using CaptionMaker.Core.Models;

namespace CaptionMaker.Core.Services;

public class CaptionStructuriser
{
    public List<CaptionLine> ProcessCaptions(List<CaptionLine> captions)
    {
        var processed = CombineIntoCompleteSentences(captions);
        processed = AdjustSegmentLengths(processed);
        return processed;
    }

    private static List<CaptionLine> CombineIntoCompleteSentences(List<CaptionLine> captions)
    {
        List<CaptionLine> combined = [];
        var currentText = string.Empty;
        var currentStart = TimeSpan.Zero;
        var previousEnd = TimeSpan.Zero;

        foreach (var caption in captions)
        {
            currentText += (string.IsNullOrEmpty(currentText) ? "" : " ") + caption.Text;

            var sentences = SplitIntoSentences(currentText);
            for (var j = 0; j < sentences.Count - 1; j++)
            {
                var sentence = sentences[j];
                var sentenceProportion = (double)sentence.Length / currentText.Length;
                var sentenceEnd = currentStart + (caption.End - currentStart) * sentenceProportion;

                combined.Add(new CaptionLine
                {
                    Text = sentence.Trim(),
                    Start = currentStart,
                    End = sentenceEnd
                });

                currentStart = sentenceEnd;
            }

            currentText = sentences.Last().Trim();
            previousEnd = caption.End;
        }

        if (!string.IsNullOrEmpty(currentText))
        {
            combined.Add(new CaptionLine
            {
                Text = currentText.Trim(),
                Start = currentStart,
                End = previousEnd
            });
        }

        return combined;
    }

    private static List<CaptionLine> AdjustSegmentLengths(List<CaptionLine> combined)
    {
        List<CaptionLine> adjusted = [];
        for (var i = 0; i < combined.Count; i++)
        {
            var caption = combined[i];

            // Combine short segments (less than 3 words) with the next one
            if (WordCount(caption.Text) < 3 && i + 1 < combined.Count)
            {
                var nextCaption = combined[i + 1];
                combined[i + 1] = new CaptionLine
                {
                    Text = caption.Text + " " + nextCaption.Text,
                    Start = caption.Start,
                    End = nextCaption.End
                };
                continue;
            }

            if (caption.End - caption.Start > TimeSpan.FromSeconds(10))
            {
                // Split long sentences proportionally
                adjusted.AddRange(SplitLongCaption(caption));
            }
            else
            {
                adjusted.Add(caption);
            }
        }

        return adjusted;
    }

    private static int WordCount(string text)
    {
        return text.Split(new[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }

    private static List<CaptionLine> SplitLongCaption(CaptionLine caption)
    {
        List<CaptionLine> result = [];
        var totalDuration = (caption.End - caption.Start).TotalSeconds;
        const double splitDurationSeconds = 10; //To some config?
        var words = caption.Text.Split(' ');

        var startWordIndex = 0;
        var currentStart = caption.Start;

        while (totalDuration > splitDurationSeconds)
        {
            var proportion = splitDurationSeconds / totalDuration;
            var splitWordCount = (int)(words.Length * proportion);

            var firstPart = string.Join(" ", words.Skip(startWordIndex).Take(splitWordCount));
            currentStart += TimeSpan.FromSeconds(splitDurationSeconds);

            result.Add(new CaptionLine
            {
                Text = firstPart,
                Start = caption.Start,
                End = currentStart
            });

            startWordIndex += splitWordCount;
            totalDuration -= splitDurationSeconds;
            caption = new CaptionLine
            {
                Text = string.Join(" ", words.Skip(startWordIndex)),
                Start = currentStart,
                End = caption.End
            };
        }

        if (startWordIndex < words.Length)
        {
            result.Add(caption);
        }

        return result;
    }

    private static List<string> SplitIntoSentences(string text)
    {
        List<string> sentences = [];
        var sentenceStart = 0;
        var inPunctuation = false;

        for (var i = 0; i < text.Length; i++)
        {
            if (char.IsPunctuation(text[i]))
            {
                if (!inPunctuation)
                {
                    inPunctuation = true;
                }
            }
            else if (inPunctuation)
            {
                inPunctuation = false;
                sentences.Add(text.Substring(sentenceStart, i - sentenceStart));
                sentenceStart = i;
            }
        }

        if (sentenceStart < text.Length)
        {
            sentences.Add(text.Substring(sentenceStart));
        }

        return sentences;
    }
}
