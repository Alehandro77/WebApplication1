using WebApplication1.Dtos;

namespace WebApplication1.Services
{
    public interface IRoleService
    {
        Task<(List<RoleDTO> Items, int TotalCount)> GetAllAsync(int pageIndex, int pageSize, CancellationToken ct);
        Task<RoleDTO?> GetByIdAsync(int id, CancellationToken ct);
        Task<RoleDTO> CreateAsync(CreateRoleDTO dto, CancellationToken ct);
        Task<RoleDTO?> UpdateAsync(int id, UpdateRoleDTO dto, CancellationToken ct);
        Task<RoleDTO?> PatсhAsync(int id, PatchRoleDTO dto, CancellationToken ct);
        Task<bool> DeleteAsync(int id, CancellationToken ct);
    }
}
