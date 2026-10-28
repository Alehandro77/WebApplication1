using WebApplication1.Dtos;

namespace WebApplication1.Services
{
    public interface IOrderService
    {
        Task<(List<OrderDTO> Items, int TotalCount)> GetAllAsync(int pageIndex, int pageSize, CancellationToken ct);
        Task<OrderDTO?> GetByIdAsync(int id, CancellationToken ct);
        Task<OrderDTO> CreateAsync(CreateOrderDTO dto, CancellationToken ct);
        Task<OrderDTO?> UpdateAsync(int id, UpdateOrderDTO dto, CancellationToken ct);
        Task<OrderDTO?> PatсhAsync(int id, PatchOrderDTO dto, CancellationToken ct);
        Task<bool> DeleteAsync(int id, CancellationToken ct);
    }
}
