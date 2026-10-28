using WebApplication1.Dtos;

namespace WebApplication1.Services
{
    public interface ICheckoutService
    {
        Task<OrderDTO> CheckoutAsync(int userId, CancellationToken ct);
        Task<bool> CancelOrderAsync(int orderId, CancellationToken ct);
    }
}