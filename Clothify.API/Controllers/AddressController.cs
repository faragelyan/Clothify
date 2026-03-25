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

        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId) || userId == Guid.Empty)
            {
                throw new UnauthorizedAccessException("Invalid or missing user id in token.");
            }
            return userId;
        }

        // -------------------------
        // POST: api/Address
        // Add new address
        // -------------------------
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAddressDto dto)
        {
            var userId = GetUserId();

            var result = await _addressService.AddAsync(userId, dto);

            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Data); // returns new AddressId
        }

        // -------------------------
        // PUT: api/Address
        // Update existing address
        // -------------------------
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAddressDto dto)
        {
            var userId = GetUserId();

            var result = await _addressService.UpdateAsync(userId, dto);

            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Data); // true if updated
        }

        // -------------------------
        // DELETE: api/Address/{id}
        // Remove address
        // -------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = GetUserId();

            var result = await _addressService.RemoveAsync(userId, id);

            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Data); // true if deleted
        }

        // -------------------------
        // GET: api/Address
        // Get all addresses of current user
        // -------------------------
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();

            var result = await _addressService.GetAllAsync(userId);

            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }

        // -------------------------
        // GET: api/Address/{id}
        // Get single address of current user
        // -------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var userId = GetUserId();

            var result = await _addressService.GetAsync(userId, id);

            if (!result.Success)
                return NotFound(result.Error);

            return Ok(result.Data);
        }
    }
}
