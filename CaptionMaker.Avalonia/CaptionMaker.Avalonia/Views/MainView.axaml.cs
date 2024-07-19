using System.Threading.Tasks;
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

    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var file = await GetFilePath("wav");
        if (file == null) return;
        if (ViewModel == null) return;
        ViewModel.MediaFile = file;
    }

    private async void OpenSrt(object? sender, RoutedEventArgs e)
    {
        var file = await GetFilePath("srt");
        if (file == null) return;
        if (ViewModel == null) return;
        await ViewModel.LoadSrtFile(file);
    }

    private async void InsertSrt(object? sender, RoutedEventArgs e)
    {
        var file = await GetFilePath("srt");
        if (file == null) return;
        if (ViewModel == null) return;
        var vm = (sender as Button)?.DataContext as CaptionLineViewModel;
        vm.InsertAfterCommand.Execute(file);
    }
    
    private async Task<string?> GetFilePath(string extension)
    {
        _topLevel = TopLevel.GetTopLevel(this);
        // Start async operation to open the dialog.
        if (_topLevel == null) return null;
        var files = await _topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = $"Open {extension} File",
            FileTypeFilter = [new FilePickerFileType($"Only {extension} captions")
            {
                Patterns = [$"*.{extension}"] }],
            AllowMultiple = false
        });

        if (files.Count < 1) return null;
        if (ViewModel == null) return null;
        return files[0].Path.AbsolutePath;
    }
}