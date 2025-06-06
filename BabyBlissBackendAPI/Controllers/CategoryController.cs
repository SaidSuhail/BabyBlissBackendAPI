﻿using BabyBlissBackendAPI.ApiResponse;
using BabyBlissBackendAPI.Dto;
using BabyBlissBackendAPI.Services.CategoryServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BabyBlissBackendAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController:ControllerBase
    {
        private readonly ICategoryServices _services;
        public CategoryController(ICategoryServices services)
        {
            _services = services;
        }
        [HttpGet("getCategories")]
        public async Task<IActionResult> GetCat()
        {
            try
            {
                var categoryList = await _services.GetCategories();
                return Ok(new ApiResponse<IEnumerable<CategoryDto>>(true, "categories fetched", categoryList, null));

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddCategory")]

        public async Task<IActionResult> Add_category(CatAddDto newcatgory)
        {
            try
            {
                var res = await _services.AddCategory(newcatgory);

                return Ok(res);

            }
            catch (Exception exc)
            {
                return StatusCode(500, exc.Message);

            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteCategory{id}")]

        public async Task<IActionResult> Delete_Cat(int id)
        {
            try
            {
                var res = await _services.RemoveCategory(id);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
