
static void CheckNumber()
{
    var number = Convert.ToInt32(Console.ReadLine());
    if (number < 0)
        throw new NegativeNumberException("число отрицательное!");
}

try
{
    CheckNumber();
    Console.WriteLine("Всё нормально");
}
catch (NegativeNumberException ex)
{
    Console.WriteLine($"Ошибка: {ex.Message}");
}
catch(Exception ex)
{
    Console.WriteLine($"Ошибка: {ex.StackTrace}");
}
