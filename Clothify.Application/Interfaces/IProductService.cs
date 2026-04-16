using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clothify.Application.DTOs;
using Clothify.Application.DTOs.Product;

namespace Clothify.Application.Interfaces
{
    public interface IProductService
    {
        Task<Result<Guid>> AddAsync(CreateProductDto dto);
        Task<Result<bool>> UpdateAsync(UpdateProductDto dto);
        Task<Result<bool>> RemoveAsync(Guid productId);
        Task<Result<IReadOnlyList<ProductDto>>> GetAllAsync();
        Task<Result<ProductDto>> GetAsync(Guid productId);
    }
}
