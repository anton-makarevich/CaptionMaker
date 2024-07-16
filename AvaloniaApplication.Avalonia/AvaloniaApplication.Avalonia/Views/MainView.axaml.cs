using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using AvaloniaApplication.Avalonia.ViewModels;
using Sanet.MVVM.Views.Avalonia;

namespace AvaloniaApplication.Avalonia.Views;

public partial class MainView : BaseView<MainViewModel>
{
    public MainView()
    {
        InitializeComponent();
    }
    
    private static readonly string[] WavTypeExtensions = ["*.wav"];

    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {

        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        if (topLevel == null) return;
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Wav File",
            FileTypeFilter = [new FilePickerFileType("Only WebP Images")
            {
                Patterns = WavTypeExtensions }],
            AllowMultiple = false
        });

        if (files.Count < 1) return;
        if (ViewModel == null) return;
        ViewModel.MediaFile = files[0].Path.AbsolutePath;
    }
}