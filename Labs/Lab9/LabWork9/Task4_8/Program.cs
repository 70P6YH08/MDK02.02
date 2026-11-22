using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Task4_8.Context;
using Task4_8.Models;
using Task4_8.Services;

class Program
{
    static async Task Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddDbContext<AppDbContext>();
        services.AddMemoryCache();
        services.AddScoped<UserService>();

        var serviceProvider = services.BuildServiceProvider();

        // Инициализация данных и сервисов
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userService = scope.ServiceProvider.GetRequiredService<UserService>();

        // Добавление пользователей
        var usersToAdd = new List<User>
            {
                new User { Name = "Alice", Email = "alice@example.com", IsActive = true },
                new User { Name = "Bob", Email = "bob@example.com", IsActive = false },
                new User { Name = "Charlie", Email = "charlie@example.com", IsActive = true },
                new User { Name = "Kavin", Email = "kavin@example.com", IsActive = false },
                new User { Name = "Chloe", Email = "chloe@example.com", IsActive = true },
                new User { Name = "Ben", Email = "ben@example.com", IsActive = false },
                new User { Name = "Milena", Email = "milena@example.com", IsActive = true }
            };

        await userService.AddUsersAsync(usersToAdd);

        // Получение активных пользователей
        var activeUsers = await userService.GetActiveUsersAsync();
        Console.WriteLine("Active Users:");
        foreach (var user in activeUsers)
        {
            Console.WriteLine($"{user.Name} - {user.Email}");
        }

        // Получение пользователей и их заказов
        var usersWithOrders = await userService.GetUserDtosWithOrdersAsync();
        Console.WriteLine("Users with Orders:");
        foreach (var user in usersWithOrders)
        {
            Console.WriteLine($"{user.UserName} - Orders: {user.OrderCount}");
        }

        //Тестирования метода кэширования
        Console.WriteLine("\nFirst call:\n");
        var cachedUsers = await userService.GetCachedActiveUsersAsync();

        Console.WriteLine();

        Console.WriteLine("\nSecond call:\n");
        cachedUsers = await userService.GetCachedActiveUsersAsync();

        Console.WriteLine();

        var secondUsersToAdd = new List<User>
            {
                new User { Name = "Mom", Email = "mom@example.com", IsActive = true },
                new User { Name = "Dad", Email = "dad@example.com", IsActive = true },
            };

        await userService.AddUsersAsync(secondUsersToAdd);

        Console.WriteLine("\nThird call:\n");
        cachedUsers = await userService.GetCachedActiveUsersAsync();

        Console.WriteLine();
    }
}