using Whisper.net.Ggml;

namespace CaptionMaker.Core.Services;

public class ModelProvider
{
    public async Task<string> CheckModel(GgmlType type = GgmlType.LargeV3)
    {
        var modelName = $"ggml-{type}.bin".ToLower();
        if (!File.Exists(modelName))
        {
            await using var modelStream = await WhisperGgmlDownloader.GetGgmlModelAsync(GgmlType.LargeV3);
            await using var fileWriter = File.OpenWrite(modelName);
            await modelStream.CopyToAsync(fileWriter);
        }
        return modelName;
    }
}