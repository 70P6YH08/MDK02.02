MethodA();
void MethodA()
{
    MethodB();
}
void MethodB()
{
    MethodC();
}
void MethodC()
{
    int firstN = int.Parse(Console.ReadLine()!);
    int secondN = int.Parse(Console.ReadLine()!);
    try
    {
        int result = firstN / secondN;
    }
    catch(DivideByZeroException ex)
    {
        string log = $"{ex.Message} -{ex.StackTrace}" +
            $"[{DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy")}]\n\n";
        string logFilePath = "log.txt";
        File.AppendAllText(logFilePath, log);
    }
}