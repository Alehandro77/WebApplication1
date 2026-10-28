using WebApplication1.Dtos;

namespace WebApplication1.Services
{
    public interface IUserService
    {
        Task<(List<UserDTO> Items, int TotalCount)> GetAllAsync(int pageIndex, int pageSize, CancellationToken ct);
        Task<UserDTO?> GetByIdAsync(int id, CancellationToken ct);
        Task<UserDTO> CreateAsync(CreateUserDTO dto, CancellationToken ct);
        Task<UserDTO?> UpdateAsync(int id, UpdateUserDTO dto, CancellationToken ct);
        Task<UserDTO?> PatсhAsync(int id, PatchUserDTO dto, CancellationToken ct);
        Task<bool> DeleteAsync(int id, CancellationToken ct);
    }
}
