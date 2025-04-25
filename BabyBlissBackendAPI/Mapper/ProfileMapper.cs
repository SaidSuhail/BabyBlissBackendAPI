using AutoMapper;
using BabyBlissBackendAPI.Dto;
using BabyBlissBackendAPI.Models;
namespace BabyBlissBackendAPI.Mapper
{
    public class ProfileMapper:Profile
    {
        public ProfileMapper()
        {
            CreateMap<User, UserRegistrationDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CatAddDto>().ReverseMap();
            CreateMap<Product, ProductWithCategoryDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src._Category.CategoryName))
                .ReverseMap();
            //CreateMap<UpdateProductDto,Product>()
            //    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Product, AddProductDto>().ReverseMap();
            CreateMap<CartItems, CartViewDto>().ReverseMap();
            CreateMap<WishList, WishListDto>().ReverseMap();
            CreateMap<UserAddress, AddNewAddressDto>().ReverseMap();
            CreateMap<UserAddress, GetAddressDto>().ReverseMap();
            CreateMap<User, UserViewDto>().ReverseMap();
        }
    }
}
