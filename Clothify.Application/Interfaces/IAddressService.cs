using Clothify.Application.DTOs;
using Clothify.Application.DTOs.Address;

namespace Clothify.Application.Interfaces
{
    public interface IAddressService
    {
        // Add new address for current user
        Task<Result<Guid>> AddAsync(Guid userId, CreateAddressDto dto);

        // Update existing address (only if owned by user)
        Task<Result<bool>> UpdateAsync(Guid userId, UpdateAddressDto dto);

        // Remove address (only if not used in active orders)
        Task<Result<bool>> RemoveAsync(Guid userId, Guid addressId);

        // Get all addresses of a user
        Task<Result<IReadOnlyList<AddressDto>>> GetAllAsync(Guid userId);

        // Get single address (ownership check)
        Task<Result<AddressDto>> GetAsync(Guid userId, Guid addressId);
    }
}
