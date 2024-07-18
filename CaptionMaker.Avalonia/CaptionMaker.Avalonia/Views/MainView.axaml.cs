using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using CaptionMaker.Avalonia.ViewModels;
using Sanet.MVVM.Views.Avalonia;

namespace CaptionMaker.Avalonia.Views;

public partial class MainView : BaseView<MainViewModel>
{
    private TopLevel? _topLevel;
    public MainView()
    {
        InitializeComponent();
    }
    
    private static readonly string[] AudioExtensions = ["*.wav"];
    private static readonly string[] CaptionExtensions = ["*.srt"];

    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        _topLevel = TopLevel.GetTopLevel(this);
        // Start async operation to open the dialog.
        if (_topLevel == null) return;
        var files = await _topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Wav File",
            FileTypeFilter = [new FilePickerFileType("Only wav audio")
            {
                Patterns = AudioExtensions }],
            AllowMultiple = false
        });

        if (files.Count < 1) return;
        if (ViewModel == null) return;
        ViewModel.MediaFile = files[0].Path.AbsolutePath;
    }

    private async void OpenSrt(object? sender, RoutedEventArgs e)
    {
        _topLevel = TopLevel.GetTopLevel(this);
        // Start async operation to open the dialog.
        if (_topLevel == null) return;
        var files = await _topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open SRT File",
            FileTypeFilter = [new FilePickerFileType("Only SRT captions")
            {
                Patterns = CaptionExtensions }],
            AllowMultiple = false
        });

        if (files.Count < 1) return;
        if (ViewModel == null) return;
        await ViewModel.LoadSrtFile(files[0].Path.AbsolutePath);
    }
}