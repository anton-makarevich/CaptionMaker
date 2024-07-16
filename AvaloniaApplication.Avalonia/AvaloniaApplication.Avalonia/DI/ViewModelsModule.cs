using AvaloniaApplication.Avalonia.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaApplication.Avalonia.DI;

public static class ViewModelsModule
{
    public static void RegisterViewModels(this IServiceCollection services)
    {
        services.AddTransient<MainViewModel, MainViewModel>();
    }
}