using System.Collections.Generic;
using CaptionMaker.Core.Models;
using Sanet.MVVM.Core.ViewModels;

namespace CaptionMaker.Avalonia.ViewModels;

public class CaptionResultViewModel: BindableBase
{
    private readonly CaptionResult _captionResult;

    public CaptionResultViewModel(CaptionResult captionResult)
    {
        _captionResult = captionResult;

        Captions = new List<CaptionLineViewModel>();
        foreach (var caption in captionResult.Captions)
        {
            Captions.Add(new CaptionLineViewModel(caption));
        }
    }
    
    public string Name => _captionResult.Name;
    public List<CaptionLineViewModel> Captions { get; private set; }
}