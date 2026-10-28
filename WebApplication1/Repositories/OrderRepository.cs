using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ShopContext _context;
        public OrderRepository(ShopContext context)
        {
            _context = context;
        }
        public async Task<List<Order>> GetAllAsync(
            int page, int pageSize, CancellationToken ct)
        {
            return await _context.Orders
                .Include(p => p.User)
                .OrderBy(p => p.IdOrders)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<int> CountAsync(CancellationToken ct) =>
            await _context.Orders.CountAsync(ct);

        public async Task<Order?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Orders
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.IdOrders == id, ct);
        }

        public async Task<User?> FintUserAsync(int id, CancellationToken ct) =>
            await _context.Users.FirstOrDefaultAsync(u => u.IdUser == id, ct);

        public async Task AddAsync(Order order) =>
            await _context.Orders.AddAsync(order);

        public void Update(Order order) => _context.Orders.Update(order);

        public void Remove(Order order) => _context.Orders.Remove(order);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();


        public async Task<Cart?> GetCartByUserIdAsync(int userId, CancellationToken ct)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId, ct);
        }

        public async Task<Order?> GetOrderWithItemsAsync(int orderId, CancellationToken ct)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.IdOrders == orderId, ct);
        }

        public async Task AddOrderAsync(Order order) => await _context.Orders.AddAsync(order);
        public async Task AddOrderItemsAsync(List<OrderItem> orderItems) => await _context.OrderItems.AddRangeAsync(orderItems);
        public async Task AddCartAsync(Cart cart) => await _context.Carts.AddAsync(cart);
        
        public void RemoveCartItems(List<CartItem> cartItems) => _context.CartItems.RemoveRange(cartItems);
        public void RemoveOrder(Order order) => _context.Orders.Remove(order);

        public void RemoveOrderItems(List<OrderItem> orderItems) => _context.OrderItems.RemoveRange(orderItems);
    }
}
