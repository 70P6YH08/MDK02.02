
using System.Diagnostics;

int result = 0;
try
{
    do
    {
        Debug.WriteLine("1 Debug");
        Trace.WriteLine("1 Trace");
        Console.Write("Введите первое число: ");
        var firstNumber = Console.ReadLine();
        if (firstNumber == null)
            continue;
        else if (firstNumber == "exit")
            break;

        Debug.WriteLine("2 Debug");
        Trace.WriteLine("2 Trace");
        Console.Write("Введите второе число: ");
        var secondNumber = Console.ReadLine();
        if (firstNumber == null)
            continue;
        else if (firstNumber == "exit")
            break;

        result = Convert.ToInt32(firstNumber) + Convert.ToInt32(secondNumber);
        
        Debug.WriteLine("result Debug");
        Trace.WriteLine("result Trace");
        Console.WriteLine($"Результат: {result}");

    } while (true);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}