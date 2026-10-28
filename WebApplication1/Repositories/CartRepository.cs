using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ShopContext _context;
        public CartRepository(ShopContext context)
        {
            _context = context;
        }
        public async Task<List<Cart>> GetAllAsync(
            int page, int pageSize, CancellationToken ct)
        {
            return await _context.Carts
                .Include(c => c.User)
                .OrderBy(c => c.IdCarts)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<int> CountAsync(CancellationToken ct) =>
            await _context.Carts.CountAsync(ct);

        public async Task<Cart?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Carts
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.IdCarts == id, ct);
        }

        public async Task<User?> FintUserAsync(int id, CancellationToken ct) =>
            await _context.Users.FirstOrDefaultAsync(u => u.IdUser == id, ct);

        public async Task AddAsync(Cart cart) =>
            await _context.Carts.AddAsync(cart);

        public void Update(Cart cart) => _context.Carts.Update(cart);

        public void Remove(Cart cart) => _context.Carts.Remove(cart);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();

    }
}
