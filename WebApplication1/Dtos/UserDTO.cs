namespace WebApplication1.Dtos
{
    public class UserDTO
    {
        public int IdUser { get; set; }

        public string UserName { get; set; } = null!;

        public string Login { get; set; } = null!;

        public int RoleId { get; set; }
    }
}
