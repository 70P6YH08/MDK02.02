using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Task4_8.Context;
using Task4_8.DTOs;
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

        public async Task<List<User>> GetActiveUsersAsync()
        {
            var users = await _context.Users
                .AsNoTracking()
                .Where(u => u.IsActive)
                .ToListAsync();
            return users;
        }

        public async Task<List<UserDto>> GetUserDtosWithOrdersAsync()
        {
            var users = await _context.Users
                .AsNoTracking()
                .Select(u => new UserDto
                {
                    UserName = u.Name,
                    OrderCount = u.Orders.Count()
                })
                .ToListAsync();
            return users;
        }

        public async Task AddUsersAsync(List<User> users)
        {
            try
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    _context.Users.AddRange(users);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }

                _cache.Remove("ActiveUsers");
                Console.WriteLine("Пользователи добавлены, кэш сброшен.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<List<User>> GetCachedActiveUsersAsync()
        {
            if (_cache.TryGetValue("ActiveUsers", out List<User>? cachedUsers))
            {
                foreach (var cachedUser in cachedUsers)
                    Console.WriteLine($"{cachedUser.Name} извлечён из кэша");
                return cachedUsers;
            }

            cachedUsers = await _context.Users
                .Where(u => u.IsActive)
                .ToListAsync();

            foreach (var cachedUser in cachedUsers)
                Console.WriteLine($"{cachedUser.Name} извлечён из бд");

            _cache.Set("ActiveUsers",
                    cachedUsers,
                    new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(_cacheDuration));

            return cachedUsers;
        }
    }
}
