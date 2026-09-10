try
{
    //, new FileStreamOptions() { Mode = FileMode.Open }
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
    };
}
catch (FileNotFoundException ex)
{
    Console.WriteLine($"Файл не найден: {ex.Message}");
}
