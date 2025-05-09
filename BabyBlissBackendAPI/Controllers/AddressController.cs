using BabyBlissBackendAPI.Dto;
using BabyBlissBackendAPI.Services.AddressServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BabyBlissBackendAPI.ApiResponse;
namespace BabyBlissBackendAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController:ControllerBase
    {
        private readonly IAddressServices _addressServices;
        public AddressController(IAddressServices addressServices)
        {
            _addressServices = addressServices;
        }

        [HttpPost("Add-new-Address")]
        [Authorize]
        public async Task<IActionResult> Add_newAdd([FromBody]AddNewAddressDto _dto)
        {
            try
            {
                var user_id = Convert.ToInt32(HttpContext.Items["Id"]);
                var res = await _addressServices.AddnewAddress(user_id,_dto);          
                return Ok(new ApiResponse<string>(true, "Address Added Successfully", "[done]", null));
                
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpGet("GetAddresses")]
        [Authorize]
        public async Task<IActionResult>GetAddresses()
        {
            try
            {
                var user_id = Convert.ToInt32(HttpContext.Items["Id"]);
                var res = await _addressServices.GetAddress(user_id);
                if(res == null||!res.Any())
                {
                    return NotFound(new ApiResponse<string>(true, "No Address found for this user", "[]", null));
                }
                return Ok(new ApiResponse<IEnumerable<GetAddressDto>>(true, "fetched", res, null));
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("delete-address/{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult>deleteAddress(int id)
        {
            try
            {
                var res = await _addressServices.RemoveAddress(id);
                if(!res)
                {
                    return NotFound(new ApiResponse<string>(true, "Address removed", "[]", null));
                }
                return Ok(new ApiResponse<string>(true, "Address removed", "[]", null));
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

    }
}
