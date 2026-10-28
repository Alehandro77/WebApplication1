using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {

        private readonly ICartItemService _service;

        public CartItemController(ICartItemService service)
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
            var сartItem = await _service.GetByIdAsync(id, cancellationToken);
            if (сartItem is null) return NotFound();
            return Ok(сartItem);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateCartItemDTO dto, CancellationToken cancellationToken)
        {
            try
            {
                var сartItem = await _service.CreateAsync(dto, cancellationToken);
                return CreatedAtAction(
                    nameof(GetById), new { id = сartItem.IdCartItems }, сartItem);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id, UpdateCartItemDTO dto, CancellationToken cancellationToken)
        {
            try
            {
                var сartItem = await _service.UpdateAsync(id, dto, cancellationToken);
                if (сartItem is null) return NotFound();
                return Ok(сartItem);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(
            int id, PatchCartItemDTO dto, CancellationToken cancellationToken)
        {
            try
            {
                var сartItem = await _service.PatсhAsync(id, dto, cancellationToken);
                if (сartItem is null) return NotFound();
                return Ok(сartItem);
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
