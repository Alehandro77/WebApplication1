using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {

        private readonly IOrderItemService _service;

        public OrderItemController(IOrderItemService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var (items, total) = await _service.GetAllAsync(
                page, pageSize, cancellationToken);

            Response.Headers["X-Total-Count"] = total.ToString();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var orderItem = await _service.GetByIdAsync(id, cancellationToken);
            if (orderItem is null) return NotFound();
            return Ok(orderItem);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateOrderItemDTO dto, CancellationToken cancellationToken)
        {
            try
            {
                var orderItem = await _service.CreateAsync(dto, cancellationToken);
                return CreatedAtAction(
                    nameof(GetById), new { id = orderItem.IdOrderItem }, orderItem);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id, UpdateOrderItemDTO dto, CancellationToken cancellationToken)
        {
            try
            {
                var orderItem = await _service.UpdateAsync(id, dto, cancellationToken);
                if (orderItem is null) return NotFound();
                return Ok(orderItem);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(
            int id, PatchOrderItemDTO dto, CancellationToken cancellationToken)
        {
            try
            {
                var orderItem = await _service.PatсhAsync(id, dto, cancellationToken);
                if (orderItem is null) return NotFound();
                return Ok(orderItem);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            int id, CancellationToken cancellationToken)
        {
            var delete = await _service.DeleteAsync(id, cancellationToken);
            if (!delete) return NotFound();
            return NoContent();
        }
    }
}
