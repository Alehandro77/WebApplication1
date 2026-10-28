namespace WebApplication1.Dtos
{
    public class CreateUserDTO
    {
        public string UserName { get; set; } = null!;

        public string Login { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public int RoleId { get; set; }
    }
}
