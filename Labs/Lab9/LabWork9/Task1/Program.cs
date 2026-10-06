using System.Text;

string filePath = "data.txt";
await ReadFromFile(filePath);

static async Task ReadFromFile(string filePath)
{
    if (!File.Exists(filePath))
    {
        Console.WriteLine("File not found.");
        return;
    }

    using (var reader = new StreamReader(filePath, Encoding.UTF8, true, 8192))
    {
        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
            Console.WriteLine(line);
    }
}
