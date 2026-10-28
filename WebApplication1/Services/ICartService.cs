using WebApplication1.Dtos;

namespace WebApplication1.Services
{
    public interface ICartService
    {
        Task<(List<CartDTO> Items, int TotalCount)> GetAllAsync(int pageIndex, int pageSize, CancellationToken ct);
        Task<CartDTO?> GetByIdAsync(int id, CancellationToken ct);
        Task<CartDTO> CreateAsync(CreateCartDTO dto, CancellationToken ct);
        Task<CartDTO?> UpdateAsync(int id, UpdateCartDTO dto, CancellationToken ct);
        Task<CartDTO?> PatсhAsync(int id, PatchCartDTO dto, CancellationToken ct);
        Task<bool> DeleteAsync(int id, CancellationToken ct);
    }
}
