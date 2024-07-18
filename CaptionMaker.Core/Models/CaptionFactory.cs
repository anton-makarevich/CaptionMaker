using System.Globalization;
using System.Text.RegularExpressions;

namespace CaptionMaker.Core.Models;

public static class CaptionFactory
{
    public static async Task<List<CaptionLine>> CreateFromSrt(string srtFilePath, TimeSpan? adjustment = null)
    {
        var captions = new List<CaptionLine>();
        var srtContent = await File.ReadAllTextAsync(srtFilePath);
        var regex = new Regex(@"(\d+)\s+(\d{2}:\d{2}:\d{2},\d{3}) --> (\d{2}:\d{2}:\d{2},\d{3})\s+([\s\S]*?)(?=\r?\n\r?\n|\z)", RegexOptions.Multiline);

        var matches = regex.Matches(srtContent);
        foreach (Match match in matches)
        {
            var start = TimeSpan.ParseExact(match.Groups[2].Value, @"hh\:mm\:ss\,fff", CultureInfo.InvariantCulture);
            var end = TimeSpan.ParseExact(match.Groups[3].Value, @"hh\:mm\:ss\,fff", CultureInfo.InvariantCulture);
            var text = match.Groups[4].Value.Trim().Replace("\r\n", "\n");
            
            if (adjustment.HasValue)
            {
                start = start.Add(adjustment.Value);
                end = end.Add(adjustment.Value);
            }
            
            captions.Add(new CaptionLine
            {
                Start = start,
                End = end,
                Text = text
            });
        }

        return captions;
    }
}