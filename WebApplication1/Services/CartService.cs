using AutoMapper;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repository;
        private readonly IMapper _mapper;

        public CartService(ICartRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<(List<CartDTO> Items, int TotalCount)> GetAllAsync(
            int page, int pageSize, CancellationToken ct)
        {
            var carts = await _repository.GetAllAsync(page, pageSize, ct);
            var total = await _repository.CountAsync(ct);
            return (_mapper.Map<List<CartDTO>>(carts), total);
        }

        public async Task<CartDTO?> GetByIdAsync(int id, CancellationToken ct)
        {
            var cart = await _repository.GetByIdAsync(id, ct);
            return cart is null ? null : _mapper.Map<CartDTO?>(cart);
        }

        public async Task<CartDTO> CreateAsync(CreateCartDTO dto, CancellationToken ct)
        {
            _ = await _repository.FintUserAsync(dto.UserId, ct)
                ?? throw new InvalidOperationException(
                    $"Юзер с Id {dto.UserId} не найден.");

            var cart = _mapper.Map<Cart>(dto);

            await _repository.AddAsync(cart);
            await _repository.SaveChangesAsync();

            var saved = await _repository.GetByIdAsync(cart.IdCarts, ct)
                ?? throw new InvalidOperationException(
                    "Не удалось загрузить только что созданный заказ.");
            return _mapper.Map<CartDTO>(saved);
        }

        public async Task<CartDTO?> UpdateAsync(
            int id, UpdateCartDTO dto, CancellationToken ct)
        {
            var cart = await _repository.GetByIdAsync(id, ct);
            if (cart is null) return null;

            _ = await _repository.FintUserAsync(dto.UserId, ct)
                ?? throw new InvalidOperationException(
                    $"Юзер с Id {dto.UserId} не найден.");

            _mapper.Map(dto, cart);

            _repository.Update(cart);
            await _repository.SaveChangesAsync();

            return _mapper.Map<CartDTO>(cart);
        }

        public async Task<CartDTO?> PatсhAsync(
            int id, PatchCartDTO dto, CancellationToken ct)
        {
            var cart = await _repository.GetByIdAsync(id, ct);
            if (cart is null) return null;

            if (dto.UserId is not null)
            {
                _ = await _repository.FintUserAsync(dto.UserId.Value, ct)
                    ?? throw new InvalidOperationException(
                        $"Юзер с Id {dto.UserId} не найден.");
                cart.UserId = dto.UserId.Value;
            }

            _repository.Update(cart);
            await _repository.SaveChangesAsync();

            return _mapper.Map<CartDTO>(cart);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            var cart = await _repository.GetByIdAsync(id, ct);
            if (cart is null) return false;

            _repository.Remove(cart);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
