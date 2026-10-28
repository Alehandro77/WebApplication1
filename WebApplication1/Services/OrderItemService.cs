using AutoMapper;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _repository;
        private readonly IMapper _mapper;

        public OrderItemService(IOrderItemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<(List<OrderItemDTO> Items, int TotalCount)> GetAllAsync(
            int page, int pageSize, CancellationToken ct)
        {
            var orderItems = await _repository.GetAllAsync(page, pageSize, ct);
            var total = await _repository.CountAsync(ct);
            return (_mapper.Map<List<OrderItemDTO>>(orderItems), total);
        }

        public async Task<OrderItemDTO?> GetByIdAsync(int id, CancellationToken ct)
        {
            var orderItem = await _repository.GetByIdAsync(id, ct);
            return orderItem is null ? null : _mapper.Map<OrderItemDTO?>(orderItem);
        }

        public async Task<OrderItemDTO> CreateAsync(CreateOrderItemDTO dto, CancellationToken ct)
        {
            _ = await _repository.FintOrderAsync(dto.OrderId, ct)
                ?? throw new InvalidOperationException(
                    $"Заказ с Id {dto.OrderId} не найден.");

            _ = await _repository.FintProductAsync(dto.ProductId, ct)
                ?? throw new InvalidOperationException(
                    $"Продукт с Id {dto.ProductId} не найден.");

            var orderItem = _mapper.Map<OrderItem>(dto);

            await _repository.AddAsync(orderItem);
            await _repository.SaveChangesAsync();

            var saved = await _repository.GetByIdAsync(orderItem.IdOrderItem, ct)
                ?? throw new InvalidOperationException(
                    "Не удалось загрузить только что созданный состав корзины.");
            return _mapper.Map<OrderItemDTO>(saved);
        }

        public async Task<OrderItemDTO?> UpdateAsync(
            int id, UpdateOrderItemDTO dto, CancellationToken ct)
        {
            var orderItem = await _repository.GetByIdAsync(id, ct);
            if (orderItem is null) return null;

            _ = await _repository.FintOrderAsync(dto.OrderId, ct)
                ?? throw new InvalidOperationException(
                    $"Заказ с Id {dto.OrderId} не найден.");

            _ = await _repository.FintProductAsync(dto.ProductId, ct)
                ?? throw new InvalidOperationException(
                    $"Продукт с Id {dto.ProductId} не найден.");

            _mapper.Map(dto, orderItem);

            _repository.Update(orderItem);
            await _repository.SaveChangesAsync();

            return _mapper.Map<OrderItemDTO>(orderItem);
        }

        public async Task<OrderItemDTO?> PatсhAsync(
            int id, PatchOrderItemDTO dto, CancellationToken ct)
        {
            var orderItem = await _repository.GetByIdAsync(id, ct);
            if (orderItem is null) return null;

            if (dto.Quantity is not null) orderItem.Quantity = dto.Quantity.Value;

            if (dto.OrderId is not null)
            {
                _ = await _repository.FintOrderAsync(dto.OrderId.Value, ct)
                    ?? throw new InvalidOperationException(
                        $"Заказ с Id {dto.OrderId} не найден.");
                orderItem.OrderId = dto.OrderId.Value;
            }

            if (dto.ProductId is not null)
            {
                _ = await _repository.FintProductAsync(dto.ProductId.Value, ct)
                    ?? throw new InvalidOperationException(
                        $"Продукт с Id {dto.ProductId} не найден.");
                orderItem.ProductId = dto.ProductId.Value;
            }

            _repository.Update(orderItem);
            await _repository.SaveChangesAsync();

            return _mapper.Map<OrderItemDTO>(orderItem);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            var orderItem = await _repository.GetByIdAsync(id, ct);
            if (orderItem is null) return false;

            _repository.Remove(orderItem);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
