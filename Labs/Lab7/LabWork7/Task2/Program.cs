using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;
using Task2;

//Method 3
string dirPath = @"C:\Users\gorbu\Desktop\All\Альбомы";
DirectoryInfo currentDir = new DirectoryInfo(dirPath);
int countFiles = 0;

//Method 1
string textFilePath = "Dostoevsky.txt";
int countWords = 0;

string logFilePath = @"timings.log";

Stopwatch stopwatch = new Stopwatch();

Dictionary<int, (string methodName, Func<Task> method, Action debug)> methods = new()
{
    {
        0,
        (
            nameof(GetAllFilesByDirectory),
            () => GetAllFilesByDirectory(currentDir),
            () =>
            {
                Debug.WriteLine($"Всего файлов в папке {dirPath}: {countFiles}");
                countFiles = 0;
            }
        )
    },
    {
        1,
        (
            nameof(GetWordsCountAsync),
            () => GetWordsCountAsync(textFilePath),
            () =>
            {
                Debug.WriteLine($"Количество слов в файле: {countWords}");
                countWords = 0;
            }
        )
    },
    {
        2,
        (
            nameof(GetCatAsync),
            () => GetCatAsync(),
            () => Debug.WriteLine($"Вывод котиков завершён")
        )
    }
};

await ReturnMethodsAsync();

async Task ReturnMethodsAsync()
{
    double totalMs = 0;
    string methodName = "";
    for (int i = 0; i < methods.Count; i++)
    {
        var currentMethod = methods[i];

        long ms = 0;
        double methodTotalMs = 0;
        methodName = currentMethod.methodName;

        for (int j = 0; j < 3; j++)
        {
            stopwatch.Restart();
            await currentMethod.method();
            stopwatch.Stop();

            ms = stopwatch.ElapsedMilliseconds;
            methodTotalMs += ms;

            currentMethod.debug();

            await DataLogger.LogOperationsAsync(logFilePath, methodName, ms);
        }
        double average = methodTotalMs / 3;

        File.AppendAllText(logFilePath, $"Среднее время выполнения метода {methodName}: {Math.Round(average, 2)}\n");
        totalMs += methodTotalMs;
    }

    File.AppendAllText(logFilePath, $"Общее время выполения: {totalMs}\n\n");
}

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
            var jsonResponse = await response.Content.ReadAsStreamAsync();
            var cat = await JsonSerializer.DeserializeAsync<Cat>(jsonResponse);

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
        string? line;
        string[] words;

        while ((line = await reader.ReadLineAsync()) != null)
        {
            words = Regex.Split(line, @"\W+");

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