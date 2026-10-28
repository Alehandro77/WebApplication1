using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ShopContext _context;
        public RoleRepository(ShopContext context)
        {
            _context = context;
        }
        public async Task<List<Role>> GetAllAsync(
            int page, int pageSize, CancellationToken ct)
        {
            return await _context.Roles
                .OrderBy(r => r.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<int> CountAsync(CancellationToken ct) =>
            await _context.Roles.CountAsync(ct);

        public async Task<Role?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == id, ct);
        }

        public async Task AddAsync(Role role) =>
            await _context.Roles.AddAsync(role);

        public void Update(Role role) => _context.Roles.Update(role);

        public void Remove(Role role) => _context.Roles.Remove(role);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();

    }
}
