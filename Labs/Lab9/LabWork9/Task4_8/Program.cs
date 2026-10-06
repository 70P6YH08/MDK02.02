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
                new User { Name = "Charlie", Email = "charlie@example.com", IsActive = true }
            };

        await userService.AddUsersAsync(usersToAdd);

        var usersToAdd1 = new List<User>
            {
                new User { Name = "Alice", Email = "alice@example.com", IsActive = true },
                new User { Name = "Bob", Email = "bob@example.com", IsActive = false },
                new User { Name = "Charlie", Email = "charlie@example.com", IsActive = true }
            };

        await userService.AddUsersAsync(usersToAdd1);

        // Получение активных пользователей
        var activeUsers = await userService.GetActiveUsersAsync();
        Console.WriteLine("Active Users:");
        foreach (var user in activeUsers)
        {
            Console.WriteLine($"{user.Name} - {user.Email}");
        }

        // Получение пользователей и их заказов
        var usersWithOrders = await userService.GetUsersWithOrdersAsync();
        Console.WriteLine("Users with Orders:");
        foreach (var user in usersWithOrders)
        {
            Console.WriteLine($"{user.UserName} - Orders: {user.OrderCount}");
        }

        // Кеширование активных пользователей
        await userService.GetCachedActiveUsersAsync();
    }
}