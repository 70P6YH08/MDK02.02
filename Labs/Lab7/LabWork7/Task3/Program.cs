using System.Diagnostics;

var traceSource = new TraceSource("Calculator")
{
    Switch = new SourceSwitch("CalculatorSwitch")
    {
        Level = SourceLevels.Verbose
    }
};

traceSource.Listeners.Clear();

try
{

    //"trace.log"
    traceSource.Listeners.Add(new TextWriterTraceListener("trace_verbose.log", "fileListener"));
    traceSource.Listeners.Add(new ConsoleTraceListener());

    traceSource.TraceInformation("Приложение запущено");

    traceSource.TraceEvent(
        TraceEventType.Verbose,
        1,
        "Требуется ввести первое число: ");
    var firstNum = Console.ReadLine();
    var fn = NumberInput(firstNum);

    traceSource.TraceEvent(
        TraceEventType.Verbose,
        1,
        "Требуется ввести второе число: ");
    var secondNum = Console.ReadLine();
    var sn = NumberInput(secondNum);

    traceSource.TraceEvent(
        TraceEventType.Information,
        0,
        "Выполнение операций");

    string results = $"\tСложение: {fn + sn}\n" +
        $"\tВычитание: {fn - sn}\n" +
        $"\tУмножение: {fn * sn}\n" +
        $"\tДеление: {fn / sn}\n";

    traceSource.TraceEvent(
        TraceEventType.Information,
        0,
        "Результаты вычислений:\n{0}",
        results);

    traceSource.Flush();
    traceSource.Close();
}
catch (Exception ex)
{
    traceSource.TraceEvent(TraceEventType.Error, 3, "Ошибка, {0}", ex);
}

int NumberInput(string? num)
{
    if (int.TryParse(num, out int number))
    {

        traceSource.TraceEvent(
            TraceEventType.Information,
            0,
            "Успешно введено число: {0}",
            number);
    }
    else
        traceSource.TraceEvent(
            TraceEventType.Warning,
            3,
            "Пользователь ввёл некорректное число: {0}",
            num);

    return number;
}
