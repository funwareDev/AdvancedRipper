//args are passed C:\AppName arg1 arg2 arg3

using System.Diagnostics;

//todo to rework it to better arg reader
var pathToFirstFolder = args[0];
var pathToSecondFolder = args[1];
var outputFolder = args[2];
var desiredNameFormat = args[3];
var startOfFileSeconds = args.Length >= 5 ? args[4] : null;
var endOfFileSeconds = args.Length >= 6 ? args[5] : null;

// %0 - video
var extractAudioArgs = "-i \"{0}\" -vn -acodec copy output-audio.aac";

// %0 - audio
var cutAudioArgs = "-ss {0} -i output-audio.aac cut-output-audio.aac";

// %0 - vide0 // %1 - audio // %2 - output
var combineAudioWithVideoArgs = "-i \"{0}\" -i {1} -c copy -map 0:v:0 -map 1:a:0 {2}";

string[] filesInFirstFolder = Directory.GetFiles(pathToFirstFolder);
string[] filesInSecondFolder = Directory.GetFiles(pathToSecondFolder);
string[] filesInOutputFolder = Directory.GetFiles(outputFolder);

Console.WriteLine($"AdvancedRipper. Made with <3 by funwareDev");
Console.WriteLine($"There are {filesInFirstFolder.Length} files in audio folder.");
Console.WriteLine($"There are {filesInSecondFolder.Length} files in video folder.");
Console.WriteLine($"There are {filesInOutputFolder.Length} files ready in output folder.");

for (var index = 0; index < filesInFirstFolder.Length; index++)
{
    File.Delete("cut-output-audio.aac");
    File.Delete("output-audio.aac");
    
    var newName = string.Format(desiredNameFormat, index + 1);
    newName += Path.GetExtension(filesInFirstFolder[index]);
    var path = Path.Combine(outputFolder, newName);

    //if file was previously generated, we do not want to make this again
    if (path.Equals(filesInOutputFolder[index]))
    {
        Console.WriteLine("File exists, skip");
        continue;
    }
    
    ExtractAudio(filesInFirstFolder[index]);
    CombineVideoAndAudio(filesInSecondFolder[index], path);
}

Console.WriteLine($"Now we are done.");

void ExtractAudio(string filename)
{
    var extractAudioProcess = Process.Start("ffmpeg.exe", string.Format(extractAudioArgs, filename));
    extractAudioProcess.WaitForExit();

    if (startOfFileSeconds == null)
    {
        return;
    }
    
    var cutAudioProcess = Process.Start("ffmpeg.exe", string.Format(cutAudioArgs, startOfFileSeconds));
    cutAudioProcess.WaitForExit();    
    File.Delete("output-audio.aac");
    File.Move("cut-output-audio.aac", "output-audio.aac");
}

void CombineVideoAndAudio(string fileWithVideo, string outputPath)
{
    var newArgs = string.Format(combineAudioWithVideoArgs, fileWithVideo, "output-audio.aac", outputPath);
    var processOfCombiningAudioWithVideo = Process.Start("ffmpeg.exe", newArgs);
    processOfCombiningAudioWithVideo.WaitForExit();
}

void RenameFile(string filePath, string folderPath)
{
    // Отримати ім'я файлу без шляху
    string fileName = Path.GetFileName(filePath);
    
    
    // Ваша логіка для нового імені файлу (тут я використовую приклад і додаю "_new" до кожного імені)
    string newFileName = Path.GetFileNameWithoutExtension(fileName) + "_new" + Path.GetExtension(fileName);

    // Формування нового шляху до файлу
    string newFilePath = Path.Combine(folderPath, newFileName);

    // Перейменування файлу
    File.Move(filePath, newFilePath);
}

