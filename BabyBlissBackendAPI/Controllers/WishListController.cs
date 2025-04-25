using BabyBlissBackendAPI.ApiResponse;
using BabyBlissBackendAPI.Dto;
using BabyBlissBackendAPI.Services.WishListServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BabyBlissBackendAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WishListController:ControllerBase
    {
        private readonly IWishListServices _services;
        public WishListController(IWishListServices services)
        {
            _services = services;
        }
        [HttpGet("GetWishList")]
        [Authorize]
        public async Task<IActionResult> GetWishList()
        {
            try
            {
                int userid = Convert.ToInt32(HttpContext.Items["Id"]);
                var res = await _services.GetAllWishItems(userid);


                if (res.Count > 0)
                {
                    return Ok(new ApiResponse<IEnumerable<WishListViewDto>>(true, "whislist fetched", res, null));
                }

                return Ok(new ApiResponse<IEnumerable<WishListViewDto>>(true, "no items in whislist ", res, null));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("AddOrRemove/{productid}")]
        [Authorize]
        public async Task<IActionResult> Add(int productid)
        {
            try
            {
                int userid = Convert.ToInt32(HttpContext.Items["Id"]);
                var res = await _services.AddOrRemove(userid, productid);

                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("RemoveWishlist/{productid}")]
        [Authorize]
        public async Task<IActionResult> remove(int productid)
        {
            try
            {
                int userid = Convert.ToInt32(HttpContext.Items["Id"]);
                var res = await _services.RemoveFromWishList(userid, productid);

                if (res.IsSuccess)
                {
                    return Ok(res);
                }

                return BadRequest(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
