using Clothify.Application.DTOs.Category;
using Clothify.Application.Interfaces;
using Clothify.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clothify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            var result = await _categoryService.AddAsync(dto);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Category created successfully.", data = result.Data });
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCategoryDto dto)
        {
            var result = await _categoryService.UpdateAsync(dto);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Category updated successfully.", data = result.Data });
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _categoryService.RemoveAsync(id);
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Category deleted successfully." });
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllAsync();
            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(new { message = "Categories retrieved successfully.", data = result.Data });
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _categoryService.GetAsync(id);
            if (!result.Success)
                return NotFound(new { message = result.Error });

            return Ok(new { message = "Category retrieved successfully.", data = result.Data });
        }
    }
}
