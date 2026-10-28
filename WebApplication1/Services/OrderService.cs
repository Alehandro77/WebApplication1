using AutoMapper;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<(List<OrderDTO> Items, int TotalCount)> GetAllAsync(
            int page, int pageSize, CancellationToken ct)
        {
            var orders = await _repository.GetAllAsync(page, pageSize, ct);
            var total = await _repository.CountAsync(ct);
            return (_mapper.Map<List<OrderDTO>>(orders), total);
        }

        public async Task<OrderDTO?> GetByIdAsync(int id, CancellationToken ct)
        {
            var order = await _repository.GetByIdAsync(id, ct);
            return order is null ? null : _mapper.Map<OrderDTO?>(order);
        }

        public async Task<OrderDTO> CreateAsync(CreateOrderDTO dto, CancellationToken ct)
        {
            _ = await _repository.FintUserAsync(dto.UserId, ct)
                ?? throw new InvalidOperationException(
                    $"Юзер с Id {dto.UserId} не найден.");

            var order = _mapper.Map<Order>(dto);

            order.OrderDate = DateTime.Now;

            await _repository.AddAsync(order);
            await _repository.SaveChangesAsync();

            var saved = await _repository.GetByIdAsync(order.IdOrders, ct)
                ?? throw new InvalidOperationException(
                    "Не удалось загрузить только что созданный заказ.");
            return _mapper.Map<OrderDTO>(saved);
        }

        public async Task<OrderDTO?> UpdateAsync(
            int id, UpdateOrderDTO dto, CancellationToken ct)
        {
            var order = await _repository.GetByIdAsync(id, ct);
            if (order is null) return null;

            _ = await _repository.FintUserAsync(dto.UserId, ct)
                ?? throw new InvalidOperationException(
                    $"Юзер с Id {dto.UserId} не найден.");

            _mapper.Map(dto, order);

            _repository.Update(order);
            await _repository.SaveChangesAsync();

            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<OrderDTO?> PatсhAsync(
            int id, PatchOrderDTO dto, CancellationToken ct)
        {
            var order = await _repository.GetByIdAsync(id, ct);
            if (order is null) return null;

            if (dto.OrderDate is not null) order.OrderDate = dto.OrderDate.Value;
            if (dto.TotalAmount is not null) order.TotalAmount = dto.TotalAmount.Value;

            if (dto.UserId is not null)
            {
                _ = await _repository.FintUserAsync(dto.UserId.Value, ct)
                    ?? throw new InvalidOperationException(
                        $"Юзер с Id {dto.UserId} не найден.");
                order.UserId = dto.UserId.Value;
            }

            _repository.Update(order);
            await _repository.SaveChangesAsync();

            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            var order = await _repository.GetByIdAsync(id, ct);
            if (order is null) return false;

            _repository.Remove(order);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
