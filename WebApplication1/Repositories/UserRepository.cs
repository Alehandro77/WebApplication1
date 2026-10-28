using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ShopContext _context;
        public UserRepository(ShopContext context)
        {
            _context = context;
        }
        public async Task<List<User>> GetAllAsync(
            int page, int pageSize, CancellationToken ct)
        {
            return await _context.Users
                .Include(u => u.Role)
                .OrderBy(u => u.IdUser)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<int> CountAsync(CancellationToken ct) =>
            await _context.Users.CountAsync(ct);

        public async Task<User?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.IdUser == id, ct);
        }

        public async Task<Role?> FintRoleAsync(int id, CancellationToken ct) =>
            await _context.Roles.FirstOrDefaultAsync(r => r.Id == id, ct);

        public async Task AddAsync(User user) =>
            await _context.Users.AddAsync(user);

        public void Update(User user) => _context.Users.Update(user);

        public void Remove(User user) => _context.Users.Remove(user);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();

    }
}
