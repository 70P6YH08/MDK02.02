
Dictionary<int, int> _fibonacciCache = new();

Console.WriteLine("Fibonacci Sequence:");

for (int i = 0; i < 20; i++)
    Console.WriteLine($"Fib({i}) = {Fibonacci(i)}");

int Fibonacci(int n)
{
    if (_fibonacciCache.ContainsKey(n))
        return _fibonacciCache[n];

    int result = (n <= 1)
        ? n
        : Fibonacci(n - 1) + Fibonacci(n - 2);

    _fibonacciCache[n] = result;
    return result;
}