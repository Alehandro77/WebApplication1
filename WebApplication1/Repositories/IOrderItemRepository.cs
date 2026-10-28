using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IOrderItemRepository
    {
        Task<List<OrderItem>> GetAllAsync(int page, int pageSize, CancellationToken ct);
        Task<int> CountAsync(CancellationToken ct);
        Task<OrderItem?> GetByIdAsync(int id, CancellationToken ct);
        Task<Order?> FintOrderAsync(int orderId, CancellationToken ct);
        Task<Product?> FintProductAsync(int productId, CancellationToken ct);
        Task AddAsync(OrderItem orderItem);
        void Update(OrderItem orderItem);
        void Remove(OrderItem orderItem);
        Task SaveChangesAsync();
    }
}
