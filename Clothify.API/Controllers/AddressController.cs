using Clothify.Application.DTOs.Address;
using Clothify.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Clothify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        private bool TryGetUserId(out Guid userId)
        {
            userId = Guid.Empty;
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out userId) && userId != Guid.Empty;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAddressDto dto)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "Invalid or missing user id in token." });

            var result = await _addressService.AddAsync(userId, dto);

            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Address created successfully.", data = result.Data });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAddressDto dto)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "Invalid or missing user id in token." });

            var result = await _addressService.UpdateAsync(userId, dto);

            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Address updated successfully.", data = result.Data });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "Invalid or missing user id in token." });

            var result = await _addressService.RemoveAsync(userId, id);

            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Address deleted successfully." });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "Invalid or missing user id in token." });

            var result = await _addressService.GetAllAsync(userId);

            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Addresses retrieved successfully.", data = result.Data });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "Invalid or missing user id in token." });

            var result = await _addressService.GetAsync(userId, id);

            if (!result.Success)
                return NotFound(new { message = result.Error });

            return Ok(new { message = "Address retrieved successfully.", data = result.Data });
        }
    }
}
