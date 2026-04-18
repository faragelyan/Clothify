using Clothify.Application.DTOs.User;
using Clothify.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clothify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IAppUserService _userService;

        public UserController(IAppUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            var result = await _userService.AddAsync(dto);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "User created successfully.", data = result.Data });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserDto dto)
        {
            var result = await _userService.UpdateAsync(dto);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "User updated successfully.", data = result.Data });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.RemoveAsync(id);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "User deleted successfully." });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllAsync();
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Users retrieved successfully.", data = result.Data });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _userService.GetAsync(id);
            if (!result.Success)
                return NotFound(new { message = result.Error });

            return Ok(new { message = "User retrieved successfully.", data = result.Data });
        }
    }
}
