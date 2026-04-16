using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clothify.Application.DTOs;
using Clothify.Application.DTOs.UserPhone;

namespace Clothify.Application.Interfaces
{
    public interface IUserPhoneService
    {
        Task<Result<Guid>> AddAsync(CreateUserPhoneDto dto);
        Task<Result<bool>> UpdateAsync(UpdateUserPhoneDto dto);
        Task<Result<bool>> RemoveAsync(Guid phoneId);
        Task<Result<IReadOnlyList<UserPhoneDto>>> GetAllByUserIdAsync(Guid userId);
        Task<Result<UserPhoneDto>> GetAsync(Guid phoneId);
    }
}
