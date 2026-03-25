using Clothify.Application.DTOs.Brand;
using Clothify.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clothify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBrandDto dto)
        {
            var result = await _brandService.AddAsync(dto);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Brand created successfully.", data = result.Data });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBrandDto dto)
        {
            var result = await _brandService.UpdateAsync(dto);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Brand updated successfully.", data = result.Data });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _brandService.RemoveAsync(id);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Brand deleted successfully." });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _brandService.GetAllAsync();
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Brands retrieved successfully.", data = result.Data });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _brandService.GetAsync(id);
            if (!result.Success)
                return NotFound(new { message = result.Error });

            return Ok(new { message = "Brand retrieved successfully.", data = result.Data });
        }
    }
}
