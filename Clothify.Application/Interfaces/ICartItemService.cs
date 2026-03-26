using Clothify.Application.DTOs;
using Clothify.Application.DTOs.CartItem;

namespace Clothify.Application.Interfaces
{
    public interface ICartItemService
    {
        Task<Result<Guid>> AddAsync(Guid userId, CreateCartItemDto dto);
        Task<Result<bool>> UpdateAsync(Guid userId, UpdateCartItemDto dto);
        Task<Result<bool>> RemoveAsync(Guid userId, Guid productId);
        Task<Result<IReadOnlyList<CartItemDto>>> GetAllAsync(Guid userId);
        Task<Result<CartItemDto>> GetAsync(Guid userId, Guid productId);
    }
}
