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

Console.Write("\n\n\n");
Console.WriteLine(
    @"              _                               _ _____  _                       
     /\      | |                             | |  __ \(_)                      
    /  \   __| |_   ____ _ _ __   ___ ___  __| | |__) |_ _ __  _ __   ___ _ __ 
   / /\ \ / _` \ \ / / _` | '_ \ / __/ _ \/ _` |  _  /| | '_ \| '_ \ / _ \ '__|
  / ____ \ (_| |\ V / (_| | | | | (_|  __/ (_| | | \ \| | |_) | |_) |  __/ |   
 /_/    \_\__,_| \_/ \__,_|_| |_|\___\___|\__,_|_|  \_\_| .__/| .__/ \___|_|   
                                                        | |   | |              
                                                        |_|   |_|              ");
Console.Write("\n\n\n");


Console.WriteLine($"There are {filesInFirstFolder.Length} files in audio folder.");
Console.WriteLine($"There are {filesInSecondFolder.Length} files in video folder.");

for (var index = 0; index < filesInFirstFolder.Length; index++)
{
    File.Delete("cut-output-audio.aac");
    File.Delete("output-audio.aac");
    
    var newName = string.Format(desiredNameFormat, index + 1);
    newName += Path.GetExtension(filesInFirstFolder[index]);
    var path = Path.Combine(outputFolder, newName);

    //if file was previously generated, we do not want to make this again
    if (filesInOutputFolder.Contains(path))
    {
        Console.WriteLine($"{newName} is done.");
        continue;
    }
    
    ExtractAudio(filesInFirstFolder[index]);
    CombineVideoAndAudio(filesInSecondFolder[index], path);
    Console.WriteLine($"{newName} is done.");
}

Console.WriteLine($"Now we are done.");

void ExtractAudio(string filename)
{
    var process = FfmpegProcessInNewConsole(string.Format(extractAudioArgs, filename));

    process.Start();
    process.WaitForExit();

    if (startOfFileSeconds == null)
    {
        return;
    }
    
    var cutAudioProcess = FfmpegProcessInNewConsole(string.Format(cutAudioArgs, startOfFileSeconds));
    cutAudioProcess.Start();
    cutAudioProcess.WaitForExit();
    
    File.Delete("output-audio.aac");
    File.Move("cut-output-audio.aac", "output-audio.aac");
}

void CombineVideoAndAudio(string fileWithVideo, string outputPath)
{
    var newArgs = string.Format(combineAudioWithVideoArgs, fileWithVideo, "output-audio.aac", outputPath);
    var processOfCombiningAudioWithVideo = FfmpegProcessInNewConsole(newArgs);
    processOfCombiningAudioWithVideo.Start();
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

Process FfmpegProcessInNewConsole(string ffmpegArguments)
{
    var process = new Process();
    process.StartInfo.FileName = "ffmpeg.exe";
    process.StartInfo.Arguments = ffmpegArguments;
    process.StartInfo.UseShellExecute = true;
    return process;
}

