using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaApplication.Avalonia.DI;
using AvaloniaApplication.Avalonia.ViewModels;
using AvaloniaApplication.Avalonia.Views;
using CaptionMaker.Core.DI;
using Microsoft.Extensions.DependencyInjection;
using Sanet.MVVM.Core.Services;
using Sanet.MVVM.Navigation.Avalonia.Services;

namespace AvaloniaApplication.Avalonia;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        IServiceCollection services = new ServiceCollection();
        services.RegisterCaptionMaker();
        services.RegisterViewModels();
        var serviceProvider = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            INavigationService navigationService = new NavigationService(desktop,serviceProvider);
            var viewModel =  navigationService.GetViewModel<MainViewModel>();
            desktop.MainWindow = new MainWindow
            {
                Content = new MainView()
                {
                    ViewModel = viewModel
                }
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView();
        }

        base.OnFrameworkInitializationCompleted();
    }
}