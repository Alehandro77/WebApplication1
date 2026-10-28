using AutoMapper;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _repository;
        private readonly IMapper _mapper;

        public CartItemService(ICartItemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<(List<CartItemDTO> Items, int TotalCount)> GetAllAsync(
            int page, int pageSize, CancellationToken ct)
        {
            var cartItems = await _repository.GetAllAsync(page, pageSize, ct);
            var total = await _repository.CountAsync(ct);
            return (_mapper.Map<List<CartItemDTO>>(cartItems), total);
        }

        public async Task<CartItemDTO?> GetByIdAsync(int id, CancellationToken ct)
        {
            var cartItem = await _repository.GetByIdAsync(id, ct);
            return cartItem is null ? null : _mapper.Map<CartItemDTO?>(cartItem);
        }

        public async Task<CartItemDTO> CreateAsync(CreateCartItemDTO dto, CancellationToken ct)
        {
            _ = await _repository.FintCartAsync(dto.CartId, ct)
                ?? throw new InvalidOperationException(
                    $"Корзина с Id {dto.CartId} не найдена.");

            _ = await _repository.FintProductAsync(dto.ProductId, ct)
                ?? throw new InvalidOperationException(
                    $"Продукт с Id {dto.ProductId} не найден.");

            var cartItem = _mapper.Map<CartItem>(dto);

            await _repository.AddAsync(cartItem);
            await _repository.SaveChangesAsync();

            var saved = await _repository.GetByIdAsync(cartItem.IdCartItems, ct)
                ?? throw new InvalidOperationException(
                    "Не удалось загрузить только что созданный состав корзины.");
            return _mapper.Map<CartItemDTO>(saved);
        }

        public async Task<CartItemDTO?> UpdateAsync(
            int id, UpdateCartItemDTO dto, CancellationToken ct)
        {
            var cartItem = await _repository.GetByIdAsync(id, ct);
            if (cartItem is null) return null;

            _ = await _repository.FintCartAsync(dto.CartId, ct)
                ?? throw new InvalidOperationException(
                    $"Корзина с Id {dto.CartId} не найдена.");

            _ = await _repository.FintProductAsync(dto.ProductId, ct)
                ?? throw new InvalidOperationException(
                    $"Продукт с Id {dto.ProductId} не найден.");

            _mapper.Map(dto, cartItem);

            _repository.Update(cartItem);
            await _repository.SaveChangesAsync();

            return _mapper.Map<CartItemDTO>(cartItem);
        }

        public async Task<CartItemDTO?> PatсhAsync(
            int id, PatchCartItemDTO dto, CancellationToken ct)
        {
            var cartItem = await _repository.GetByIdAsync(id, ct);
            if (cartItem is null) return null;

            if (dto.Quantity is not null) cartItem.Quantity = dto.Quantity.Value;

            if (dto.CartId is not null)
            {
                _ = await _repository.FintCartAsync(dto.CartId.Value, ct)
                    ?? throw new InvalidOperationException(
                        $"Корзина с Id {dto.CartId} не найдена.");
                cartItem.CartId = dto.CartId.Value;
            }

            if (dto.ProductId is not null)
            {
                _ = await _repository.FintProductAsync(dto.ProductId.Value, ct)
                    ?? throw new InvalidOperationException(
                        $"Продукт с Id {dto.ProductId} не найден.");
                cartItem.ProductId = dto.ProductId.Value;
            }

            _repository.Update(cartItem);
            await _repository.SaveChangesAsync();

            return _mapper.Map<CartItemDTO>(cartItem);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            var cartItem = await _repository.GetByIdAsync(id, ct);
            if (cartItem is null) return false;

            _repository.Remove(cartItem);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
