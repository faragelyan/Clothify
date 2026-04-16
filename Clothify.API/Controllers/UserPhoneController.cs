using Clothify.Application.DTOs.UserPhone;
using Clothify.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clothify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserPhoneController : ControllerBase
    {
        private readonly IUserPhoneService _userPhoneService;

        public UserPhoneController(IUserPhoneService userPhoneService)
        {
            _userPhoneService = userPhoneService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserPhoneDto dto)
        {
            var result = await _userPhoneService.AddAsync(dto);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "User phone created successfully.", data = result.Data });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserPhoneDto dto)
        {
            var result = await _userPhoneService.UpdateAsync(dto);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "User phone updated successfully.", data = result.Data });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userPhoneService.RemoveAsync(id);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "User phone deleted successfully." });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllByUserId(Guid userId)
        {
            var result = await _userPhoneService.GetAllByUserIdAsync(userId);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "User phones retrieved successfully.", data = result.Data });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _userPhoneService.GetAsync(id);
            if (!result.Success)
                return NotFound(new { message = result.Error });

            return Ok(new { message = "User phone retrieved successfully.", data = result.Data });
        }
    }
}
