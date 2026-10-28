using AutoMapper;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<(List<RoleDTO> Items, int TotalCount)> GetAllAsync(
            int page, int pageSize, CancellationToken ct)
        {
            var roles = await _repository.GetAllAsync(page, pageSize, ct);
            var total = await _repository.CountAsync(ct);
            return (_mapper.Map<List<RoleDTO>>(roles), total);
        }

        public async Task<RoleDTO?> GetByIdAsync(int id, CancellationToken ct)
        {
            var role = await _repository.GetByIdAsync(id, ct);
            return role is null ? null : _mapper.Map<RoleDTO?>(role);
        }

        public async Task<RoleDTO> CreateAsync(CreateRoleDTO dto, CancellationToken ct)
        {
            var role = _mapper.Map<Role>(dto);

            await _repository.AddAsync(role);
            await _repository.SaveChangesAsync();

            var saved = await _repository.GetByIdAsync(role.Id, ct)
                ?? throw new InvalidOperationException(
                    "Не удалось загрузить только что созданную роль.");
            return _mapper.Map<RoleDTO>(saved);
        }

        public async Task<RoleDTO?> UpdateAsync(
            int id, UpdateRoleDTO dto, CancellationToken ct)
        {
            var role = await _repository.GetByIdAsync(id, ct);
            if (role is null) return null;

            _mapper.Map(dto, role);

            _repository.Update(role);
            await _repository.SaveChangesAsync();

            return _mapper.Map<RoleDTO>(role);
        }

        public async Task<RoleDTO?> PatсhAsync(
            int id, PatchRoleDTO dto, CancellationToken ct)
        {
            var role = await _repository.GetByIdAsync(id, ct);
            if (role is null) return null;

            if (dto.Name is not null) role.Name = dto.Name;

            _repository.Update(role);
            await _repository.SaveChangesAsync();

            return _mapper.Map<RoleDTO>(role);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            var role = await _repository.GetByIdAsync(id, ct);
            if (role is null) return false;

            _repository.Remove(role);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
