using AutoMapper;
using Clothify.Application.DTOs;
using Clothify.Application.DTOs.Address;
using Clothify.Application.Interfaces;
using Clothify.Domain.Entities;
using Clothify.Domain.Interfaces;

namespace Clothify.Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddressService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // -------------------------
        // Add Address
        // -------------------------
        public async Task<Result<Guid>> AddAsync(Guid userId, CreateAddressDto dto)
        {
            var address = _mapper.Map<Address>(dto);
            address.UserId = userId;

            var added = await _unitOfWork.Addresses.AddAsync(address);
            if (!added)
                return Result<Guid>.Fail("Failed to add address");

            await _unitOfWork.CommitAsync();

            return Result<Guid>.Ok(address.AddressId);
        }

        // -------------------------
        // Update Address
        // -------------------------
        public async Task<Result<bool>> UpdateAsync(Guid userId, UpdateAddressDto dto)
        {
            var address = await _unitOfWork.Addresses.GetSingleEntityAsync(
                filter: a => a.AddressId == dto.AddressId && a.UserId == userId
            );

            if (address is null)
                return Result<bool>.Fail("Address not found");

            _mapper.Map(dto, address);

            var updated = _unitOfWork.Addresses.Update(address);
            if (!updated)
                return Result<bool>.Fail("Failed to update address");

            await _unitOfWork.CommitAsync();

            return Result<bool>.Ok(true);
        }

        // -------------------------
        // Remove Address
        // -------------------------
        public async Task<Result<bool>> RemoveAsync(Guid userId, Guid addressId)
        {
            var address = await _unitOfWork.Addresses.GetSingleEntityAsync(
                filter: a => a.AddressId == addressId && a.UserId == userId
            );

            if (address is null)
                return Result<bool>.Fail("Address not found");

            // Check if address is used in any order
            var isUsed = await _unitOfWork.Orders.GetCountAsync(
                filter: o => o.AddressId == addressId
            );

            if (isUsed > 0)
                return Result<bool>.Fail("Address is used in orders and cannot be deleted");

            var deleted = _unitOfWork.Addresses.Delete(address);
            if (!deleted)
                return Result<bool>.Fail("Failed to delete address");

            await _unitOfWork.CommitAsync();

            return Result<bool>.Ok(true);
        }

        // -------------------------
        // Get All User Addresses
        // -------------------------
        public async Task<Result<IReadOnlyList<AddressDto>>> GetAllAsync(Guid userId)
        {
            var addresses = await _unitOfWork.Addresses.GetAllEntitiesAsync(
                filter: a => a.UserId == userId,
                disableTracking: true
            );

            var result = _mapper.Map<IReadOnlyList<AddressDto>>(addresses);

            return Result<IReadOnlyList<AddressDto>>.Ok(result);
        }

        // -------------------------
        // Get Single Address
        // -------------------------
        public async Task<Result<AddressDto>> GetAsync(Guid userId, Guid addressId)
        {
            var address = await _unitOfWork.Addresses.GetSingleEntityAsync(
                filter: a => a.AddressId == addressId && a.UserId == userId,
                disableTracking: true
            );

            if (address is null)
                return Result<AddressDto>.Fail("Address not found");

            var dto = _mapper.Map<AddressDto>(address);

            return Result<AddressDto>.Ok(dto);
        }
    }
}
