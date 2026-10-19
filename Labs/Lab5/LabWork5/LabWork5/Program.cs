using System.Diagnostics;

try
{
    string firstNumber = "";
    string secondNumber = "";

    while (firstNumber != "exit" || secondNumber != "exit")
    {
        Debug.WriteLine("1 num Debug");
        Trace.WriteLine("1 num Trace");
        Console.Write("Введите первое число: ");
        firstNumber = Console.ReadLine();

        Debug.WriteLine("2 num Debug");
        Trace.WriteLine("2 num Trace");
        Console.Write("Введите второе число: ");
        secondNumber = Console.ReadLine();

        if (string.IsNullOrEmpty(firstNumber) ||
            string.IsNullOrEmpty(secondNumber))
            continue;

        int result = Convert.ToInt32(firstNumber)
            + Convert.ToInt32(secondNumber);

        Debug.WriteLine("result Debug\n");
        Trace.WriteLine("result Trace\n");
        Console.WriteLine($"Результат: {result}");
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}