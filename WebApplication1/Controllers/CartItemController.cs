using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {

        private readonly ShopContext _context;

        public CartItemController(ShopContext context)
        {
            _context = context;
        }

        private static CartItemDTO ToDTO(CartItem cartItem) => new CartItemDTO
        {
            IdCartItems = cartItem.IdCartItems,
            CartId = cartItem.CartId,
            ProductId = cartItem.ProductId,
            Quantity = cartItem.Quantity,
        };

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10)
        {
            var cartItems = _context.CartItems
                .OrderBy(ci => ci.IdCartItems)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(ToDTO)
                .ToList();
            return Ok(cartItems);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var cartItem = _context.CartItems.FirstOrDefault(ci => ci.IdCartItems == id);
            if (cartItem is null) return NotFound();
            return Ok(ToDTO(cartItem));
        }

        [HttpPost]
        public IActionResult Create(CreateCartItemDTO dto)
        {
            var cartItem = new CartItem
            {
                CartId = dto.CartId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
            };
            _context.CartItems.Add(cartItem);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = cartItem.IdCartItems }, ToDTO(cartItem));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateCartItemDTO dto)
        {
            var cartItem = _context.CartItems.FirstOrDefault(ci => ci.IdCartItems == id);
            if (cartItem is null) return NotFound();

            cartItem.CartId = dto.CartId;
            cartItem.ProductId = dto.ProductId;
            cartItem.Quantity = dto.Quantity;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var cartItem = _context.CartItems.FirstOrDefault(ci => ci.IdCartItems == id);
            if (cartItem is null) return NotFound();

            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, PatchCartItemDTO dto)
        {
            var cartItem = _context.CartItems.FirstOrDefault(ci => ci.IdCartItems == id);
            if (cartItem is null) return NotFound();

            if (dto.CartId is not null) cartItem.CartId = dto.CartId.Value;
            if (dto.ProductId is not null) cartItem.ProductId = dto.ProductId.Value;
            if (dto.Quantity is not null) cartItem.Quantity = dto.Quantity.Value;

            _context.SaveChanges();
            return NoContent();
        }
    }
}
