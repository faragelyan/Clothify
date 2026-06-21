using Clothify.Application.DTOs.Payment;
using Clothify.Application.Interfaces;
using Clothify.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clothify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IPaymentGatewayService _paymentGatewayService;

        public PaymentController(IPaymentService paymentService, IPaymentGatewayService paymentGatewayService)
        {
            _paymentService = paymentService;
            _paymentGatewayService = paymentGatewayService;
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePaymentDto dto)
        {
            var result = await _paymentService.AddAsync(dto);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Payment created successfully.", data = result.Data });
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatePaymentDto dto)
        {
            var result = await _paymentService.UpdateAsync(dto);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Payment updated successfully.", data = result.Data });
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _paymentService.RemoveAsync(id);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Payment deleted successfully." });
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _paymentService.GetAllAsync();
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Payments retrieved successfully.", data = result.Data });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _paymentService.GetAsync(id);
            if (!result.Success)
                return NotFound(new { message = result.Error });

            return Ok(new { message = "Payment retrieved successfully.", data = result.Data });
        }

        [HttpPost("pay-with-wallet")]
        public async Task<IActionResult> PayWithWallet([FromBody] PayWithWalletDto dto)
        {
            // Payment Service handles orchestrating domain logic (validating order) and gateway dispatch
            var result = await _paymentService.PayWithWalletAsync(dto);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Wallet payment initialized successfully.", redirectUrl = result.Data });
        }

        [AllowAnonymous]
        [HttpPost("callback")]
        public async Task<IActionResult> PaymobCallback([FromQuery] string hmac, [FromBody] PaymobCallbackDto callback)
        {
            if (string.IsNullOrEmpty(hmac))
            {
                return Unauthorized(new { message = "Missing HMAC signature." });
            }

            var isValid = _paymentGatewayService.VerifyWebhookHmac(hmac, callback.obj);
            if (!isValid)
            {
                return Unauthorized(new { message = "Invalid HMAC signature." });
            }

            var result = await _paymentService.ProcessCallbackAsync(callback);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }

            return Ok(new { message = "Callback processed successfully." });
        }
    }
}
