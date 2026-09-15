using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly ShopContext _context;

        public ProductController(ShopContext context)
        {
            _context = context;
        }

        private static ProductDTO ToDTO(Product product) => new ProductDTO
        {
            IdProducts = product.IdProducts,
            Name =product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            Url = product.Url,
        };

        [HttpGet]
        public IActionResult GetAll(int page=1, int pageSize = 10)
        {
            var products = _context.Products
                .OrderBy(p => p.IdProducts)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(ToDTO)
                .ToList();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.IdProducts == id);
            if (product is null) return NotFound();
            return Ok(ToDTO(product));
        }

        [HttpPost]
        public IActionResult Create(CreateProductDTO dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                CategoryId = dto.CategoryId,
                Url = dto.Url,
            };
            _context.Products.Add(product);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = product.IdProducts }, ToDTO(product));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateProductDTO dto)
        {
            var product = _context.Products.FirstOrDefault(p => p.IdProducts == id);
            if (product is null) return NotFound();

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.CategoryId = dto.CategoryId;
            product.Url = dto.Url;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.IdProducts == id);
            if (product is null) return NotFound();

            _context.Products.Remove(product);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, PatchProductDTO dto)
        {
            var product = _context.Products.FirstOrDefault(p => p.IdProducts == id);
            if (product is null) return NotFound();

            if (dto.Name is not null) product.Name = dto.Name;
            if (dto.Description is not null) product.Description = dto.Description;
            if (dto.Price is not null) product.Price = dto.Price.Value;
            if (dto.CategoryId is not null) product.CategoryId = dto.CategoryId.Value;
            if (dto.Url is not null) product.Url = dto.Url;

            _context.SaveChanges();
            return NoContent();
        }

    }
}
