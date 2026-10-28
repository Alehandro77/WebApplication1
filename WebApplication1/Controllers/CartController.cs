using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly ICartService _service;

        public CartController(ICartService service)
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
            var cart = await _service.GetByIdAsync(id, cancellationToken);
            if (cart is null) return NotFound();
            return Ok(cart);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateCartDTO dto, CancellationToken cancellationToken)
        {
            try
            {
                var cart = await _service.CreateAsync(dto, cancellationToken);
                return CreatedAtAction(
                    nameof(GetById), new { id = cart.IdCarts }, cart);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id, UpdateCartDTO dto, CancellationToken cancellationToken)
        {
            try
            {
                var cart = await _service.UpdateAsync(id, dto, cancellationToken);
                if (cart is null) return NotFound();
                return Ok(cart);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(
            int id, PatchCartDTO dto, CancellationToken cancellationToken)
        {
            try
            {
                var cart = await _service.PatсhAsync(id, dto, cancellationToken);
                if (cart is null) return NotFound();
                return Ok(cart);
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
