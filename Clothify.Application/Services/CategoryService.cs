using AutoMapper;
using Clothify.Application.DTOs;
using Clothify.Application.DTOs.Category;
using Clothify.Application.Interfaces;
using Clothify.Domain.Entities;
using Clothify.Domain.Interfaces;

namespace Clothify.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Guid>> AddAsync(CreateCategoryDto dto)
        {
            var exists = await _unitOfWork.Categories.GetCountAsync(
                filter: c => c.Name.Trim().ToLower() == dto.Name.Trim().ToLower()
            );

            if (exists > 0)
                return Result<Guid>.Fail("Category name already exists");

            var category = _mapper.Map<Category>(dto);
            category.Name = category.Name.Trim();
            category.Description = category.Description.Trim();

            var added = await _unitOfWork.Categories.AddAsync(category);
            if (!added)
                return Result<Guid>.Fail("Failed to add category");

            await _unitOfWork.CommitAsync();
            return Result<Guid>.Ok(category.CategoryId);
        }

        public async Task<Result<bool>> UpdateAsync(UpdateCategoryDto dto)
        {
            var category = await _unitOfWork.Categories.GetSingleEntityAsync(
                filter: c => c.CategoryId == dto.CategoryId
            );

            if (category is null)
                return Result<bool>.Fail("Category not found");

            var normalizedName = dto.Name.Trim().ToLower();
            var exists = await _unitOfWork.Categories.GetCountAsync(
                filter: c => c.CategoryId != dto.CategoryId && c.Name.Trim().ToLower() == normalizedName
            );

            if (exists > 0)
                return Result<bool>.Fail("Category name already exists");

            _mapper.Map(dto, category);
            category.Name = category.Name.Trim();
            category.Description = category.Description.Trim();

            var updated = _unitOfWork.Categories.Update(category);
            if (!updated)
                return Result<bool>.Fail("Failed to update category");

            await _unitOfWork.CommitAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> RemoveAsync(Guid categoryId)
        {
            var category = await _unitOfWork.Categories.GetSingleEntityAsync(
                filter: c => c.CategoryId == categoryId
            );

            if (category is null)
                return Result<bool>.Fail("Category not found");

            var usedInProducts = await _unitOfWork.Products.GetCountAsync(
                filter: p => p.CategoryId == categoryId
            );

            if (usedInProducts > 0)
                return Result<bool>.Fail("Category is used in products and cannot be deleted");

            var deleted = _unitOfWork.Categories.Delete(category);
            if (!deleted)
                return Result<bool>.Fail("Failed to delete category");

            await _unitOfWork.CommitAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<IReadOnlyList<CategoryDto>>> GetAllAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllEntitiesAsync(
                orderBy: q => q.OrderBy(c => c.Name),
                disableTracking: true
            );

            var result = _mapper.Map<IReadOnlyList<CategoryDto>>(categories);
            return Result<IReadOnlyList<CategoryDto>>.Ok(result);
        }

        public async Task<Result<CategoryDto>> GetAsync(Guid categoryId)
        {
            var category = await _unitOfWork.Categories.GetSingleEntityAsync(
                filter: c => c.CategoryId == categoryId,
                disableTracking: true
            );

            if (category is null)
                return Result<CategoryDto>.Fail("Category not found");

            var dto = _mapper.Map<CategoryDto>(category);
            return Result<CategoryDto>.Ok(dto);
        }
    }
}
