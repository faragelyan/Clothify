using AutoMapper;
using Clothify.Application.DTOs;
using Clothify.Application.DTOs.CartItem;
using Clothify.Application.Interfaces;
using Clothify.Domain.Entities;
using Clothify.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Clothify.Application.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Guid>> AddAsync(Guid userId, CreateCartItemDto dto)
        {
            var product = await _unitOfWork.Products.GetSingleEntityAsync(
                filter: p => p.ProductId == dto.ProductId,
                disableTracking: true
            );

            if (product is null)
                return Result<Guid>.Fail("Product not found");

            if (product.Stock < dto.Quantity)
                return Result<Guid>.Fail("Insufficient stock");

            var cart = await GetOrCreateCartAsync(userId);

            var cartItem = await _unitOfWork.CartItems.GetSingleEntityAsync(
                filter: ci => ci.CartId == cart.CartId && ci.ProductId == dto.ProductId
            );

            if (cartItem is null)
            {
                var newItem = _mapper.Map<Domain.Entities.CartItem>(dto);
                newItem.CartId = cart.CartId;
                newItem.AddedAt = DateTime.UtcNow;

                var added = await _unitOfWork.CartItems.AddAsync(newItem);
                if (!added)
                    return Result<Guid>.Fail("Failed to add cart item");
            }
            else
            {
                var newQuantity = cartItem.Quantity + dto.Quantity;
                if (product.Stock < newQuantity)
                    return Result<Guid>.Fail("Insufficient stock");

                cartItem.Quantity = newQuantity;
                var updated = _unitOfWork.CartItems.Update(cartItem);
                if (!updated)
                    return Result<Guid>.Fail("Failed to update cart item quantity");
            }

            await RecalculateCartTotalAsync(cart.CartId);
            await _unitOfWork.CommitAsync();

            return Result<Guid>.Ok(dto.ProductId);
        }

        public async Task<Result<bool>> UpdateAsync(Guid userId, UpdateCartItemDto dto)
        {
            var cart = await _unitOfWork.ShoppingCarts.GetSingleEntityAsync(
                filter: c => c.UserId == userId
            );

            if (cart is null)
                return Result<bool>.Fail("Cart not found");

            var cartItem = await _unitOfWork.CartItems.GetSingleEntityAsync(
                filter: ci => ci.CartId == cart.CartId && ci.ProductId == dto.ProductId
            );

            if (cartItem is null)
                return Result<bool>.Fail("Cart item not found");

            var product = await _unitOfWork.Products.GetSingleEntityAsync(
                filter: p => p.ProductId == dto.ProductId,
                disableTracking: true
            );

            if (product is null)
                return Result<bool>.Fail("Product not found");

            if (product.Stock < dto.Quantity)
                return Result<bool>.Fail("Insufficient stock");

            cartItem.Quantity = dto.Quantity;
            var updated = _unitOfWork.CartItems.Update(cartItem);
            if (!updated)
                return Result<bool>.Fail("Failed to update cart item");

            await RecalculateCartTotalAsync(cart.CartId);
            await _unitOfWork.CommitAsync();

            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> RemoveAsync(Guid userId, Guid productId)
        {
            var cart = await _unitOfWork.ShoppingCarts.GetSingleEntityAsync(
                filter: c => c.UserId == userId
            );

            if (cart is null)
                return Result<bool>.Fail("Cart not found");

            var cartItem = await _unitOfWork.CartItems.GetSingleEntityAsync(
                filter: ci => ci.CartId == cart.CartId && ci.ProductId == productId
            );

            if (cartItem is null)
                return Result<bool>.Fail("Cart item not found");

            var deleted = _unitOfWork.CartItems.Delete(cartItem);
            if (!deleted)
                return Result<bool>.Fail("Failed to delete cart item");

            await RecalculateCartTotalAsync(cart.CartId);
            await _unitOfWork.CommitAsync();

            return Result<bool>.Ok(true);
        }

        public async Task<Result<IReadOnlyList<CartItemDto>>> GetAllAsync(Guid userId)
        {
            var cart = await _unitOfWork.ShoppingCarts.GetSingleEntityAsync(
                filter: c => c.UserId == userId,
                disableTracking: true
            );

            if (cart is null)
                return Result<IReadOnlyList<CartItemDto>>.Ok(Array.Empty<CartItemDto>());

            var items = await _unitOfWork.CartItems.GetAllEntitiesAsync(
                filter: ci => ci.CartId == cart.CartId,
                includes: q => q.Include(ci => ci.Product),
                orderBy: q => q.OrderByDescending(ci => ci.AddedAt),
                disableTracking: true
            );

            var result = _mapper.Map<IReadOnlyList<CartItemDto>>(items);
            return Result<IReadOnlyList<CartItemDto>>.Ok(result);
        }

        public async Task<Result<CartItemDto>> GetAsync(Guid userId, Guid productId)
        {
            var cart = await _unitOfWork.ShoppingCarts.GetSingleEntityAsync(
                filter: c => c.UserId == userId,
                disableTracking: true
            );

            if (cart is null)
                return Result<CartItemDto>.Fail("Cart not found");

            var item = await _unitOfWork.CartItems.GetSingleEntityAsync(
                filter: ci => ci.CartId == cart.CartId && ci.ProductId == productId,
                includes: q => q.Include(ci => ci.Product),
                disableTracking: true
            );

            if (item is null)
                return Result<CartItemDto>.Fail("Cart item not found");

            var dto = _mapper.Map<CartItemDto>(item);
            return Result<CartItemDto>.Ok(dto);
        }

        private async Task<ShoppingCart> GetOrCreateCartAsync(Guid userId)
        {
            var cart = await _unitOfWork.ShoppingCarts.GetSingleEntityAsync(
                filter: c => c.UserId == userId
            );

            if (cart != null)
                return cart;

            var newCart = new ShoppingCart
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                TotalAmount = 0m
            };

            var added = await _unitOfWork.ShoppingCarts.AddAsync(newCart);
            if (!added)
                throw new InvalidOperationException("Failed to create shopping cart");

            return newCart;
        }

        private async Task RecalculateCartTotalAsync(Guid cartId)
        {
            var cart = await _unitOfWork.ShoppingCarts.GetSingleEntityAsync(
                filter: c => c.CartId == cartId
            );

            if (cart is null)
                return;

            var items = await _unitOfWork.CartItems.GetAllEntitiesAsync(
                filter: ci => ci.CartId == cartId,
                includes: q => q.Include(ci => ci.Product),
                disableTracking: true
            );

            // Be null-safe in case cart items exist with missing/deleted products.
            var total = items.Sum(ci => (ci.Product?.Price ?? 0m) * ci.Quantity);
            cart.TotalAmount = total;
            _unitOfWork.ShoppingCarts.Update(cart);
        }
    }
}
