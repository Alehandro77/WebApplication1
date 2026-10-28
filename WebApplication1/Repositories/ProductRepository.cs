using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopContext _context;
        public ProductRepository(ShopContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetAllAsync(
            int page, int pageSize, CancellationToken ct)
        {
            return await _context.Products
                .Include(p => p.Category)
                .OrderBy(p => p.IdProducts)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<int> CountAsync(CancellationToken ct) =>
            await _context.Products.CountAsync(ct);

        public async Task<Product?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync( p => p.IdProducts == id, ct);
        }

        public async Task<Category?> FintCategoryAsync(int id, CancellationToken ct) =>
            await _context.Categories.FirstOrDefaultAsync(c => c.IdCategori == id, ct);

        public async Task AddAsync(Product product) =>
            await _context.Products.AddAsync(product);

        public void Update(Product product) => _context.Products.Update(product);

        public void Remove(Product product) => _context.Products.Remove(product);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();

    }
}
