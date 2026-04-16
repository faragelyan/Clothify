using AutoMapper;
using Clothify.Application.DTOs;
using Clothify.Application.DTOs.ShoppingCart;
using Clothify.Application.Interfaces;
using Clothify.Domain.Entities;
using Clothify.Domain.Interfaces;

namespace Clothify.Application.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ShoppingCartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Guid>> AddAsync(CreateShoppingCartDto dto)
        {
            var exists = await _unitOfWork.ShoppingCarts.GetCountAsync(
                filter: sc => sc.UserId == dto.UserId
            );

            if (exists > 0)
                return Result<Guid>.Fail("Shopping cart already exists for this user");

            var shoppingCart = _mapper.Map<ShoppingCart>(dto);
            shoppingCart.CreatedAt = DateTime.UtcNow;
            shoppingCart.TotalAmount = 0;

            var added = await _unitOfWork.ShoppingCarts.AddAsync(shoppingCart);
            if (!added)
                return Result<Guid>.Fail("Failed to add shopping cart");

            await _unitOfWork.CommitAsync();
            return Result<Guid>.Ok(shoppingCart.CartId);
        }

        public async Task<Result<bool>> UpdateAsync(UpdateShoppingCartDto dto)
        {
            var shoppingCart = await _unitOfWork.ShoppingCarts.GetSingleEntityAsync(
                filter: sc => sc.CartId == dto.CartId
            );

            if (shoppingCart is null)
                return Result<bool>.Fail("Shopping cart not found");

            _mapper.Map(dto, shoppingCart);

            var updated = _unitOfWork.ShoppingCarts.Update(shoppingCart);
            if (!updated)
                return Result<bool>.Fail("Failed to update shopping cart");

            await _unitOfWork.CommitAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> RemoveAsync(Guid cartId)
        {
            var shoppingCart = await _unitOfWork.ShoppingCarts.GetSingleEntityAsync(
                filter: sc => sc.CartId == cartId
            );

            if (shoppingCart is null)
                return Result<bool>.Fail("Shopping cart not found");

            var deleted = _unitOfWork.ShoppingCarts.Delete(shoppingCart);
            if (!deleted)
                return Result<bool>.Fail("Failed to delete shopping cart");

            await _unitOfWork.CommitAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<IReadOnlyList<ShoppingCartDto>>> GetAllAsync()
        {
            var shoppingCarts = await _unitOfWork.ShoppingCarts.GetAllEntitiesAsync(
                orderBy: q => q.OrderByDescending(sc => sc.CreatedAt),
                disableTracking: true
            );

            var result = _mapper.Map<IReadOnlyList<ShoppingCartDto>>(shoppingCarts);
            return Result<IReadOnlyList<ShoppingCartDto>>.Ok(result);
        }

        public async Task<Result<ShoppingCartDto>> GetAsync(Guid cartId)
        {
            var shoppingCart = await _unitOfWork.ShoppingCarts.GetSingleEntityAsync(
                filter: sc => sc.CartId == cartId,
                disableTracking: true
            );

            if (shoppingCart is null)
                return Result<ShoppingCartDto>.Fail("Shopping cart not found");

            var dto = _mapper.Map<ShoppingCartDto>(shoppingCart);
            return Result<ShoppingCartDto>.Ok(dto);
        }

        public async Task<Result<ShoppingCartDto>> GetByUserIdAsync(Guid userId)
        {
            var shoppingCart = await _unitOfWork.ShoppingCarts.GetSingleEntityAsync(
                filter: sc => sc.UserId == userId,
                disableTracking: true
            );

            if (shoppingCart is null)
                return Result<ShoppingCartDto>.Fail("Shopping cart not found for this user");

            var dto = _mapper.Map<ShoppingCartDto>(shoppingCart);
            return Result<ShoppingCartDto>.Ok(dto);
        }
    }
}
