using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly ShopContext _context;
        public CartItemRepository(ShopContext context)
        {
            _context = context;
        }
        public async Task<List<CartItem>> GetAllAsync(
            int page, int pageSize, CancellationToken ct)
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                .Include(ci => ci.Cart)
                .OrderBy(ci => ci.IdCartItems)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<int> CountAsync(CancellationToken ct) =>
            await _context.CartItems.CountAsync(ct);

        public async Task<CartItem?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _context.CartItems
                .Include(ci => ci.Cart)
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.IdCartItems == id, ct);
        }

        public async Task<Cart?> FintCartAsync(int id, CancellationToken ct) =>
            await _context.Carts.FirstOrDefaultAsync(u => u.IdCarts == id, ct);

        public async Task<Product?> FintProductAsync(int id, CancellationToken ct) =>
            await _context.Products.FirstOrDefaultAsync(u => u.IdProducts == id, ct);

        public async Task AddAsync(CartItem cartItem) =>
            await _context.CartItems.AddAsync(cartItem);

        public void Update(CartItem cartItem) => _context.CartItems.Update(cartItem);

        public void Remove(CartItem cartItem) => _context.CartItems.Remove(cartItem);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();

    }
}
