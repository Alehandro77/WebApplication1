using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly ShopContext _context;

        public CategoryController(ShopContext context)
        {
            _context = context;
        }

        private static CategoryDTO ToDTO(Category category) => new CategoryDTO
        {
            IdCategori = category.IdCategori,
            Name = category.Name,
        };

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10)
        {
            var categorys = _context.Categories
                .OrderBy(c => c.IdCategori)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(ToDTO)
                .ToList();
            return Ok(categorys);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.IdCategori == id);
            if (category is null) return NotFound();
            return Ok(ToDTO(category));
        }

        [HttpPost]
        public IActionResult Create(CreateCategoryDTO dto)
        {
            var category = new Category
            {
                Name = dto.Name,
            };
            _context.Categories.Add(category);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = category.IdCategori }, ToDTO(category));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateCategoryDTO dto)
        {
            var category = _context.Categories.FirstOrDefault(c => c.IdCategori == id);
            if (category is null) return NotFound();

            category.Name = dto.Name;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.IdCategori == id);
            if (category is null) return NotFound();

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, PatchCategoryDTO dto)
        {
            var category = _context.Categories.FirstOrDefault(c => c.IdCategori == id);
            if (category is null) return NotFound();

            if (dto.Name is not null) category.Name = dto.Name;

            _context.SaveChanges();
            return NoContent();
        }
    }
}
