using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> Checkout(int userId, CancellationToken ct)
        {
            try
            {
                var order = await _checkoutService.CheckoutAsync(userId, ct);
                return Ok(order);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("cancel/{orderId}")]
        public async Task<IActionResult> CancelOrder(int orderId, CancellationToken ct)
        {
            var result = await _checkoutService.CancelOrderAsync(orderId, ct);
            if (!result) return NotFound();
            return Ok(new { message = "Заказ отменен, товары возвращены в корзину." });
        }
    }
}