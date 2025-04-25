﻿using BabyBlissBackendAPI.Dto;

namespace BabyBlissBackendAPI.Services.ProductServices
{
    public interface IProductServices
    {
        Task AddProduct(AddProductDto addpro, IFormFile image);
        Task<List<ProductWithCategoryDto>> GetProducts();
        Task<List<ProductWithCategoryDto>> FeaturedPro();
        Task<ProductWithCategoryDto> GetProductById(int id);
        Task<List<ProductWithCategoryDto>> GetProductsByCategoryName(string categoryname);
        Task<bool> DeleteProduct(int id);
        Task UpdatePro(int id, UpdateProductDto addpro, IFormFile image);
        Task<List<ProductWithCategoryDto>> SearchProduct(string search);
        Task<List<ProductWithCategoryDto>> HotDeals();
    }
}
