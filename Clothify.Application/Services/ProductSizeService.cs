using AutoMapper;
using Clothify.Application.DTOs;
using Clothify.Application.DTOs.ProductSize;
using Clothify.Application.Interfaces;
using Clothify.Domain.Entities;
using Clothify.Domain.Interfaces;

namespace Clothify.Application.Services
{
    public class ProductSizeService : IProductSizeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductSizeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<bool>> AddAsync(CreateProductSizeDto dto)
        {
            var exists = await _unitOfWork.ProductSizes.GetCountAsync(
                filter: ps => ps.ProductId == dto.ProductId && ps.SizeId == dto.SizeId
            );

            if (exists > 0)
                return Result<bool>.Fail("Product size already exists");

            var productSize = _mapper.Map<ProductSize>(dto);

            var added = await _unitOfWork.ProductSizes.AddAsync(productSize);
            if (!added)
                return Result<bool>.Fail("Failed to add product size");

            await _unitOfWork.CommitAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> RemoveAsync(Guid productId, Guid sizeId)
        {
            var productSize = await _unitOfWork.ProductSizes.GetSingleEntityAsync(
                filter: ps => ps.ProductId == productId && ps.SizeId == sizeId
            );

            if (productSize is null)
                return Result<bool>.Fail("Product size not found");

            var deleted = _unitOfWork.ProductSizes.Delete(productSize);
            if (!deleted)
                return Result<bool>.Fail("Failed to delete product size");

            await _unitOfWork.CommitAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<IReadOnlyList<ProductSizeDto>>> GetSizesByProductIdAsync(Guid productId)
        {
            var productSizes = await _unitOfWork.ProductSizes.GetAllEntitiesAsync(
                filter: ps => ps.ProductId == productId,
                disableTracking: true
            );

            var result = _mapper.Map<IReadOnlyList<ProductSizeDto>>(productSizes);
            return Result<IReadOnlyList<ProductSizeDto>>.Ok(result);
        }
    }
}
