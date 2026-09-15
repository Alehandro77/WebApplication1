using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ShopContext _context;

        public UserController(ShopContext context)
        {
            _context = context;
        }

        private static UserDTO ToDTO(User user) => new UserDTO
        {
            IdUser = user.IdUser,
            UserName = user.UserName,
            Login = user.Login,
            PasswordHash = user.PasswordHash,
            RoleId = user.RoleId,
        };

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10)
        {
            var users = _context.Users
                .OrderBy(u => u.IdUser)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(ToDTO)
                .ToList();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.IdUser == id);
            if (user is null) return NotFound();
            return Ok(ToDTO(user));
        }

        [HttpPost]
        public IActionResult Create(CreateUserDTO dto)
        {
            var user = new User
            {
                UserName = dto.UserName,
                Login = dto.Login,
                PasswordHash = dto.PasswordHash,
                RoleId = dto.RoleId,
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = user.IdUser }, ToDTO(user));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateUserDTO dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.IdUser == id);
            if (user is null) return NotFound();

            user.UserName = dto.UserName;
            user.Login = dto.Login;
            user.PasswordHash = dto.PasswordHash;
            user.RoleId = dto.RoleId;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.IdUser == id);
            if (user is null) return NotFound();

            _context.Users.Remove(user);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, PatchUserDTO dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.IdUser == id);
            if (user is null) return NotFound();

            if (dto.UserName is not null) user.UserName = dto.UserName;
            if (dto.Login is not null) user.Login = dto.Login;
            if (dto.PasswordHash is not null) user.PasswordHash = dto.PasswordHash;
            if (dto.RoleId is not null) user.RoleId = dto.RoleId.Value;

            _context.SaveChanges();
            return NoContent();
        }
    }
}
