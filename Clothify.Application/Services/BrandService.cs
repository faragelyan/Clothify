using AutoMapper;
using Clothify.Application.DTOs;
using Clothify.Application.DTOs.Brand;
using Clothify.Application.Interfaces;
using Clothify.Domain.Entities;
using Clothify.Domain.Interfaces;

namespace Clothify.Application.Services
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BrandService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Guid>> AddAsync(CreateBrandDto dto)
        {
            var exists = await _unitOfWork.Brands.GetCountAsync(
                filter: b => b.Name.Trim().ToLower() == dto.Name.Trim().ToLower()
            );

            if (exists > 0)
                return Result<Guid>.Fail("Brand name already exists");

            var brand = _mapper.Map<Brand>(dto);
            brand.Name = brand.Name.Trim();
            brand.Description = brand.Description.Trim();

            var added = await _unitOfWork.Brands.AddAsync(brand);
            if (!added)
                return Result<Guid>.Fail("Failed to add brand");

            await _unitOfWork.CommitAsync();
            return Result<Guid>.Ok(brand.BrandId);
        }

        public async Task<Result<bool>> UpdateAsync(UpdateBrandDto dto)
        {
            var brand = await _unitOfWork.Brands.GetSingleEntityAsync(
                filter: b => b.BrandId == dto.BrandId
            );

            if (brand is null)
                return Result<bool>.Fail("Brand not found");

            var normalizedName = dto.Name.Trim().ToLower();
            var exists = await _unitOfWork.Brands.GetCountAsync(
                filter: b => b.BrandId != dto.BrandId && b.Name.Trim().ToLower() == normalizedName
            );

            if (exists > 0)
                return Result<bool>.Fail("Brand name already exists");

            _mapper.Map(dto, brand);
            brand.Name = brand.Name.Trim();
            brand.Description = brand.Description.Trim();

            var updated = _unitOfWork.Brands.Update(brand);
            if (!updated)
                return Result<bool>.Fail("Failed to update brand");

            await _unitOfWork.CommitAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> RemoveAsync(Guid brandId)
        {
            var brand = await _unitOfWork.Brands.GetSingleEntityAsync(
                filter: b => b.BrandId == brandId
            );

            if (brand is null)
                return Result<bool>.Fail("Brand not found");

            var usedInProducts = await _unitOfWork.Products.GetCountAsync(
                filter: p => p.BrandId == brandId
            );

            if (usedInProducts > 0)
                return Result<bool>.Fail("Brand is used in products and cannot be deleted");

            var deleted = _unitOfWork.Brands.Delete(brand);
            if (!deleted)
                return Result<bool>.Fail("Failed to delete brand");

            await _unitOfWork.CommitAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<IReadOnlyList<BrandDto>>> GetAllAsync()
        {
            var brands = await _unitOfWork.Brands.GetAllEntitiesAsync(
                orderBy: q => q.OrderBy(b => b.Name),
                disableTracking: true
            );

            var result = _mapper.Map<IReadOnlyList<BrandDto>>(brands);
            return Result<IReadOnlyList<BrandDto>>.Ok(result);
        }

        public async Task<Result<BrandDto>> GetAsync(Guid brandId)
        {
            var brand = await _unitOfWork.Brands.GetSingleEntityAsync(
                filter: b => b.BrandId == brandId,
                disableTracking: true
            );

            if (brand is null)
                return Result<BrandDto>.Fail("Brand not found");

            var dto = _mapper.Map<BrandDto>(brand);
            return Result<BrandDto>.Ok(dto);
        }
    }
}
