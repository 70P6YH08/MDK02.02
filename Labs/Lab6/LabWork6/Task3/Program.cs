AppDomain.CurrentDomain.UnhandledException += ExceptionsRecord_UnhandledException;

try
{
    using (var fileReader = new StreamReader("textTask3.txt"))
    {
        string? line;
        while ((line = fileReader.ReadLine()) != null)
        {
            int counter = 0;
            while (true)
            {
                if (counter == line.Length)
                    break;

                if (char.IsDigit(line[counter]))
                {
                    int number = Convert.ToInt32(line[counter]);
                    if (number % 2 == 0)
                        Console.Write(line[counter] + " ");
                }
                counter++;
            }
        }
        Console.WriteLine();
    };
}
catch (FileNotFoundException ex)
{
    Console.WriteLine($"Файл не найден. {ex.Message}");
}

throw new Exception("Это ошибка на фоновом потоке");

void ExceptionsRecord_UnhandledException(object sender, UnhandledExceptionEventArgs e)
{
    string crushFilePath = "crush.log";
    Console.WriteLine("Произошла ошибка. Подробности в логах.");

    using(var streamWriter = new StreamWriter(crushFilePath, true))
    {
        streamWriter.WriteLine($"{e.ExceptionObject.ToString()}\n\t" +
            $"[{DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy")}]\n");
    }
}
