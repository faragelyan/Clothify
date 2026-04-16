using Clothify.Application.DTOs;
using Clothify.Application.DTOs.Order;

namespace Clothify.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Result<Guid>> AddAsync(CreateOrderDto dto);
        Task<Result<bool>> UpdateAsync(UpdateOrderDto dto);
        Task<Result<bool>> RemoveAsync(Guid orderId);
        Task<Result<IReadOnlyList<OrderDto>>> GetAllAsync();
        Task<Result<OrderDto>> GetAsync(Guid orderId);
    }
}
