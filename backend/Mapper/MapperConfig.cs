using AutoMapper;
using backend.Dto.Product;
using backend.Dto.Review;
using backend.Dto.Size;
using backend.Dto.User;
using backend.Entity;

namespace backend.Mapper;

public class MapperConfig : Profile
{
        public MapperConfig()
        {
                CreateMap<Size, SizeDto>();

                // Product
                CreateMap<ProductUpdateDto, Product>()
                        .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
                        .ForMember(dest => dest.Sizes, opt => opt.Ignore());
                CreateMap<ProductCreateDto, Product>()
                        .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
                        .ForMember(dest => dest.Sizes, opt => opt.Ignore());
                CreateMap<Product, ProductDto>()
                        .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(s => s.Category != null ? s.Category.Name : string.Empty))
                        .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.ProductImages.Select(pi => pi.ImageUrl)))
                        .ForMember(dest => dest.Sizes, opt => opt.MapFrom(src => src.Sizes.Select(s => new SizeDto
                        {
                                Name = s.Size != null ? s.Size.Name : string.Empty,
                                Inventory = s.Inventory
                        })));

                // User
                CreateMap<User, UserDto>();
                CreateMap<UserRegisterDto, User>()
                        .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
                CreateMap<UserUpdateDto, User>()
                        .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                        .ForMember(dest => dest.Role, opt => opt.Ignore());

                //Review
                CreateMap<ReviewCreateDto, Review>();
                CreateMap<ReviewUpdateDto, Review>();
                CreateMap<Review, ReviewDto>()
                        .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
                        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.Name : string.Empty));

        }

}
