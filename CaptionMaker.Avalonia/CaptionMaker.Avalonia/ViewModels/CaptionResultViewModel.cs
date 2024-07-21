using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CaptionMaker.Core.Models;
using Sanet.MVVM.Core.ViewModels;

namespace CaptionMaker.Avalonia.ViewModels;

public class CaptionResultViewModel: BindableBase
{
    private readonly CaptionResult _captionResult;

    public CaptionResultViewModel(CaptionResult captionResult)
    {
        _captionResult = captionResult;

        Captions = new ObservableCollection<CaptionLineViewModel>();
        foreach (var caption in captionResult.Captions)
        {
            Captions.Add(new CaptionLineViewModel(caption, DeleteLine, InsertLineAfter));
        }
    }

    private async Task InsertLineAfter(CaptionLineViewModel captionLine, string srtFilePath)
    {
        var adjustment = captionLine.End; // Adjust according to the current line's duration
        var newCaptions = await CaptionFactory.CreateFromSrt(srtFilePath, adjustment);

        var index = Captions.IndexOf(captionLine);
        foreach (var newCaption in newCaptions)
        {
            var newCaptionViewModel = new CaptionLineViewModel(newCaption, DeleteLine, InsertLineAfter);
            Captions.Insert(++index, newCaptionViewModel);
        }
    }

    public string Name => _captionResult.Name;
    public ObservableCollection<CaptionLineViewModel> Captions { get; private set; }
    
    private void DeleteLine(CaptionLineViewModel captionLine)
    {
        Captions.Remove(captionLine);
        //NotifyPropertyChanged(nameof(Captions));
    }
}