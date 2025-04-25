﻿using AutoMapper;
using BabyBlissBackendAPI.CloudinaryServices;
using BabyBlissBackendAPI.Data;
using BabyBlissBackendAPI.Dto;
using BabyBlissBackendAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BabyBlissBackendAPI.Services.ProductServices
{
    public class ProductServices:IProductServices
    {
        private readonly AppDbContext _context;
        private readonly ICloudinaryServices _cloudinary;
        private readonly IMapper _mapper;
        public ProductServices(AppDbContext context, ICloudinaryServices cloudinary, IMapper mapper)
        {
            _context = context;
            _cloudinary = cloudinary;
            _mapper = mapper;
        }
        public async Task<List<ProductWithCategoryDto>> HotDeals()
        {
            try
            {
                var productWithCategory = await _context.products
                    .Where(a => (a.ProductPrice - a.offerPrize) > 200).ToListAsync();

                return _mapper.Map<List<ProductWithCategoryDto>>(productWithCategory);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ProductWithCategoryDto>> FeaturedPro()
        {
            try
            {
                var ProductWithCategory = await _context.products
                    .Where(c => c.Rating > 4).ToListAsync();

                return _mapper.Map<List<ProductWithCategoryDto>>(ProductWithCategory);

            }
            catch (Exception exp)
            {
                throw new Exception(exp.Message);
            }
        }
        public async Task AddProduct(AddProductDto addPro, IFormFile image)
        {
            try
            {
                string imageUrl = await _cloudinary.UploadImageAsync(image);
                var category = await _context.categories
                    .FirstOrDefaultAsync(c => c.Id == addPro.CategoryId);

                if (category == null)
                {
                    throw new Exception("Invalid Category ID");
                }
                var product = new Product
                {
                    ProductName = addPro.ProductName,
                    ProductDescription = addPro.ProductDescription,
                    ProductPrice = addPro.ProductPrice,
                    offerPrize = addPro.OfferPrize,
                    Rating = addPro.Rating,             
                    CategoryId = addPro.CategoryId,
                    ImageUrl = imageUrl,
                };
                await _context.products.AddAsync(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<ProductWithCategoryDto>> GetProducts()
        {
            try
            {
                var productWithCategory = await _context.products
                    .Include(x => x._Category)
                    .ToListAsync();
                if (productWithCategory == null)
                {
                    throw new Exception("Product is not exist");
                }
                return _mapper.Map<List<ProductWithCategoryDto>>(productWithCategory);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ProductWithCategoryDto> GetProductById(int id)
        {
            try
            {
                var product = await _context.products.FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    return null;
                }
                return _mapper.Map<ProductWithCategoryDto>(product);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<ProductWithCategoryDto>> GetProductsByCategoryName(string Cat_name)
        {
            try
            {
                if (Cat_name.ToLower() == "all")
                {
                    var allpro = await _context.products.ToListAsync();
                    if (allpro == null)
                    {
                        return null;
                    }
                    var products = _mapper.Map<ProductWithCategoryDto>(allpro);
                }

                var catP1 = await _context.products.Include(x => x._Category)
                    .Where(b => b._Category.CategoryName.ToLower() == Cat_name.ToLower()).ToListAsync();

                if (catP1 == null)
                {
                    return null;
                }

                return _mapper.Map<List<ProductWithCategoryDto>>(catP1);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteProduct(int id)
        {
            try
            {
                var isExists = await _context.products.FirstOrDefaultAsync(a => a.Id == id);
                if (isExists == null)
                {
                    return false;
                }

                _context.products.Remove(isExists);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdatePro(int id, UpdateProductDto addPro, IFormFile? image)
        {
            try
            {
                var pro = await _context.products.FirstOrDefaultAsync(x => x.Id == id);
                if (pro == null)
                    throw new Exception("Product not found");
                var CatExists = await _context.categories.FirstOrDefaultAsync(x => x.Id == addPro.CategoryId);

                if (addPro.CategoryId != null && CatExists == null)
                {
                    throw new Exception("Category not found");
                }

                if (!string.IsNullOrEmpty(addPro.ProductName)) pro.ProductName = addPro.ProductName;
                if (!string.IsNullOrEmpty(addPro.ProductDescription)) pro.ProductDescription = addPro.ProductDescription;
                if (addPro.ProductPrice != null) pro.ProductPrice = addPro.ProductPrice.Value;
                if (addPro.OfferPrize != null) pro.offerPrize = addPro.OfferPrize.Value;
                if (addPro.Rating != null) pro.Rating = addPro.Rating.Value;
                if (addPro.CategoryId != null) pro.CategoryId = addPro.CategoryId.Value;

                //if (pro != null)
                //{

                    //_mapper.Map(addPro, pro);


                    if (image != null && image.Length > 0)
                    {
                        string imgUrl = await _cloudinary.UploadImageAsync(image);
                        pro.ImageUrl = imgUrl;
                    }

                    await _context.SaveChangesAsync();
                //}
                //else
                //{
                //    throw new Exception($"Product with ID: {id} not found!");
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<ProductWithCategoryDto>> SearchProduct(string search)
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                {
                    return new List<ProductWithCategoryDto> { new ProductWithCategoryDto() };
                }

                var pro = await _context.products.Where(a => a.ProductName.ToLower().Contains(search.ToLower())).ToListAsync();

                return _mapper.Map<List<ProductWithCategoryDto>>(pro);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
