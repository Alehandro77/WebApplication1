using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ShopContext _context;
        public OrderItemRepository(ShopContext context)
        {
            _context = context;
        }
        public async Task<List<OrderItem>> GetAllAsync(
            int page, int pageSize, CancellationToken ct)
        {
            return await _context.OrderItems
                .Include(oi => oi.Product)
                .Include(oi => oi.Order)
                .OrderBy(oi => oi.IdOrderItem)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<int> CountAsync(CancellationToken ct) =>
            await _context.OrderItems.CountAsync(ct);

        public async Task<OrderItem?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _context.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.Product)
                .FirstOrDefaultAsync(oi => oi.IdOrderItem == id, ct);
        }

        public async Task<Order?> FintOrderAsync(int id, CancellationToken ct) =>
            await _context.Orders.FirstOrDefaultAsync(u => u.IdOrders == id, ct);

        public async Task<Product?> FintProductAsync(int id, CancellationToken ct) =>
            await _context.Products.FirstOrDefaultAsync(u => u.IdProducts == id, ct);

        public async Task AddAsync(OrderItem orderItem) =>
            await _context.OrderItems.AddAsync(orderItem);

        public void Update(OrderItem orderItem) => _context.OrderItems.Update(orderItem);

        public void Remove(OrderItem orderItem) => _context.OrderItems.Remove(orderItem);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
