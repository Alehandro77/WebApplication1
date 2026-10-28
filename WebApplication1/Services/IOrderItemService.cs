using WebApplication1.Dtos;

namespace WebApplication1.Services
{
    public interface IOrderItemService
    {
        Task<(List<OrderItemDTO> Items, int TotalCount)> GetAllAsync(int pageIndex, int pageSize, CancellationToken ct);
        Task<OrderItemDTO?> GetByIdAsync(int id, CancellationToken ct);
        Task<OrderItemDTO> CreateAsync(CreateOrderItemDTO dto, CancellationToken ct);
        Task<OrderItemDTO?> UpdateAsync(int id, UpdateOrderItemDTO dto, CancellationToken ct);
        Task<OrderItemDTO?> PatсhAsync(int id, PatchOrderItemDTO dto, CancellationToken ct);
        Task<bool> DeleteAsync(int id, CancellationToken ct);
    }
}
