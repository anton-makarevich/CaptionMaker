using CaptionMaker.Avalonia.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CaptionMaker.Avalonia.DI;

public static class ViewModelsModule
{
    public static void RegisterViewModels(this IServiceCollection services)
    {
        services.AddTransient<MainViewModel, MainViewModel>();
    }
}