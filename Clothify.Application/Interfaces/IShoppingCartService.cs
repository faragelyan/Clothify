using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clothify.Application.DTOs;
using Clothify.Application.DTOs.ShoppingCart;

namespace Clothify.Application.Interfaces
{
    public interface IShoppingCartService
    {
        Task<Result<Guid>> AddAsync(CreateShoppingCartDto dto);
        Task<Result<bool>> UpdateAsync(UpdateShoppingCartDto dto);
        Task<Result<bool>> RemoveAsync(Guid cartId);
        Task<Result<IReadOnlyList<ShoppingCartDto>>> GetAllAsync();
        Task<Result<ShoppingCartDto>> GetAsync(Guid cartId);
        Task<Result<ShoppingCartDto>> GetByUserIdAsync(Guid userId);
    }
}
