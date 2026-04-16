using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clothify.Application.DTOs;
using Clothify.Application.DTOs.Size;

namespace Clothify.Application.Interfaces
{
    public interface ISizeService
    {
        Task<Result<Guid>> AddAsync(CreateSizeDto dto);
        Task<Result<bool>> UpdateAsync(UpdateSizeDto dto);
        Task<Result<bool>> RemoveAsync(Guid sizeId);
        Task<Result<IReadOnlyList<SizeDto>>> GetAllAsync();
        Task<Result<SizeDto>> GetAsync(Guid sizeId);
    }
}
