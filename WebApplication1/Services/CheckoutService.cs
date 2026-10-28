using AutoMapper;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public CheckoutService(IOrderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OrderDTO> CheckoutAsync(int userId, CancellationToken ct)
        {
            var cart = await _repository.GetCartByUserIdAsync(userId, ct);
            if (cart == null || !cart.CartItems.Any())
            {
                throw new InvalidOperationException("Корзина пуста или не найдена.");
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = cart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price)
            };

            var orderItems = cart.CartItems.Select(ci => new OrderItem
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                Price = ci.Product.Price
            }).ToList();

            order.OrderItems = orderItems;

            await _repository.AddOrderAsync(order);

            _repository.RemoveCartItems(cart.CartItems.ToList());

            await _repository.SaveChangesAsync();
            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<bool> CancelOrderAsync(int orderId, CancellationToken ct)
        {
            var order = await _repository.GetOrderWithItemsAsync(orderId, ct);
            if (order == null)
            {
                return false;
            }

            var cart = await _repository.GetCartByUserIdAsync(order.UserId, ct);
            if (cart == null)
            {
                cart = new Cart { UserId = order.UserId };
                await _repository.AddCartAsync(cart); // Добавим метод AddCartAsync в репозиторий
            }

            foreach (var item in order.OrderItems)
            {
                var existingCartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == item.ProductId);

                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += item.Quantity;
                }
                else
                {
                    cart.CartItems.Add(new CartItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    });
                }
            }

            _repository.RemoveOrderItems(order.OrderItems.ToList());
            _repository.RemoveOrder(order);

            await _repository.SaveChangesAsync();

            return true;
        }
    }
}