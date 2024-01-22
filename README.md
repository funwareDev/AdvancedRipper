# AdvancedRipper

## Purpose
Supposingly, you have folder with videos, and you want to rip its audio and subs and combine them with videos in another folder. Well, this shitty app is just for you.

## Dependencies
- .NET 7
- ffmpeg 

## Usage
Execute in cmd or powershell:

`AdvancedRipper.exe pathToFirstFolder pathToSecondFolder outputFolder desiredNameFormat`

## Args

### Essential:
**pathToFirstFolder** - folder with your audio videos;

**pathToSecondFolder** - folder with videos you wanna add audio;

**outputFolder** - there we meet;

**desiredNameFormat** - the name format of resulted files. Example: ```ShittyAnime_{0}```, where `{0}` will be replaced by episode number. File extension will be added automatically.


### Optional (enought self-explanatory):
**startOfFileSeconds**;

**endOfFileSeconds**;

## Todo
- [x] readme
- [ ] maybe other audio formats should be supported (now only aac)
- [ ] more flexible, like merge already prepared audio files
- [ ] async - should be far more faster
- [ ] better argument parser - https://github.com/fschwiet/ManyConsole/ or this could be used instead https://learn.microsoft.com/en-us/archive/msdn-magazine/2019/march/net-parse-the-command-line-with-system-commandline




