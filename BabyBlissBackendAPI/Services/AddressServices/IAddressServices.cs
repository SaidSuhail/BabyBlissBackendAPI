using BabyBlissBackendAPI.Dto;

namespace BabyBlissBackendAPI.Services.AddressServices
{
    public interface IAddressServices
    {
        Task<bool> AddnewAddress(int userId, AddNewAddressDto address);
        Task<List<GetAddressDto>> GetAddress(int userId);
        Task<bool> RemoveAddress(int addId);
    }
}
