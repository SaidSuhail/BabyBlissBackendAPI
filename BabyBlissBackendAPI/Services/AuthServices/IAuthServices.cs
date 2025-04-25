using BabyBlissBackendAPI.Dto;

namespace BabyBlissBackendAPI.Services.AuthServices
{
    public interface IAuthServices
    {
        Task<bool> Register(UserRegistrationDto userRegistrationDto);
        Task<UserResponseDto> Login(UserLoginDto userLoginDto);
    }
}
