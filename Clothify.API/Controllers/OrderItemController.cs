using Clothify.Application.DTOs.OrderItem;
using Clothify.Application.Interfaces;
using Clothify.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clothify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderItemDto dto)
        {
            var result = await _orderItemService.AddAsync(dto);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Order item created successfully.", data = result.Data });
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateOrderItemDto dto)
        {
            var result = await _orderItemService.UpdateAsync(dto);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Order item updated successfully.", data = result.Data });
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{orderId}/{productId}")]
        public async Task<IActionResult> Delete(Guid orderId, Guid productId)
        {
            var result = await _orderItemService.RemoveAsync(orderId, productId);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Order item deleted successfully." });
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetAllByOrderId(Guid orderId)
        {
            var result = await _orderItemService.GetAllByOrderIdAsync(orderId);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Order items retrieved successfully.", data = result.Data });
        }

        [HttpGet("{orderId}/{productId}")]
        public async Task<IActionResult> Get(Guid orderId, Guid productId)
        {
            var result = await _orderItemService.GetAsync(orderId, productId);
            if (!result.Success)
                return NotFound(new { message = result.Error });

            return Ok(new { message = "Order item retrieved successfully.", data = result.Data });
        }
    }
}
