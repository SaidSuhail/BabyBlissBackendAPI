using BabyBlissBackendAPI.ApiResponse;
using BabyBlissBackendAPI.Dto;

namespace BabyBlissBackendAPI.Services.CategoryServices
{
    public interface ICategoryServices
    {
        Task<List<CategoryDto>> GetCategories();
        Task<ApiResponse<CatAddDto>> AddCategory(CatAddDto categorty);

        Task<ApiResponse<string>> RemoveCategory(int id);
    }
}
