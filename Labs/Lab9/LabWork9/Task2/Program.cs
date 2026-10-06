using System.Text;

string filePath = "data.txt";
await WriteToFile(filePath, "Some data to be written to the file");

static async Task WriteToFile(string filePath, string data)
{
    using (var writer = new StreamWriter(filePath, true, Encoding.UTF8, 8192))
        await writer.WriteLineAsync(data);
}