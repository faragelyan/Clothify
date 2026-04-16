using AutoMapper;
using Clothify.Application.DTOs;
using Clothify.Application.DTOs.Size;
using Clothify.Application.Interfaces;
using Clothify.Domain.Entities;
using Clothify.Domain.Interfaces;

namespace Clothify.Application.Services
{
    public class SizeService : ISizeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SizeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Guid>> AddAsync(CreateSizeDto dto)
        {
            var exists = await _unitOfWork.Sizes.GetCountAsync(
                filter: s => s.Name.Trim().ToLower() == dto.Name.Trim().ToLower()
            );

            if (exists > 0)
                return Result<Guid>.Fail("Size already exists");

            var size = _mapper.Map<Size>(dto);
            size.Name = size.Name.Trim();

            var added = await _unitOfWork.Sizes.AddAsync(size);
            if (!added)
                return Result<Guid>.Fail("Failed to add size");

            await _unitOfWork.CommitAsync();
            return Result<Guid>.Ok(size.SizeId);
        }

        public async Task<Result<bool>> UpdateAsync(UpdateSizeDto dto)
        {
            var size = await _unitOfWork.Sizes.GetSingleEntityAsync(
                filter: s => s.SizeId == dto.SizeId
            );

            if (size is null)
                return Result<bool>.Fail("Size not found");

            var normalizedName = dto.Name.Trim().ToLower();
            var exists = await _unitOfWork.Sizes.GetCountAsync(
                filter: s => s.SizeId != dto.SizeId && s.Name.Trim().ToLower() == normalizedName
            );

            if (exists > 0)
                return Result<bool>.Fail("Size name already exists");

            _mapper.Map(dto, size);
            size.Name = size.Name.Trim();

            var updated = _unitOfWork.Sizes.Update(size);
            if (!updated)
                return Result<bool>.Fail("Failed to update size");

            await _unitOfWork.CommitAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> RemoveAsync(Guid sizeId)
        {
            var size = await _unitOfWork.Sizes.GetSingleEntityAsync(
                filter: s => s.SizeId == sizeId
            );

            if (size is null)
                return Result<bool>.Fail("Size not found");

            var usedInProducts = await _unitOfWork.ProductSizes.GetCountAsync(
                filter: ps => ps.SizeId == sizeId
            );

            if (usedInProducts > 0)
                return Result<bool>.Fail("Size is used in products and cannot be deleted");

            var deleted = _unitOfWork.Sizes.Delete(size);
            if (!deleted)
                return Result<bool>.Fail("Failed to delete size");

            await _unitOfWork.CommitAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<IReadOnlyList<SizeDto>>> GetAllAsync()
        {
            var sizes = await _unitOfWork.Sizes.GetAllEntitiesAsync(
                orderBy: q => q.OrderBy(s => s.Name),
                disableTracking: true
            );

            var result = _mapper.Map<IReadOnlyList<SizeDto>>(sizes);
            return Result<IReadOnlyList<SizeDto>>.Ok(result);
        }

        public async Task<Result<SizeDto>> GetAsync(Guid sizeId)
        {
            var size = await _unitOfWork.Sizes.GetSingleEntityAsync(
                filter: s => s.SizeId == sizeId,
                disableTracking: true
            );

            if (size is null)
                return Result<SizeDto>.Fail("Size not found");

            var dto = _mapper.Map<SizeDto>(size);
            return Result<SizeDto>.Ok(dto);
        }
    }
}
