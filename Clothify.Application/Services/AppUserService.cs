using AutoMapper;
using Clothify.Application.DTOs;
using Clothify.Application.DTOs.User;
using Clothify.Application.Interfaces;
using Clothify.Domain.Entities;
using Clothify.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Clothify.Application.Services
{
    public class AppUserService : IAppUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public AppUserService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<Result<Guid>> AddAsync(CreateUserDto dto)
        {
            var exists = await _userManager.FindByEmailAsync(dto.Email);
            if (exists != null)
                return Result<Guid>.Fail("User with this email already exists.");

            var user = _mapper.Map<AppUser>(dto);
            user.UserName = dto.Email; // Email as username
            
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return Result<Guid>.Fail(string.Join(", ", result.Errors.Select(e => e.Description)));

            return Result<Guid>.Ok(user.Id);
        }

        public async Task<Result<bool>> UpdateAsync(UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.Id.ToString());
            if (user == null)
                return Result<bool>.Fail("User not found.");

            var emailExists = await _userManager.FindByEmailAsync(dto.Email);
            if (emailExists != null && emailExists.Id != dto.Id)
                return Result<bool>.Fail("Another user with this email already exists.");

            _mapper.Map(dto, user);
            user.UserName = dto.Email; 

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return Result<bool>.Fail(string.Join(", ", result.Errors.Select(e => e.Description)));

            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> RemoveAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return Result<bool>.Fail("User not found.");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return Result<bool>.Fail(string.Join(", ", result.Errors.Select(e => e.Description)));

            return Result<bool>.Ok(true);
        }

        public async Task<Result<IReadOnlyList<UserDto>>> GetAllAsync()
        {
            var users = await _unitOfWork.AppUsers.GetAllEntitiesAsync(
                disableTracking: true
            );

            var result = _mapper.Map<IReadOnlyList<UserDto>>(users);
            return Result<IReadOnlyList<UserDto>>.Ok(result);
        }

        public async Task<Result<UserDto>> GetAsync(Guid id)
        {
            var user = await _unitOfWork.AppUsers.GetSingleEntityAsync(
                filter: u => u.Id == id,
                disableTracking: true
            );

            if (user == null)
                return Result<UserDto>.Fail("User not found.");

            var dto = _mapper.Map<UserDto>(user);
            return Result<UserDto>.Ok(dto);
        }
    }
}
