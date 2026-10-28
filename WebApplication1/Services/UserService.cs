using AutoMapper;
using BCrypt;
using Microsoft.Win32.SafeHandles;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<(List<UserDTO> Items, int TotalCount)> GetAllAsync(
            int page, int pageSize, CancellationToken ct)
        {
            var users = await _repository.GetAllAsync(page, pageSize, ct);
            var total = await _repository.CountAsync(ct);
            return (_mapper.Map<List<UserDTO>>(users), total);
        }

        public async Task<UserDTO?> GetByIdAsync(int id, CancellationToken ct)
        {
            var user = await _repository.GetByIdAsync(id, ct);
            return user is null ? null : _mapper.Map<UserDTO?>(user);
        }

        public async Task<UserDTO> CreateAsync(CreateUserDTO dto, CancellationToken ct)
        {
            _ = await _repository.FintRoleAsync(dto.RoleId, ct)
                ?? throw new InvalidOperationException(
                    $"Роль с Id {dto.RoleId} не найдена.");

            var user = _mapper.Map<User>(dto);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);

            await _repository.AddAsync(user);
            await _repository.SaveChangesAsync();

            var saved = await _repository.GetByIdAsync(user.IdUser, ct)
                ?? throw new InvalidOperationException(
                    "Не удалось загрузить только что созданного юзера.");
            return _mapper.Map<UserDTO>(saved);
        }

        public async Task<UserDTO?> UpdateAsync(
            int id, UpdateUserDTO dto, CancellationToken ct)
        {
            var user = await _repository.GetByIdAsync(id, ct);
            if (user is null) return null;

            _ = await _repository.FintRoleAsync(dto.RoleId, ct)
                ?? throw new InvalidOperationException(
                    $"Роль с Id {dto.RoleId} не найдена.");

            _mapper.Map(dto, user);

            if (!string.IsNullOrEmpty(dto.PasswordHash))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);
            }

            _repository.Update(user);
            await _repository.SaveChangesAsync();

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO?> PatсhAsync(
            int id, PatchUserDTO dto, CancellationToken ct)
        {
            var user = await _repository.GetByIdAsync(id, ct);
            if (user is null) return null;

            if (dto.UserName is not null) user.UserName = dto.UserName;
            if (dto.Login is not null) user.Login = dto.Login;

            if (dto.RoleId is not null)
            {
                _ = await _repository.FintRoleAsync(dto.RoleId.Value, ct)
                    ?? throw new InvalidOperationException(
                        $"Роль с Id {dto.RoleId} не найдена.");
                user.RoleId = dto.RoleId.Value;
            }

            if (dto.PasswordHash is not null)
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);

            _repository.Update(user);
            await _repository.SaveChangesAsync();

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            var user = await _repository.GetByIdAsync(id, ct);
            if (user is null) return false;

            _repository.Remove(user);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
