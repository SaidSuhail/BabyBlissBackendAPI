using BabyBlissBackendAPI.ApiResponse;
using BabyBlissBackendAPI.Dto;

namespace BabyBlissBackendAPI.Services.WishListServices
{
    public interface IWishListServices
    {
        Task<ApiResponse<string>> AddOrRemove(int u_id, int Pro_id);
        Task<ApiResponse<string>> RemoveFromWishList(int u_id, int pro_id);
        Task<List<WishListViewDto>> GetAllWishItems(int u_id);
    }
}
