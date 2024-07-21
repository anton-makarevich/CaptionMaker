using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using CaptionMaker.Core.Models;
using Sanet.MVVM.Core.ViewModels;

namespace CaptionMaker.Avalonia.ViewModels;

public class CaptionLineViewModel: BindableBase
{
    private readonly Action<CaptionLineViewModel> _deleteLine;
    private readonly Func<CaptionLineViewModel, string, Task> _insertAfterCallback;

    public CaptionLineViewModel(CaptionLine captionLine, Action<CaptionLineViewModel> deleteLine, Func<CaptionLineViewModel, string, Task> insertAfterCallback)
    {
        _deleteLine = deleteLine;
        _insertAfterCallback = insertAfterCallback;
        CaptionLine = captionLine;
    }
    
    public string Text
    {
        get => CaptionLine.Text;
        set => CaptionLine.Text = value;
    }

    public TimeSpan Start
    {
        get => CaptionLine.Start;
        set => CaptionLine.Start= value;
    }

    public TimeSpan End
    {
        get => CaptionLine.End;
        set => CaptionLine.End = value;
    }
    
    public CaptionLine CaptionLine { get; }
    
    public ICommand DeleteLineCommand => new AsyncCommand(DeleteLine);
    public ICommand InsertAfterCommand => new AsyncCommand<string>(OnInsertAfter);
    
    private Task DeleteLine()
    {
        return Task.Run(() => _deleteLine(this));
    }
    
    private async Task OnInsertAfter(string? srtFilePath)
    {
        if (!string.IsNullOrEmpty(srtFilePath) && File.Exists(srtFilePath))
        {
            await _insertAfterCallback.Invoke(this, srtFilePath);
        }
    }
}