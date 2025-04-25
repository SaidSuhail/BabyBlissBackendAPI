using BabyBlissBackendAPI.Dto;

namespace BabyBlissBackendAPI.Services.UserServices
{
    public interface IUserService
    {
        Task<List<UserViewDto>> ListUsers();
        Task<UserViewDto> GetUser(int id);
        Task<BlockUnblockDto> BlockUnblockUser(int id);
    }
}
