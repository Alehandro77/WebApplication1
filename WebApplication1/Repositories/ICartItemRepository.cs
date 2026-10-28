using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface ICartItemRepository
    {
        Task<List<CartItem>> GetAllAsync(int page, int pageSize, CancellationToken ct);
        Task<int> CountAsync(CancellationToken ct);
        Task<CartItem?> GetByIdAsync(int id, CancellationToken ct);
        Task<Cart?> FintCartAsync(int cartId, CancellationToken ct);
        Task<Product?> FintProductAsync(int productId, CancellationToken ct);
        Task AddAsync(CartItem cartItem);
        void Update(CartItem cartItem);
        void Remove(CartItem cartItem);
        Task SaveChangesAsync();
    }
}
