
static void CheckNumber(double number)
{
    if (number < 0)
        throw new NegativeNumberException("Число отрицательное!");
}

try
{
    CheckNumber(-5);
    Console.WriteLine("Всё нормально");
}
catch (NegativeNumberException ex)
{
    Console.WriteLine($"Ошибка, {ex.Message}");
}
catch(Exception ex)
{
    Console.WriteLine($"Ошибка, {ex.StackTrace}");
}