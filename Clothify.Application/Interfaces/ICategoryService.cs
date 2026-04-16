using Clothify.Application.DTOs;
using Clothify.Application.DTOs.Category;

namespace Clothify.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<Result<Guid>> AddAsync(CreateCategoryDto dto);
        Task<Result<bool>> UpdateAsync(UpdateCategoryDto dto);
        Task<Result<bool>> RemoveAsync(Guid categoryId);
        Task<Result<IReadOnlyList<CategoryDto>>> GetAllAsync();
        Task<Result<CategoryDto>> GetAsync(Guid categoryId);
    }
}
