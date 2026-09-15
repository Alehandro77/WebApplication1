using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {

        private readonly ShopContext _context;

        public OrderItemController(ShopContext context)
        {
            _context = context;
        }

        private static OrderItemDTO ToDTO(OrderItem orderItem) => new OrderItemDTO
        {
            IdOrderItem = orderItem.IdOrderItem,
            OrderId = orderItem.OrderId,
            ProductId = orderItem.ProductId,
            Quantity = orderItem.Quantity,
            Price = orderItem.Price,
        };

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10)
        {
            var orderItems = _context.OrderItems
                .OrderBy(oi => oi.IdOrderItem)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(ToDTO)
                .ToList();
            return Ok(orderItems);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var orderItem = _context.OrderItems.FirstOrDefault(oi => oi.IdOrderItem == id);
            if (orderItem is null) return NotFound();
            return Ok(ToDTO(orderItem));
        }

        [HttpPost]
        public IActionResult Create(CreateOrderItemDTO dto)
        {
            var orderItem = new OrderItem
            {
                OrderId = dto.OrderId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                Price = dto.Price,
            };
            _context.OrderItems.Add(orderItem);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = orderItem.IdOrderItem }, ToDTO(orderItem));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateOrderItemDTO dto)
        {
            var orderItem = _context.OrderItems.FirstOrDefault(oi => oi.IdOrderItem == id);
            if (orderItem is null) return NotFound();

            orderItem.OrderId = dto.OrderId;
            orderItem.ProductId = dto.ProductId;
            orderItem.Quantity = dto.Quantity;
            orderItem.Price = dto.Price;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var orderItem = _context.OrderItems.FirstOrDefault(oi => oi.IdOrderItem == id);
            if (orderItem is null) return NotFound();

            _context.OrderItems.Remove(orderItem);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, PatchOrderItemDTO dto)
        {
            var orderItem = _context.OrderItems.FirstOrDefault(oi => oi.IdOrderItem == id);
            if (orderItem is null) return NotFound();

            if (dto.OrderId is not null) orderItem.OrderId = dto.OrderId.Value;
            if (dto.ProductId is not null) orderItem.ProductId = dto.ProductId.Value;
            if (dto.Quantity is not null) orderItem.Quantity = dto.Quantity.Value;
            if (dto.Price is not null) orderItem.Price = dto.Price.Value;

            _context.SaveChanges();
            return NoContent();
        }
    }
}
