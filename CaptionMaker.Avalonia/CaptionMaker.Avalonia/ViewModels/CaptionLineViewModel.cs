using System;
using CaptionMaker.Core.Models;
using Sanet.MVVM.Core.ViewModels;

namespace CaptionMaker.Avalonia.ViewModels;

public class CaptionLineViewModel: BindableBase
{
    public CaptionLineViewModel(CaptionLine captionLine)
    {
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
}