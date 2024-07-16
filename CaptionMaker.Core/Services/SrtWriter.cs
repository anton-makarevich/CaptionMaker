using System.Text;
using CaptionMaker.Core.Models;

namespace CaptionMaker.Core.Services;

public class SrtWriter
{
    public async Task WriteSrt(string filePath, List<CaptionLine> captions)
    {
        var sb = new StringBuilder();
        var index = 1;
        foreach (var caption in captions)
        {
            sb.AppendLine($"{index}");
            sb.AppendLine($"{caption.Start:hh\\:mm\\:ss\\,fff} --> {caption.End:hh\\:mm\\:ss\\,fff}");
            sb.AppendLine(caption.Text);
            sb.AppendLine("");
            index++;
        }

        await File.WriteAllTextAsync(filePath, sb.ToString());
    }
}