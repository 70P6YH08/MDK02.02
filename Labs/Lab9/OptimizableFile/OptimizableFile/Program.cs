
string filePath = "data.txt";
//ReadFromFile(filePath);
//void ReadFromFile(string filePath)
//{
//    if (!File.Exists(filePath))
//    {
//        Console.WriteLine("File not found.");
//        return;
//    }
//    using (var reader = new StreamReader(filePath))
//    {
//        string line;
//        while ((line = reader.ReadLine()) != null)
//        {
//            Console.WriteLine(line);
//        }
//    }
//}

//WriteToFile(filePath, "Some data to be written to the file");
//void WriteToFile(string filePath, string data)
//{
//    using (var writer = new StreamWriter(filePath, true))
//    {
//        foreach (char c in data)
//        {
//            writer.Write(c);
//        }
//    }
//}

//Console.WriteLine("Fibonacci Sequence:");

//for (int i = 0; i < 20; i++)
//{
//    Console.WriteLine($"Fib({i}) = {Fibonacci(i)}");
//}
//int Fibonacci(int n)
//{
//    if (n <= 1)
//        return n;
//    return Fibonacci(n - 1) + Fibonacci(n - 2);
//}

