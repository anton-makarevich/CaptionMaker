using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CaptionMaker.Avalonia.DI;
using CaptionMaker.Avalonia.ViewModels;
using CaptionMaker.Core.DI;
using Microsoft.Extensions.DependencyInjection;
using Sanet.MVVM.Core.Services;
using Sanet.MVVM.Navigation.Avalonia.Services;
using MainView = CaptionMaker.Avalonia.Views.MainView;
using MainWindow = CaptionMaker.Avalonia.Views.MainWindow;

namespace CaptionMaker.Avalonia;

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