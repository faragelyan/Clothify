using Clothify.Application.DTOs;
using Clothify.Application.DTOs.User;

namespace Clothify.Application.Interfaces
{
    public interface IAppUserService
    {
        Task<Result<Guid>> AddAsync(CreateUserDto dto);
        Task<Result<bool>> UpdateAsync(UpdateUserDto dto);
        Task<Result<bool>> RemoveAsync(Guid id);
        Task<Result<IReadOnlyList<UserDto>>> GetAllAsync();
        Task<Result<UserDto>> GetAsync(Guid id);
    }
}
