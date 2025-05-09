using BabyBlissBackendAPI.Services.UserServices;
using Microsoft.AspNetCore.Mvc;
using BabyBlissBackendAPI.ApiResponse;
using BabyBlissBackendAPI.Dto;
using Microsoft.AspNetCore.Authorization;
namespace BabyBlissBackendAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController:ControllerBase
    {
        private readonly IUserService _services;
        public UserController(IUserService services)
        {
            _services = services;
        }
        [Authorize(Roles ="Admin")]
        [HttpGet("Get-All-Users")]
        public async Task<IActionResult>getUsers()
        {
            try
            {
                var users = await _services.ListUsers();
                if(users == null)
                {
                    return NotFound(new ApiResponse<string>(false, "No Users In this List", "", null));
                }
                return Ok(new ApiResponse<IEnumerable<UserViewDto>>(true, "Done", users, null));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpGet("UserById/{id}")]
        public async Task<IActionResult>UserById(int id)
        {
            try
            {
                var user = await _services.GetUser(id);

                if(user == null)
                {
                    return NotFound(new ApiResponse<string>(false, "No Matched users in the list", "", null));
                }
                return Ok(new ApiResponse<UserViewDto>(true, "done", user, null));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("Block/Unblock/{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult>block_unb(int id)
        {
            try
            {
                var res = await _services.BlockUnblockUser(id);
                return Ok(new ApiResponse<BlockUnblockDto>(true, "updated", res, null));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
