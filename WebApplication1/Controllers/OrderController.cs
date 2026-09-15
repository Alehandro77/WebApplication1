using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly ShopContext _context;

        public OrderController(ShopContext context)
        {
            _context = context;
        }

        private static OrderDTO ToDTO(Order order) => new OrderDTO
        {
            IdOrders = order.IdOrders,
            UserId = order.UserId,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
        };

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10)
        {
            var orders = _context.Orders
                .OrderBy(o => o.IdOrders)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(ToDTO)
                .ToList();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.IdOrders == id);
            if (order is null) return NotFound();
            return Ok(ToDTO(order));
        }

        [HttpPost]
        public IActionResult Create(CreateOrderDTO dto)
        {
            var order = new Order
            {
                UserId = dto.UserId,
                OrderDate = dto.OrderDate,
                TotalAmount = dto.TotalAmount,
            };
            _context.Orders.Add(order);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = order.IdOrders }, ToDTO(order));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateOrderDTO dto)
        {
            var order = _context.Orders.FirstOrDefault(o => o.IdOrders == id);
            if (order is null) return NotFound();

            order.UserId = dto.UserId;
            order.OrderDate = dto.OrderDate;
            order.TotalAmount = dto.TotalAmount;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.IdOrders == id);
            if (order is null) return NotFound();

            _context.Orders.Remove(order);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, PatchOrderDTO dto)
        {
            var order = _context.Orders.FirstOrDefault(o => o.IdOrders == id);
            if (order is null) return NotFound();

            if (dto.UserId is not null) order.UserId = dto.UserId.Value;
            if (dto.OrderDate is not null) order.OrderDate = dto.OrderDate.Value;
            if (dto.TotalAmount is not null) order.TotalAmount = dto.TotalAmount.Value;

            _context.SaveChanges();
            return NoContent();
        }
    }
}
