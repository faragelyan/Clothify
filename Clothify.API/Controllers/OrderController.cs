using Clothify.Application.DTOs.Order;
using Clothify.Application.Interfaces;
using Clothify.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clothify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {
            var result = await _orderService.AddAsync(dto);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Order created successfully.", data = result.Data });
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateOrderDto dto)
        {
            var result = await _orderService.UpdateAsync(dto);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Order updated successfully.", data = result.Data });
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _orderService.RemoveAsync(id);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Order deleted successfully." });
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _orderService.GetAllAsync();
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Orders retrieved successfully.", data = result.Data });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _orderService.GetAsync(id);
            if (!result.Success)
                return NotFound(new { message = result.Error });

            return Ok(new { message = "Order retrieved successfully.", data = result.Data });
        }
    }
}
