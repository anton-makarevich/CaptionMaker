using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace CaptionMaker.Avalonia.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private static readonly string[] WavTypeExtensions = ["*.wav"];

    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {

        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Wav File",
            FileTypeFilter = [new FilePickerFileType("Only WebP Images")
            {
                Patterns = WavTypeExtensions }],
            AllowMultiple = false
        });

        if (files.Count >= 1)
        {
            
        }
    }
}