using Clothify.Application.DTOs.CartItem;
using Clothify.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Clothify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;

        public CartItemController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        private bool TryGetUserId(out Guid userId)
        {
            userId = Guid.Empty;
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out userId) && userId != Guid.Empty;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCartItemDto dto)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "Invalid or missing user id in token." });

            var result = await _cartItemService.AddAsync(userId, dto);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Cart item added successfully.", data = result.Data });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCartItemDto dto)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "Invalid or missing user id in token." });

            var result = await _cartItemService.UpdateAsync(userId, dto);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Cart item updated successfully.", data = result.Data });
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(Guid productId)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "Invalid or missing user id in token." });

            var result = await _cartItemService.RemoveAsync(userId, productId);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Cart item deleted successfully." });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "Invalid or missing user id in token." });

            var result = await _cartItemService.GetAllAsync(userId);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Cart items retrieved successfully.", data = result.Data });
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> Get(Guid productId)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "Invalid or missing user id in token." });

            var result = await _cartItemService.GetAsync(userId, productId);
            if (!result.Success)
                return NotFound(new { message = result.Error });

            return Ok(new { message = "Cart item retrieved successfully.", data = result.Data });
        }
    }
}

