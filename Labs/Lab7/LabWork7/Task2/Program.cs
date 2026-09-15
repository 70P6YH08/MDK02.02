using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using Task2;

string dirPath = @"C:\Program Files";
DirectoryInfo currentDir = new DirectoryInfo(dirPath);
int countFiles = 0;

string textFilePath = "Dostoevsky.txt";
int countWords = 0;

string logFilePath = @"timings.log";

Stopwatch stopwatch = new Stopwatch();

Dictionary<int, Func<Task>> methods = new Dictionary<int, Func<Task>>()
{
    { 0, () => GetAllFilesByDirectory(currentDir) },
    { 1, () => GetWordsCountAsync(textFilePath) },
    { 2, () => GetCatAsync() }
};

await ReturnMethodsAsync();


async Task ReturnMethodsAsync()
{
    int methodId = 0;

    double totalMs = 0;
    while (methodId < methods.Count)
    {
        double averageMs = 0;
        int counter = 0;
        string methodName = "";
        while (counter < 3)
        {
            stopwatch.Restart();
            await methods[methodId]();
            stopwatch.Stop();

            averageMs += stopwatch.ElapsedMilliseconds;

            switch (methodId)
            {
                case 0:
                    Debug.WriteLine($"Всего файлов в папке {dirPath}: {countFiles}");
                    methodName = nameof(GetAllFilesByDirectory);
                    break;
                case 1:
                    Debug.WriteLine($"Количество слов в файле: {countWords}");
                    methodName = nameof(GetWordsCountAsync);
                    break;
                case 2:
                    Debug.WriteLine($"Вывод завершён");
                    methodName = nameof(GetCatAsync);
                    break;
            }
            await LogOperationsAsync(logFilePath, methodName);
            counter++;
        }

        File.AppendAllText(logFilePath, $"Среднее время выполнения метода {methodName}: {Math.Round(averageMs / 3, 2)}\n");
        totalMs += averageMs;
        methodId++;
    }

    File.AppendAllText(logFilePath, $"Общее время выполения: {totalMs}\n\n");
}


async Task LogOperationsAsync(string logFilePath, string methodName) =>
    File.AppendAllText(logFilePath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Operation={methodName}," +
    $"Elapsed={stopwatch.ElapsedMilliseconds}\n");

async Task GetCatAsync()
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

            Debug.WriteLine($"{cat.Fact} - {cat.Length}");
        }
        else
            Debug.WriteLine($"Ошибка: {response.StatusCode}");
    }
}



async Task GetWordsCountAsync(string filePath)
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


async Task GetAllFilesByDirectory(DirectoryInfo currentDir)
{
    try
    {
        FileInfo[] files = currentDir.GetFiles();

        foreach (var file in files)
            countFiles++;

        DirectoryInfo[] subDirs = currentDir.GetDirectories();

        foreach (var subDir in subDirs)
            await GetAllFilesByDirectory(subDir);
    }
    catch (Exception ex)
    {
        Debug.WriteLine(ex.Message);
    }
}