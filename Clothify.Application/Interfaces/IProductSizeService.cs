using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clothify.Application.DTOs;
using Clothify.Application.DTOs.ProductSize;

namespace Clothify.Application.Interfaces
{
    public interface IProductSizeService
    {
        Task<Result<bool>> AddAsync(CreateProductSizeDto dto);
        Task<Result<bool>> RemoveAsync(Guid productId, Guid sizeId);
        Task<Result<IReadOnlyList<ProductSizeDto>>> GetSizesByProductIdAsync(Guid productId);
    }
}
