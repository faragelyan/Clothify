using AutoMapper;
using Clothify.Application.DTOs;
using Clothify.Application.DTOs.Order;
using Clothify.Application.Interfaces;
using Clothify.Domain.Entities;
using Clothify.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Clothify.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Guid>> AddAsync(CreateOrderDto dto)
        {
            var userExists = await _unitOfWork.AppUsers.GetSingleEntityAsync(u => u.Id == dto.UserId);
            if (userExists == null)
            {
                return Result<Guid>.Fail("User not found");
            }

            var addressExists = await _unitOfWork.Addresses.GetSingleEntityAsync(a => a.AddressId == dto.AddressId);
            if (addressExists == null)
            {
                return Result<Guid>.Fail("Address not found");
            }

            // Secure Pricing Calculation & Lifecycle Management
            var cart = await _unitOfWork.ShoppingCarts.GetSingleEntityAsync(c => c.UserId == dto.UserId);
            if (cart == null) return Result<Guid>.Fail("Shopping cart not found");

            var cartItems = await _unitOfWork.CartItems.GetAllEntitiesAsync(
                filter: ci => ci.CartId == cart.CartId,
                includes: q => q.Include(ci => ci.Product)
            );

            if (!cartItems.Any()) return Result<Guid>.Fail("Shopping cart is empty");

            var secureTotalAmount = cartItems.Sum(ci => (ci.Product?.Price ?? 0m) * ci.Quantity);

            var order = _mapper.Map<Order>(dto);
            order.OrderDate = DateTime.UtcNow;
            order.TotalAmount = secureTotalAmount; // Overwrite DTO input to absolutely prevent fraudulent price manipulation

            var added = await _unitOfWork.Orders.AddAsync(order);
            if (!added)
                return Result<Guid>.Fail("Failed to add order");

            // Convert Cart Items into immutable Order Items
            foreach (var item in cartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Product?.Price ?? 0m
                };
                await _unitOfWork.OrderItems.AddAsync(orderItem);
            }

            // Flush the cart correctly
            _unitOfWork.CartItems.DeleteRange(cartItems);
            cart.TotalAmount = 0m;
            _unitOfWork.ShoppingCarts.Update(cart);

            await _unitOfWork.CommitAsync();
            return Result<Guid>.Ok(order.OrderId);
        }

        public async Task<Result<bool>> UpdateAsync(UpdateOrderDto dto)
        {
            var order = await _unitOfWork.Orders.GetSingleEntityAsync(
                filter: o => o.OrderId == dto.OrderId
            );

            if (order is null)
                return Result<bool>.Fail("Order not found");

            var userExists = await _unitOfWork.AppUsers.GetSingleEntityAsync(u => u.Id == dto.UserId);
            if (userExists == null)
            {
                return Result<bool>.Fail("User not found");
            }

            var addressExists = await _unitOfWork.Addresses.GetSingleEntityAsync(a => a.AddressId == dto.AddressId);
            if (addressExists == null)
            {
                return Result<bool>.Fail("Address not found");
            }

            _mapper.Map(dto, order);

            var updated = _unitOfWork.Orders.Update(order);
            if (!updated)
                return Result<bool>.Fail("Failed to update order");

            await _unitOfWork.CommitAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> RemoveAsync(Guid orderId)
        {
            var order = await _unitOfWork.Orders.GetSingleEntityAsync(
                filter: o => o.OrderId == orderId
            );

            if (order is null)
                return Result<bool>.Fail("Order not found");

            var usedInItems = await _unitOfWork.OrderItems.GetCountAsync(
                filter: oi => oi.OrderId == orderId
            );

            if (usedInItems > 0)
                return Result<bool>.Fail("Order has items and cannot be deleted directly without handling them");

            var deleted = _unitOfWork.Orders.Delete(order);
            if (!deleted)
                return Result<bool>.Fail("Failed to delete order");

            await _unitOfWork.CommitAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<IReadOnlyList<OrderDto>>> GetAllAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllEntitiesAsync(
                orderBy: q => q.OrderByDescending(o => o.OrderDate),
                disableTracking: true
            );

            var result = _mapper.Map<IReadOnlyList<OrderDto>>(orders);
            return Result<IReadOnlyList<OrderDto>>.Ok(result);
        }

        public async Task<Result<OrderDto>> GetAsync(Guid orderId)
        {
            var order = await _unitOfWork.Orders.GetSingleEntityAsync(
                filter: o => o.OrderId == orderId,
                disableTracking: true
            );

            if (order is null)
                return Result<OrderDto>.Fail("Order not found");

            var dto = _mapper.Map<OrderDto>(order);
            return Result<OrderDto>.Ok(dto);
        }
    }
}
