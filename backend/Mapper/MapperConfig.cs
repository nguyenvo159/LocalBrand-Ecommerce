using AutoMapper;
using backend.Dto.Product;
using backend.Dto.Size;
using backend.Entity;

namespace backend.Mapper;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<Size, SizeDto>();
        CreateMap<ProductCreateDto, Product>()
                .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
                .ForMember(dest => dest.Sizes, opt => opt.Ignore()); ;
        CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(s => s.Category.Name))
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.ProductImages.Select(pi => pi.ImageUrl)))
                .ForMember(dest => dest.Sizes, opt => opt.MapFrom(src => src.Sizes.Select(s => new SizeDto
                {
                    Name = s.Size.Name,
                    Inventory = s.Inventory
                })));

    }

}
