using AutoMapper;
using BabyBlissBackendAPI.ApiResponse;
using BabyBlissBackendAPI.Data;
using BabyBlissBackendAPI.Dto;
using BabyBlissBackendAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace BabyBlissBackendAPI.Services.WishListServices
{
    public class WishListServices:IWishListServices
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public WishListServices(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> AddOrRemove(int userid, int productid)
        {
            try
            {
                var isExists = await _context.wishlist
                    .Include(a => a._Product)
                    .FirstOrDefaultAsync(b => b.ProductId == productid && b.UserId == userid);

                if (isExists == null)
                {
                    WishListDto wishListDto = new WishListDto()
                    {
                        UserId = userid,
                        ProductId = productid,
                    };
                    var wish = _mapper.Map<WishList>(wishListDto);
                    _context.wishlist.Add(wish);
                    await _context.SaveChangesAsync();
                    return new ApiResponse<string>(true,"Item added to the wishList", "done", null);
                }
                else
                {
                    _context.wishlist.Remove(isExists);
                    await _context.SaveChangesAsync();

                    return new ApiResponse<string>(true, "Item removed from wishList", "done", null);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while updating the wishlist.", ex);
            }
        }


        public async Task<List<WishListViewDto>> GetAllWishItems(int userid)
        {
            try
            {
                var items = await _context.wishlist
                    .Include(a => a._Product)
                    .ThenInclude(b => b._Category)
                    .Where(c => c.UserId == userid)
                    .ToListAsync();

                if (items != null)
                {
                    var p = items.Select(a => new WishListViewDto
                    {
                        Id = a.Id,
                        ProductId = a._Product.Id,
                        ProductName = a._Product.ProductName,
                        ProductDescription = a._Product.ProductDescription,
                        Price = a._Product.ProductPrice,
                        OfferPrice = a._Product.offerPrize,
                        ProductImage = a._Product.ImageUrl,
                        CategoryName = a._Product._Category?.CategoryName
                    }).ToList();

                    return p;
                }
                else
                {
                    return new List<WishListViewDto>();
                }

            }

            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }


        //public async Task<ApiResponse<string>> RemoveFromWishList(int userid, int productid)
        //{
        //    try
        //    {
        //        var isExists = await _context.wishlist
        //          .Include(a => a._Product)
        //          .FirstOrDefaultAsync(b => b.Id == productid && b.UserId == userid);

        //        if (isExists != null)
        //        {
        //            _context.wishlist.Remove(isExists);
        //            await _context.SaveChangesAsync();
        //            return new ApiResponse<string>(true, "Item removed from wishList", "done", null);
        //        }

        //        return new ApiResponse<string>(false, "Product not found", "", null);


        //    }
        //    catch (Exception ex)
        //    {
        //        throw new InvalidOperationException(ex.Message);
        //    }
        //}

        public async Task<ApiResponse<string>> RemoveFromWishList(int userid, int productid)
        {
            try
            {
                // Debug: Log the received parameters
                Console.WriteLine($"Received request to remove product {productid} from wishlist for user {userid}");

                // Query to check if the product exists in the user's wishlist
                var isExists = await _context.wishlist
                    .Where(w => w.UserId == userid && w.ProductId == productid) // Ensure both userId and productId match
                    .FirstOrDefaultAsync();

                if (isExists != null)
                {
                    _context.wishlist.Remove(isExists);
                    await _context.SaveChangesAsync();
                    return new ApiResponse<string>(true, "Item removed from wishList", "done", null);
                }

                // Debug: Log the result if product not found
                Console.WriteLine($"Product with id {productid} for user {userid} not found in wishlist.");

                return new ApiResponse<string>(false, $"Product with id {productid} for user {userid} not found", "", null);
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"Error occurred: {ex.Message}");

                // Return the error response with exception message
                return new ApiResponse<string>(false, "An error occurred while removing from the wishlist", ex.Message, null);
            }
        }

    }
}
