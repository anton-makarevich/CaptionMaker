using CaptionMaker.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CaptionMaker.Core.DI;

public static class DiModule
{
    public static void RegisterCaptionMaker(this IServiceCollection services)
    {
        var modelProvider = new ModelProvider();
        var speechRecognizer = new SpeechRecognizer();
        var captionMaker = new global::CaptionMaker.Core.CaptionMaker(modelProvider, speechRecognizer);
        services.AddSingleton(captionMaker);
        services.AddSingleton(new SrtWriter());
    }
}