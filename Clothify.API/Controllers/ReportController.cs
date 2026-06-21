using Clothify.Application.DTOs.Report;
using Clothify.Application.Interfaces;
using Clothify.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clothify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReportDto dto)
        {
            var result = await _reportService.AddAsync(dto);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Report created successfully.", data = result.Data });
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _reportService.RemoveAsync(id);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Report deleted successfully." });
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllByUserId(Guid userId)
        {
            var result = await _reportService.GetAllByUserIdAsync(userId);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Reports retrieved successfully.", data = result.Data });
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _reportService.GetAsync(id);
            if (!result.Success)
                return NotFound(new { message = result.Error });

            return Ok(new { message = "Report retrieved successfully.", data = result.Data });
        }
    }
}
