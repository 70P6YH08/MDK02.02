using NLog;

var logger = LogManager.GetCurrentClassLogger();
var config = new NLog.Config.LoggingConfiguration();
var targetFile = new NLog.Targets.FileTarget();
targetFile.FileName = "errors.log";

config.AddRule(LogLevel.Error, LogLevel.Fatal, targetFile);
LogManager.Configuration = config;

try
{
    Console.Write("Введите первое число: ");
    var firstNum = Convert.ToInt32(Console.ReadLine());
    Console.Write("Введите второе число: ");
    var secondNum = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine($"Результат {firstNum / secondNum}");
}
catch (DivideByZeroException ex)
{
    logger.Error(ex.Message, "Деление на 0");
    Console.WriteLine("Деление на 0!");
}
catch (FormatException ex)
{
    logger.Error(ex.Message, "Введён текст вместо числа");
    Console.WriteLine("Введён текст вместо числа!");
}
catch (Exception ex)
{
    logger.Error(ex.Message, "Ошибка");
    Console.WriteLine(ex.StackTrace);
}