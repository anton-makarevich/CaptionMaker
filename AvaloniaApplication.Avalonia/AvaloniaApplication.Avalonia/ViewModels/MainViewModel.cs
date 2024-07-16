﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AsyncAwaitBestPractices.MVVM;
using CaptionMaker.Core.Models;
using CaptionMaker.Core.Services;
using Sanet.MVVM.Core.ViewModels;
using Whisper.net.Ggml;

namespace AvaloniaApplication.Avalonia.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly CaptionMaker.Core.CaptionMaker _captionMaker;
    private readonly SrtWriter _srtWriter;
    private string _mediaFile="";
    private bool _isStructorizerActive=true;
    private bool _isSpellCheckerActive=false;
    private CaptionResult? _selectedCaptionResult;

    public MainViewModel(CaptionMaker.Core.CaptionMaker captionMaker, SrtWriter srtWriter)
    {
        _captionMaker = captionMaker;
        _srtWriter = srtWriter;
    }

    public string MediaFile
    {
        get => _mediaFile;
        set
        {
            SetProperty(ref _mediaFile, value);
            NotifyPropertyChanged(nameof(CanProcess));
        }
    }

    public bool IsStructorizerActive
    {
        get => _isStructorizerActive;
        set => SetProperty(ref _isStructorizerActive, value);
    }
    
    public bool IsSpellCheckerActive
    {
        get => _isSpellCheckerActive;
        set => SetProperty(ref _isSpellCheckerActive, value);
    }

    public List<CaptionResult> ProcessedCaptions => _captionMaker.ProcessedCaptions;

    public CaptionResult? SelectedCaptionResult
    {
        get => _selectedCaptionResult;
        set
        {
            SetProperty(ref _selectedCaptionResult, value);
            NotifyPropertyChanged(nameof(HasSelectedCaption));
        }
    }

    public bool HasSelectedCaption => SelectedCaptionResult != null;

    public bool CanProcess => !string.IsNullOrWhiteSpace(MediaFile) && File.Exists(MediaFile);
    
    public IAsyncCommand ProcessMediaFileCommand => new AsyncCommand(ProcessMediaFile);
    
    private async Task ProcessMediaFile()
    {
        IsBusy = true;
        var parameters = new CaptionParameters
        {
            AudioFilePath = MediaFile,
            Language = "be",
            ModelType = GgmlType.LargeV2
        };
        var postProcessors = new List<ICaptionsPostProcessor>();
        if (IsStructorizerActive)
        {
            postProcessors.Add(new CaptionStructuriser());
        }
        if (IsSpellCheckerActive)
        {
            postProcessors.Add(new SpellingChecker());
            
        }
        var results =await _captionMaker.CreateCaption(parameters, postProcessors);
        NotifyPropertyChanged(nameof(ProcessedCaptions));
        IsBusy = false;
    }
    
    public IAsyncCommand WriteSrtCommand => new AsyncCommand(WriteSrt);
    
    private async Task WriteSrt()
    {
        var filePath = Path.ChangeExtension(MediaFile, ".srt");
        await _srtWriter.WriteSrt(filePath, SelectedCaptionResult.Captions);
    }
}