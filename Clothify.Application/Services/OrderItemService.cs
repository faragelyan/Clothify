using AutoMapper;
using Clothify.Application.DTOs;
using Clothify.Application.DTOs.OrderItem;
using Clothify.Application.Interfaces;
using Clothify.Domain.Entities;
using Clothify.Domain.Interfaces;

namespace Clothify.Application.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<bool>> AddAsync(CreateOrderItemDto dto)
        {
            var exists = await _unitOfWork.OrderItems.GetCountAsync(
                filter: oi => oi.OrderId == dto.OrderId && oi.ProductId == dto.ProductId
            );

            if (exists > 0)
                return Result<bool>.Fail("Order item already exists");

            var orderItem = _mapper.Map<OrderItem>(dto);

            var added = await _unitOfWork.OrderItems.AddAsync(orderItem);
            if (!added)
                return Result<bool>.Fail("Failed to add order item");

            await _unitOfWork.CommitAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> UpdateAsync(UpdateOrderItemDto dto)
        {
            var orderItem = await _unitOfWork.OrderItems.GetSingleEntityAsync(
                filter: oi => oi.OrderId == dto.OrderId && oi.ProductId == dto.ProductId
            );

            if (orderItem is null)
                return Result<bool>.Fail("Order item not found");

            _mapper.Map(dto, orderItem);

            var updated = _unitOfWork.OrderItems.Update(orderItem);
            if (!updated)
                return Result<bool>.Fail("Failed to update order item");

            await _unitOfWork.CommitAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> RemoveAsync(Guid orderId, Guid productId)
        {
            var orderItem = await _unitOfWork.OrderItems.GetSingleEntityAsync(
                filter: oi => oi.OrderId == orderId && oi.ProductId == productId
            );

            if (orderItem is null)
                return Result<bool>.Fail("Order item not found");

            var deleted = _unitOfWork.OrderItems.Delete(orderItem);
            if (!deleted)
                return Result<bool>.Fail("Failed to delete order item");

            await _unitOfWork.CommitAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<IReadOnlyList<OrderItemDto>>> GetAllByOrderIdAsync(Guid orderId)
        {
            var orderItems = await _unitOfWork.OrderItems.GetAllEntitiesAsync(
                filter: oi => oi.OrderId == orderId,
                disableTracking: true
            );

            var result = _mapper.Map<IReadOnlyList<OrderItemDto>>(orderItems);
            return Result<IReadOnlyList<OrderItemDto>>.Ok(result);
        }

        public async Task<Result<OrderItemDto>> GetAsync(Guid orderId, Guid productId)
        {
            var orderItem = await _unitOfWork.OrderItems.GetSingleEntityAsync(
                filter: oi => oi.OrderId == orderId && oi.ProductId == productId,
                disableTracking: true
            );

            if (orderItem is null)
                return Result<OrderItemDto>.Fail("Order item not found");

            var dto = _mapper.Map<OrderItemDto>(orderItem);
            return Result<OrderItemDto>.Ok(dto);
        }
    }
}
