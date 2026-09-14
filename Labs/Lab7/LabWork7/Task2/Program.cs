using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

string dirPath = @"C:\Program Files";
DirectoryInfo currentDir = new DirectoryInfo(dirPath);
int countFiles = 0;

string textFilePath = "Dostoevsky.txt";
int countWords = 0;

string logFilePath = @"timings.log";
long totalMs = 0;

Stopwatch stopwatch = new Stopwatch();


stopwatch.Start();
await GetNewDirectoryAsync(currentDir);
stopwatch.Stop();
totalMs += stopwatch.ElapsedMilliseconds;
Console.WriteLine($"Всего файлов в папке {dirPath}: {countFiles}");
await LogOperationsAsync(logFilePath, nameof(GetNewDirectoryAsync));

stopwatch.Restart();
await GetWordsCountAsync(textFilePath, countWords);
stopwatch.Stop();
totalMs += stopwatch.ElapsedMilliseconds;
Console.WriteLine($"Количество слов в файле: {countWords}");
await LogOperationsAsync(logFilePath, nameof(GetWordsCountAsync));

stopwatch.Restart();
await GetUsersAsync();
stopwatch.Stop();
totalMs += stopwatch.ElapsedMilliseconds;
Console.WriteLine($"Вывод завершён");
await LogOperationsAsync(logFilePath, nameof(GetUsersAsync));
File.AppendAllText(logFilePath, $"Общее время выполения: {totalMs}\n\n");

async Task LogOperationsAsync(string logFilePath, string methodName) =>
    File.AppendAllText(logFilePath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Operation={methodName}," +
    $"Elapsed={stopwatch.ElapsedMilliseconds}\n");


async Task GetUsersAsync()
{
    string baseUrl = "https://catfact.ninja";
    string endpoint = "/fact";

    using (HttpClient client = new HttpClient()
    {
        BaseAddress = new Uri(baseUrl)
    })
    {
        HttpResponseMessage response = await client.GetAsync(endpoint);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var cat = JsonSerializer.Deserialize<Cat>(jsonResponse);

            if (cat == null)
                return;

            Console.WriteLine($"{cat.Fact} - {cat.Length}");
        }
        else
            Console.WriteLine($"Ошибка: {response.StatusCode}");
    }
}



static async Task GetWordsCountAsync(string filePath, int countWords)
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
}


async Task GetNewDirectoryAsync(DirectoryInfo currentDir)
{
    try
    {
        FileInfo[] files = currentDir.GetFiles();

        foreach (var file in files)
            countFiles++;

        DirectoryInfo[] subDirs = currentDir.GetDirectories();

        foreach (var subDir in subDirs)
            await GetNewDirectoryAsync(subDir);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
public class Cat
{
    [JsonPropertyName("fact")]
    public string Fact { get; set; } = null!;

    [JsonPropertyName("length")]
    public int Length { get; set; }
}