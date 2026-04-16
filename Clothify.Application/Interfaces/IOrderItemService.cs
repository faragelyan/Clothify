using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clothify.Application.DTOs;
using Clothify.Application.DTOs.OrderItem;

namespace Clothify.Application.Interfaces
{
    public interface IOrderItemService
    {
        Task<Result<bool>> AddAsync(CreateOrderItemDto dto);
        Task<Result<bool>> UpdateAsync(UpdateOrderItemDto dto);
        Task<Result<bool>> RemoveAsync(Guid orderId, Guid productId);
        Task<Result<IReadOnlyList<OrderItemDto>>> GetAllByOrderIdAsync(Guid orderId);
        Task<Result<OrderItemDto>> GetAsync(Guid orderId, Guid productId);
    }
}
