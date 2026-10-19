
using Microsoft.Extensions.Logging;

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
    double firstN = 1;
    double secondN = 0;
    try
    {
        double result = firstN / secondN;
    }
    catch(DivideByZeroException ex)
    {
        string log = ex.StackTrace;
        string logFilePath = "log.txt";
        File.AppendAllText(logFilePath, log);
    }
}