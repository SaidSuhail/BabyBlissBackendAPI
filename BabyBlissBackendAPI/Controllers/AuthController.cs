using BabyBlissBackendAPI.Dto;
using BabyBlissBackendAPI.Services.AuthServices;
using Microsoft.AspNetCore.Mvc;
using BabyBlissBackendAPI.ApiResponse;

namespace BabyBlissBackendAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController:ControllerBase
    {
        private readonly IAuthServices _authServices;
        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody]UserRegistrationDto newUser)
        {
            try
            {
                bool isdone = await _authServices.Register(newUser);
                if(!isdone)
                {
                    return BadRequest(new ApiResponse<string>(false, "User Already Exist", "[]", null));
                }
                return Ok(new ApiResponse<string>(true, "User Registered Successfully", "[done]", null));
            }
            catch(Exception ex)
            {
                return StatusCode(500, "server error ");
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult>Login(UserLoginDto login)
        {
            try
            {
                var res = await _authServices.Login(login);
                if(res.Error == "Not Found" )
                {
                    return NotFound("Email is not verified");
                }
                if(res.Error == "invalid password")
                {
                    return BadRequest(res.Error);
                }
                if(res.Error == "User Blocked")
                {
                    return StatusCode(403, "User is blocked by admin");
                }
                return Ok(res);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
