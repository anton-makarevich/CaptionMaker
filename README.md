# CaptionMaker

A tool to generate captions from audio files.
Was developed to specifically support Belarusian language.

## Code Structure

There are 3 main parts of the solution (.NET8):

### [CaptionMaker.Core](CaptionMaker.Core)
All the business logic, uses Whisper.net library for speech recognition (large model is required to work with Belarusian language).
Then it can run some postprocessors to improve the caption structure and spelling:
- [CaptionStructuriser](CaptionMaker.Core/Services/CaptionStructuriser) - restructures lines and sentences based on length and punctuations.
- [SpellingChecker](CaptionMaker.Core/Services/SpellingChecker) - is supposed to correct spelling mistakes by using Hunspell, but it doesn't work well as there is no way to pick up the right suggestion.

### [CaptionMaker.Cli](CaptionMaker.Cli)
The CLI application that can be used to generate captions from audio files. It asks for media path and postprocessors to be applied and then generates captions.
Relies on the [CaptionMaker.Core](CaptionMaker.Core) project for actual functionality.

### [CaptionMaker.Avalonia](CaptionMaker.Avalonia)
The GUI application that can be used to generate captions from audio files. Behaves in the same way as the CLI application, but provides a graphical user interface.

## Usage

Both the CLI and GUI applications can be used on any platform (Windows, Mac Linux). But the compiled executable is only provided for Windows (see "Releases").
If you need to run on any other platform, you will need to clone the repo and build it locally using .NET SDK.

Before launching the application, you need to have your audio file ready. For now we only support wav files with 16kHz sample rate. Please use your favorite audio converter (i.e. ffmpeg, Audacity etc) to convert your audio/video file to that format.

### CLI 
Just launch the application and follow the instructions. On first run the app fill need to download the large whisper model which is more than 1GB in size. Depending on your connection it might take some time, please be patient and don't close the app, there is no appropriate indication about the progress yet.
Once the recognition is done, the results will be presented in the console. The raw output from the whisper model will be marked as RAW, if any postprocessors were applied the results will be marked a corresponding processor name.
You will need to review the outputs and select the one you want to be saved as an SRT file. The file will be saved in the same location where the audio file is located just with .srt extension.

### GUI(Avalonia)
It works exactly the same as the CLI application, but it provides a graphical user interface. The first thing you fill need to specify the audio file either with a button/file picker or just by copy/pasting the path into the corresponding field.
Once processed the names of the results will be displayed on the left side of the screen, by selecting the name you can review the captions and save selected captions to an SRT file. 

## Other languages
As stated the initial version only suports recognizing Belarusian language (as there are a lot of tools that work with other languages already). But it will be a configurable parameter in the future versions. For now if you need another language you can just change it in the code (of either CLI or GUI application). Search for "be" in the configuration we pass to the CaptionMaker instance and replace it with the code of your language. First verify if the language is supported by OpenAI Whisper model.

## License
As this project is basically a wrapper around the [Whisper.net](https://github.com/ggerganov/whisper.cpp) library, it is using the same MIT license.


