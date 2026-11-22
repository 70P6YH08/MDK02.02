using System;

public class Class1
{
    string filePath = "data.txt";
    project.ReadFromFile(filePath);  
    // Нерациональный метод для чтения из файла
    public void ReadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("File not found.");
            return;
        }
        // Оптимизация: добавить в конструктор параметр bufferSize: 8192
        // для увеличения размера буфера
        using (var reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                // Оптимизация: использовать асинхронные операции
                Console.WriteLine(line);
            }
        }
    }
}
