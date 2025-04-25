using BabyBlissBackendAPI.ApiResponse;
using BabyBlissBackendAPI.Dto;

namespace BabyBlissBackendAPI.Services.CartServices
{
    public interface ICartService
    {
        Task<CartWithTotalPrice> GetAllCartItems(int userId);
        Task<ApiResponse<CartViewDto>> AddToCart(int userId, int productId);
        Task<ApiResponse<string>> RemoveFromCart(int userId, int ProductId);
        Task<ApiResponse<CartViewDto>> IncraseQuantity(int userId, int productId);
        Task<ApiResponse<CartViewDto>> DecreaseQuantity(int userId, int ProductId);
    }
}
