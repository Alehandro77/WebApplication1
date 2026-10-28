using WebApplication1.Dtos;

namespace WebApplication1.Services
{
    public interface ICartItemService
    {
        Task<(List<CartItemDTO> Items, int TotalCount)> GetAllAsync(int pageIndex, int pageSize, CancellationToken ct);
        Task<CartItemDTO?> GetByIdAsync(int id, CancellationToken ct);
        Task<CartItemDTO> CreateAsync(CreateCartItemDTO dto, CancellationToken ct);
        Task<CartItemDTO?> UpdateAsync(int id, UpdateCartItemDTO dto, CancellationToken ct);
        Task<CartItemDTO?> PatсhAsync(int id, PatchCartItemDTO dto, CancellationToken ct);
        Task<bool> DeleteAsync(int id, CancellationToken ct);
    }
}
