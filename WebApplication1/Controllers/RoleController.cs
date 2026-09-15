using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {

        private readonly ShopContext _context;

        public RoleController(ShopContext context)
        {
            _context = context;
        }

        private static RoleDTO ToDTO(Role role) => new RoleDTO
        {
            Id = role.Id,
        };

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10)
        {
            var roles = _context.Roles
                .OrderBy(r => r.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(ToDTO)
                .ToList();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var role = _context.Roles.FirstOrDefault(r => r.Id == id);
            if (role is null) return NotFound();
            return Ok(ToDTO(role));
        }

        [HttpPost]
        public IActionResult Create(CreateRoleDTO dto)
        {
            var role = new Role
            {
                Name = dto.Name,
            };
            _context.Roles.Add(role);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = role.Id }, ToDTO(role));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateRoleDTO dto)
        {
            var role = _context.Roles.FirstOrDefault(r => r.Id == id);
            if (role is null) return NotFound();

            role.Name = dto.Name;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var role = _context.Roles.FirstOrDefault(r => r.Id == id);
            if (role is null) return NotFound();

            _context.Roles.Remove(role);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, PatchRoleDTO dto)
        {
            var role = _context.Roles.FirstOrDefault(r => r.Id == id);
            if (role is null) return NotFound();

            if (dto.Name is not null) role.Name = dto.Name;

            _context.SaveChanges();
            return NoContent();
        }
    }
}
