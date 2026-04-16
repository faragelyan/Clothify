using Clothify.Application.DTOs.ProductSize;
using Clothify.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clothify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductSizeController : ControllerBase
    {
        private readonly IProductSizeService _productSizeService;

        public ProductSizeController(IProductSizeService productSizeService)
        {
            _productSizeService = productSizeService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductSizeDto dto)
        {
            var result = await _productSizeService.AddAsync(dto);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Product size created successfully.", data = result.Data });
        }

        [HttpDelete("{productId}/{sizeId}")]
        public async Task<IActionResult> Delete(Guid productId, Guid sizeId)
        {
            var result = await _productSizeService.RemoveAsync(productId, sizeId);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Product size deleted successfully." });
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetSizesByProductId(Guid productId)
        {
            var result = await _productSizeService.GetSizesByProductIdAsync(productId);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Product sizes retrieved successfully.", data = result.Data });
        }
    }
}
