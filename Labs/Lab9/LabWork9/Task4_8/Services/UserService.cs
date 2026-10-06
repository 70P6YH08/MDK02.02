using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Task4_8.Context;
using Task4_8.Models;

namespace Task4_8.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);

        public UserService(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // Получение активных пользователей
        public async Task<List<User>> GetActiveUsersAsync()
        {
            // Оптимизация: Использовать AsNoTracking
            var users = await _context.Users
                .AsNoTracking()
                .Where(u => u.IsActive)
                .ToListAsync();
            return users;
        }

        // Получение пользователей и их заказов
        public async Task<List<UserDto>> GetUsersWithOrdersAsync()
        {
            // Оптимизация: Использовать Select для выборки нужных данных
            var users = await _context.Users
                .AsNoTracking()
                .Select(u => new UserDto()
                {
                    UserName = u.Name,
                    OrderCount = u.Orders.Count()
                })
                .ToListAsync();
            return users;
        }

        // Массовое добавление пользователей
        public async Task AddUsersAsync(List<User> users)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                _context.Users.AddRange(users);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
        }

        public async Task<List<User>> GetCachedActiveUsersAsync()
        {
            if (!_cache.TryGetValue("ActiveUsers", out List<User>? cachedUsers))
            {
                cachedUsers = await _context.Users
                    .AsNoTracking()
                    .Where(u => u.IsActive)
                    .ToListAsync();

                if(cachedUsers != null)
                    _cache.Set("ActiveUsers",
                        cachedUsers,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(_cacheDuration));
            }
            return cachedUsers;
        }
    }
}
