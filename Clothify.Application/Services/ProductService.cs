using AutoMapper;
using Clothify.Application.DTOs;
using Clothify.Application.DTOs.Product;
using Clothify.Application.Interfaces;
using Clothify.Domain.Entities;
using Clothify.Domain.Interfaces;

namespace Clothify.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Guid>> AddAsync(CreateProductDto dto)
        {
            var exists = await _unitOfWork.Products.GetCountAsync(
                filter: p => p.Name.Trim().ToLower() == dto.Name.Trim().ToLower()
            );

            if (exists > 0)
                return Result<Guid>.Fail("Product name already exists");

            var product = _mapper.Map<Product>(dto);
            product.Name = product.Name.Trim();
            product.Description = product.Description.Trim();
            product.CreatedAt = DateTime.UtcNow;

            var added = await _unitOfWork.Products.AddAsync(product);
            if (!added)
                return Result<Guid>.Fail("Failed to add product");

            await _unitOfWork.CommitAsync();
            return Result<Guid>.Ok(product.ProductId);
        }

        public async Task<Result<bool>> UpdateAsync(UpdateProductDto dto)
        {
            var product = await _unitOfWork.Products.GetSingleEntityAsync(
                filter: p => p.ProductId == dto.ProductId
            );

            if (product is null)
                return Result<bool>.Fail("Product not found");

            var normalizedName = dto.Name.Trim().ToLower();
            var exists = await _unitOfWork.Products.GetCountAsync(
                filter: p => p.ProductId != dto.ProductId && p.Name.Trim().ToLower() == normalizedName
            );

            if (exists > 0)
                return Result<bool>.Fail("Product name already exists");

            _mapper.Map(dto, product);
            product.Name = product.Name.Trim();
            product.Description = product.Description.Trim();

            var updated = _unitOfWork.Products.Update(product);
            if (!updated)
                return Result<bool>.Fail("Failed to update product");

            await _unitOfWork.CommitAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> RemoveAsync(Guid productId)
        {
            var product = await _unitOfWork.Products.GetSingleEntityAsync(
                filter: p => p.ProductId == productId
            );

            if (product is null)
                return Result<bool>.Fail("Product not found");

            var usedInOrders = await _unitOfWork.OrderItems.GetCountAsync(
                filter: oi => oi.ProductId == productId
            );

            if (usedInOrders > 0)
                return Result<bool>.Fail("Product is used in orders and cannot be deleted");

            var deleted = _unitOfWork.Products.Delete(product);
            if (!deleted)
                return Result<bool>.Fail("Failed to delete product");

            await _unitOfWork.CommitAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<IReadOnlyList<ProductDto>>> GetAllAsync()
        {
            var products = await _unitOfWork.Products.GetAllEntitiesAsync(
                orderBy: q => q.OrderByDescending(p => p.Name),
                disableTracking: true
            );

            var result = _mapper.Map<IReadOnlyList<ProductDto>>(products);
            return Result<IReadOnlyList<ProductDto>>.Ok(result);
        }

        public async Task<Result<ProductDto>> GetAsync(Guid productId)
        {
            var product = await _unitOfWork.Products.GetSingleEntityAsync(
                filter: p => p.ProductId == productId,
                disableTracking: true
            );

            if (product is null)
                return Result<ProductDto>.Fail("Product not found");

            var dto = _mapper.Map<ProductDto>(product);
            return Result<ProductDto>.Ok(dto);
        }
    }
}
