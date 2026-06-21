using Clothify.Application.DTOs.Size;
using Clothify.Application.Interfaces;
using Clothify.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clothify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SizeController : ControllerBase
    {
        private readonly ISizeService _sizeService;

        public SizeController(ISizeService sizeService)
        {
            _sizeService = sizeService;
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSizeDto dto)
        {
            var result = await _sizeService.AddAsync(dto);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Size created successfully.", data = result.Data });
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateSizeDto dto)
        {
            var result = await _sizeService.UpdateAsync(dto);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Size updated successfully.", data = result.Data });
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _sizeService.RemoveAsync(id);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Size deleted successfully." });
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _sizeService.GetAllAsync();
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Sizes retrieved successfully.", data = result.Data });
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _sizeService.GetAsync(id);
            if (!result.Success)
                return NotFound(new { message = result.Error });

            return Ok(new { message = "Size retrieved successfully.", data = result.Data });
        }
    }
}
