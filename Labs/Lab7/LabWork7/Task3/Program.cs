using System.Diagnostics;

try
{
    var traceSource = new TraceSource("Calculator")
    {
        Switch = new SourceSwitch("CalculatorSwitch")
        {
            Level = SourceLevels.Information
        }
    };

    //"trace_verbose.log"
    traceSource.Listeners.Add(new TextWriterTraceListener("trace.log", "fileListener"));
    traceSource.Listeners.Add(new ConsoleTraceListener());

    Console.Write("Введите первое число: ");
    double firstNum = Convert.ToDouble(Console.ReadLine());
    Console.Write("Введите второе число: ");
    double secondNum = Convert.ToDouble(Console.ReadLine());

    Console.WriteLine($"Сложение: {firstNum + secondNum}\n" +
        $"Вычитание: {firstNum - secondNum}\n" +
        $"Умножение: {firstNum * secondNum}\n" +
        $"Деление: {firstNum / secondNum}\n");

    traceSource.TraceInformation("информация");
    traceSource.TraceEvent(TraceEventType.Verbose, 1, "подробности");
    traceSource.TraceEvent(TraceEventType.Warning, 2, "предупреждение");
    traceSource.TraceEvent(TraceEventType.Error, 3, "ошибка");

    traceSource.Flush();
    traceSource.Close();
}
catch (Exception ex)
{
    Console.WriteLine($"{ex.Message} Ошибка");
}

//static void LogErrors(Exception ex)
//{
//    string textFilePath = "log.txt";
//    File.AppendAllText(textFilePath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {ex.ToString()}\n\n");
//}