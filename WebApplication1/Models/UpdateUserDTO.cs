namespace WebApplication1.Models
{
    public class UpdateUserDTO
    {
        public string UserName { get; set; } = null!;

        public string Login { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public int RoleId { get; set; }
    }
}
