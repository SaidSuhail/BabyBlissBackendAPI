using BabyBlissBackendAPI.ApiResponse;
using BabyBlissBackendAPI.Dto;
using BabyBlissBackendAPI.Services.ProductServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BabyBlissBackendAPI.Controllers
{
    public class ProductController:ControllerBase
    {
        private readonly IProductServices _Services;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductServices services, ILogger<ProductController> logger)
        {
            _Services = services;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Add-Products")]
        public async Task<IActionResult> AddPro([FromForm] AddProductDto new_pro, IFormFile image)
        {
            try
            {

                if (new_pro == null || image == null)
                {
                    return BadRequest("Invalid product data or image file.");
                }


                if (image.Length > 10485760)
                {
                    return BadRequest("File size exceeds the 10 MB limit.");
                }

                if (!image.ContentType.StartsWith("image/"))
                {
                    return BadRequest("Invalid file type. Only image files are allowed.");
                }

                await _Services.AddProduct(new_pro, image);
                return Ok(new ApiResponse<string>(true, "Product added successfully!", null, null));
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a product.");


                return StatusCode(500, ex.InnerException?.Message);
            }
        }

       

        [HttpGet("All Products")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _Services.GetProducts();
                return Ok(new ApiResponse<IEnumerable<ProductWithCategoryDto>>(true, "Products fetched ", products, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var p = await _Services.GetProductById(id);

                if (p == null)
                {
                    return Ok(new ApiResponse<string>(false, "Product not found", " ", null));
                }
                return Ok(new ApiResponse<ProductWithCategoryDto>(true, "Product  found", p, null));

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("GetByCategoryName")]
        public async Task<IActionResult> GetByCateName(string CatName)
        {
            try
            {
                var p = await _Services.GetProductsByCategoryName(CatName);
                if (p == null)
                {
                    return Ok(new ApiResponse<string>(false, "No products in this category", " ", null));
                }
                return Ok(new ApiResponse<IEnumerable<ProductWithCategoryDto>>(true, " products in this category", p, null));

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


       


        [HttpGet("Search-item")]
        public async Task<IActionResult> SearchPro(string search)
        {
            try
            {
                var res = await _Services.SearchProduct(search);
                if (res == null||!res.Any())
                {
                    return NotFound(new ApiResponse<string>(true, "no products matched", null, null));

                }
                return Ok(new ApiResponse<IEnumerable<ProductWithCategoryDto>>(true, " products are match with..", res, null));

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("HotDeals")]
        public async Task<IActionResult> HotDeals()
        {
            try
            {
                var res = await _Services.HotDeals();
                return Ok(new ApiResponse<IEnumerable<ProductWithCategoryDto>>(true, "Product fetched", res, null));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("FeaturedProducts")]
        public async Task<IActionResult> FeturedPro()
        {
            try
            {
                var res = await _Services.FeaturedPro();
                return Ok(new ApiResponse<IEnumerable<ProductWithCategoryDto>>(true, "Product fetched", res, null));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }







        [Authorize(Roles = "Admin")]
        [HttpPut("Update-Products/{id}")]
        public async Task<IActionResult> Update_pro(int id, [FromForm] UpdateProductDto updateProduct_Dto, IFormFile? image)
        {
            try
            {
                await _Services.UpdatePro(id, updateProduct_Dto, image);

                return Ok(new ApiResponse<string>(true, "product updated", null, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePro(int id)
        {
            try
            {
                bool res = await _Services.DeleteProduct(id);
                if (res)
                {
                    return Ok(new ApiResponse<string>(true, "Product deleted", null, null));
                }

                return NotFound(new ApiResponse<string>(false, "Product Not found", null, null));

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("paginated")]
        public async Task<ActionResult<PagedResponseDTO<ProductViewDto>>> GetPaginatedProducts(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var result = await _Services.GetPaginatedProducts(pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
