using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly ShopContext _context;

        public CartController(ShopContext context)
        {
            _context = context;
        }

        private static CartDTO ToDTO(Cart cart) => new CartDTO
        {
            IdCarts = cart.IdCarts,
            UserId = cart.UserId,
    };

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10)
        {
            var carts = _context.Carts
                .OrderBy(c => c.IdCarts)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(ToDTO)
                .ToList();
            return Ok(carts);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var cart = _context.Carts.FirstOrDefault(c => c.IdCarts == id);
            if (cart is null) return NotFound();
            return Ok(ToDTO(cart));
        }

        [HttpPost]
        public IActionResult Create(CreateCartDTO dto)
        {
            var cart = new Cart
            {
                UserId = dto.UserId,
            };
            _context.Carts.Add(cart);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = cart.IdCarts }, ToDTO(cart));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateCartDTO dto)
        {
            var cart = _context.Carts.FirstOrDefault(c => c.IdCarts == id);
            if (cart is null) return NotFound();

            cart.UserId = dto.UserId;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var cart = _context.Carts.FirstOrDefault(c => c.IdCarts == id);
            if (cart is null) return NotFound();

            _context.Carts.Remove(cart);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, PatchCartDTO dto)
        {
            var cart = _context.Carts.FirstOrDefault(c => c.IdCarts == id);
            if (cart is null) return NotFound();

            if (dto.UserId is not null) cart.UserId = dto.UserId.Value;

            _context.SaveChanges();
            return NoContent();
        }
    }
}
