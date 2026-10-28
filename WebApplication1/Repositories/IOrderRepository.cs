using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync(int page, int pageSize, CancellationToken ct);
        Task<int> CountAsync(CancellationToken ct);
        Task<Order?> GetByIdAsync(int id, CancellationToken ct);
        Task<User?> FintUserAsync(int userId, CancellationToken ct);
        Task AddAsync(Order order);
        void Update(Order order);
        void Remove(Order order);
        Task SaveChangesAsync();

        Task<Cart?> GetCartByUserIdAsync(int userId, CancellationToken ct);
        Task<Order?> GetOrderWithItemsAsync(int orderId, CancellationToken ct);
        Task AddOrderAsync(Order order);
        Task AddCartAsync(Cart cart);
        Task AddOrderItemsAsync(List<OrderItem> orderItems);
        void RemoveCartItems(List<CartItem> cartItems);
        void RemoveOrder(Order order);
        void RemoveOrderItems(List<OrderItem> orderItems);
    }
}
