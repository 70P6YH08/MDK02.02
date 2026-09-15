try
{
    Console.Write("Введите первое число: ");
    double firstNum = Convert.ToDouble(Console.ReadLine());
    Console.Write("Введите второе число: ");
    double secondNum = Convert.ToDouble(Console.ReadLine());

    Console.WriteLine($"Сложение: {firstNum + secondNum}\n" +
        $"Вычитание: {firstNum - secondNum}\n" +
        $"Умножение: {firstNum * secondNum}\n" +
        $"Деление: {firstNum / secondNum}\n");
}
catch(DivideByZeroException ex)
{
    Console.WriteLine($"{ex.Message} Ошибка");
    LogErrors(ex);
}
catch(FormatException ex)
{
    Console.WriteLine($"{ex.Message} Ошибка");
    LogErrors(ex);
}
catch (OverflowException ex)
{
    Console.WriteLine($"{ex.Message} Ошибка");
    LogErrors(ex);
}
catch(NullReferenceException ex)
{
    Console.WriteLine($"{ex.Message} Ошибка");
    LogErrors(ex);
}
catch(InvalidCastException ex)
{
    Console.WriteLine($"{ex.Message} Ошибка");
    LogErrors(ex);
}
catch(Exception ex)
{
    Console.WriteLine($"{ex.Message} Ошибка");
    LogErrors(ex);
}

static void LogErrors(Exception ex)
{
    string textFilePath = "log.txt";
    File.AppendAllText(textFilePath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {ex}\n\n");
}