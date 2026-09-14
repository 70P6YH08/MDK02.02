using System.Diagnostics;
using System.Text.RegularExpressions;

string dirPath = @"C:\Program Files";
DirectoryInfo currentDir = new DirectoryInfo(dirPath);
int countFiles = 0;

string textFilePath = "Dostoevsky.txt";
int countWords = 0;

string logFilePath = @"timings.log";

Stopwatch stopwatch = new Stopwatch();


stopwatch.Start();
GetNewDirectory(currentDir);
Console.WriteLine($"Всего файлов в папке {dirPath}: {countFiles}");
stopwatch.Stop();
LogOperations(logFilePath, nameof(GetNewDirectory));

stopwatch.Start();
ReadWordsCount(textFilePath, countWords);
stopwatch.Stop();

LogOperations(logFilePath, nameof(ReadWordsCount));

void LogOperations(string logFilePath, string methodName) =>
    File.AppendAllText(logFilePath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Operation={methodName}," +
    $"Elapsed={stopwatch.ElapsedMilliseconds}\n\n");


static void ReadWordsCount(string filePath, int countWords)
{
    if (!File.Exists(filePath))
    {
        Console.WriteLine("Файл не найден.");
        return;
    }

    using (StreamReader reader = new StreamReader(filePath))
    {
        string line;

        while ((line = reader.ReadLine()) != null)
        {
            string[] words = Regex.Split(line, @"\W+");

            foreach (string word in words)
                if (Regex.IsMatch(word, @"^[а-яА-Я]+$"))
                    countWords++;
        }
    }
    Console.WriteLine($"Количество слов в файле: {countWords}");
}


void GetNewDirectory(DirectoryInfo currentDir)
{
    try
    {
        FileInfo[] files = currentDir.GetFiles();

        foreach (var file in files)
            countFiles++;

        DirectoryInfo[] subDirs = currentDir.GetDirectories();

        foreach (var subDir in subDirs)
            GetNewDirectory(subDir);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}