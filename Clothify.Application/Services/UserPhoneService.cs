using AutoMapper;
using Clothify.Application.DTOs;
using Clothify.Application.DTOs.UserPhone;
using Clothify.Application.Interfaces;
using Clothify.Domain.Entities;
using Clothify.Domain.Interfaces;

namespace Clothify.Application.Services
{
    public class UserPhoneService : IUserPhoneService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserPhoneService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Guid>> AddAsync(CreateUserPhoneDto dto)
        {
            var userPhone = _mapper.Map<UserPhone>(dto);

            var added = await _unitOfWork.UserPhones.AddAsync(userPhone);
            if (!added)
                return Result<Guid>.Fail("Failed to add user phone");

            await _unitOfWork.CommitAsync();
            return Result<Guid>.Ok(userPhone.PhoneId);
        }

        public async Task<Result<bool>> UpdateAsync(UpdateUserPhoneDto dto)
        {
            var userPhone = await _unitOfWork.UserPhones.GetSingleEntityAsync(
                filter: p => p.PhoneId == dto.PhoneId
            );

            if (userPhone is null)
                return Result<bool>.Fail("User phone not found");

            _mapper.Map(dto, userPhone);

            var updated = _unitOfWork.UserPhones.Update(userPhone);
            if (!updated)
                return Result<bool>.Fail("Failed to update user phone");

            await _unitOfWork.CommitAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> RemoveAsync(Guid phoneId)
        {
            var userPhone = await _unitOfWork.UserPhones.GetSingleEntityAsync(
                filter: p => p.PhoneId == phoneId
            );

            if (userPhone is null)
                return Result<bool>.Fail("User phone not found");

            var deleted = _unitOfWork.UserPhones.Delete(userPhone);
            if (!deleted)
                return Result<bool>.Fail("Failed to delete user phone");

            await _unitOfWork.CommitAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<IReadOnlyList<UserPhoneDto>>> GetAllByUserIdAsync(Guid userId)
        {
            var phones = await _unitOfWork.UserPhones.GetAllEntitiesAsync(
                filter: p => p.UserId == userId,
                disableTracking: true
            );

            var result = _mapper.Map<IReadOnlyList<UserPhoneDto>>(phones);
            return Result<IReadOnlyList<UserPhoneDto>>.Ok(result);
        }

        public async Task<Result<UserPhoneDto>> GetAsync(Guid phoneId)
        {
            var userPhone = await _unitOfWork.UserPhones.GetSingleEntityAsync(
                filter: p => p.PhoneId == phoneId,
                disableTracking: true
            );

            if (userPhone is null)
                return Result<UserPhoneDto>.Fail("User phone not found");

            var dto = _mapper.Map<UserPhoneDto>(userPhone);
            return Result<UserPhoneDto>.Ok(dto);
        }
    }
}
