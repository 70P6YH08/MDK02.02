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
long totalMs = 0;
int counter = 0;
long averageMs = 0;
int methodId = 0;

Stopwatch stopwatch = new Stopwatch();


//while(counter < 3)
//{
//    stopwatch.Start();
//    await GetNewDirectoryAsync(currentDir);
//    stopwatch.Stop();
//    averageMs += stopwatch.ElapsedMilliseconds;
//    counter++;
//}
//counter = 0;
//totalMs += stopwatch.ElapsedMilliseconds;
//Debug.WriteLine($"Всего файлов в папке {dirPath}: {countFiles}");
//await LogOperationsAsync(logFilePath, nameof(GetNewDirectoryAsync));

//stopwatch.Restart();
//await GetWordsCountAsync(textFilePath);
//counter = 0;
//stopwatch.Stop();
//totalMs += stopwatch.ElapsedMilliseconds;
//Debug.WriteLine($"Количество слов в файле: {countWords}");
//await LogOperationsAsync(logFilePath, nameof(GetWordsCountAsync));

//stopwatch.Restart();
//await GetUsersAsync();
//stopwatch.Stop();
//totalMs += stopwatch.ElapsedMilliseconds;
//Debug.WriteLine($"Вывод завершён");
//await LogOperationsAsync(logFilePath, nameof(GetUsersAsync));

Dictionary<int, Task> methods = new Dictionary<int, Task>()
{
    { 0, Task.Run(async () => GetNewDirectoryAsync(currentDir)) },
    { 1, Task.Run(async () => GetWordsCountAsync(textFilePath)) },
    { 2, Task.Run(async () => GetUsersAsync()) }
};

await ReturnMethodsAsync();


async Task ReturnMethodsAsync()
{
    while(methodId < 3)
    {
        string methodName = "";
        while (counter < 3)
        {
            stopwatch.Start();
            await methods[methodId];
            stopwatch.Stop();

            averageMs += stopwatch.ElapsedMilliseconds;

            switch (methodId)
            {
                case 0:
                    Debug.WriteLine($"Всего файлов в папке {dirPath}: {countFiles}");
                    methodName = nameof(GetNewDirectoryAsync);
                    break;
                case 1:
                    Debug.WriteLine($"Количество слов в файле: {countWords}");
                    methodName = nameof(GetWordsCountAsync);
                    break;
                case 2:
                    Debug.WriteLine($"Вывод завершён");
                    methodName = nameof(GetUsersAsync);
                    break;
            }
            await LogOperationsAsync(logFilePath, methodName);
            counter++;
        }

        File.AppendAllText(logFilePath, $"Среднее время выполнения метода {methodName}: {averageMs / 3}\n");
        totalMs += averageMs;
        averageMs = 0;
        methodId++;
    }

    File.AppendAllText(logFilePath, $"Общее время выполения: {totalMs}\n\n");
}


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
        Debug.WriteLine(ex.Message);
    }
}