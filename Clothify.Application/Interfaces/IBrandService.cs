using Clothify.Application.DTOs;
using Clothify.Application.DTOs.Brand;

namespace Clothify.Application.Interfaces
{
    public interface IBrandService
    {
        Task<Result<Guid>> AddAsync(CreateBrandDto dto);
        Task<Result<bool>> UpdateAsync(UpdateBrandDto dto);
        Task<Result<bool>> RemoveAsync(Guid brandId);
        Task<Result<IReadOnlyList<BrandDto>>> GetAllAsync();
        Task<Result<BrandDto>> GetAsync(Guid brandId);
    }
}
