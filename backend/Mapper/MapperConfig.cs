using AutoMapper;
using backend.Dto.Cart;
using backend.Dto.Order;
using backend.Dto.Product;
using backend.Dto.Review;
using backend.Dto.Size;
using backend.Dto.User;
using backend.Dtos.Category;
using backend.Dtos.Contact;
using backend.Dtos.InventoryLog;
using backend.Entities;
using backend.Entity;
using backend.Helper.EnumHelper;

namespace backend.Mapper;

public class MapperConfig : Profile
{
        public MapperConfig()
        {
                CreateMap<Size, SizeDto>();

                // Product
                CreateMap<ProductUpdateDto, Product>()
                        .ForMember(dest => dest.Sizes, opt => opt.Ignore())
                        .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
                CreateMap<ProductCreateDto, Product>()
                        .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
                        .ForMember(dest => dest.Sizes, opt => opt.Ignore());
                CreateMap<Product, ProductDto>()
                        .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(s => s.Category != null ? s.Category.Name : string.Empty))
                        .ForMember(dest => dest.Rating, opt => opt.MapFrom(s => s.Reviews.Count > 0 ? s.Reviews.Average(r => r.Rating) : 0))
                        .ForMember(dest => dest.RatingCount, opt => opt.MapFrom(s => s.Reviews.Count > 0 ? s.Reviews.Count : 0))
                        .ForMember(dest => dest.Sold, opt => opt.MapFrom(s => s.OrderItems.Count > 0 ? s.OrderItems.Count(x => x.Order.Status == Text.Enums.Enums.OrderStatus.Done) : 0))
                        .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.ProductImages.Select(pi => pi.ImageUrl)))
                        .ForMember(dest => dest.Sizes, opt => opt.MapFrom(src => src.Sizes.Select(s => new SizeDto
                        {
                                Name = s.Size != null ? s.Size.Name : string.Empty,
                                Inventory = s.Inventory
                        })));

                //Category
                CreateMap<Category, CategoryDto>();

                // User
                CreateMap<User, UserDto>();
                CreateMap<UserRegisterDto, User>()
                        .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
                CreateMap<UserUpdateDto, User>()
                        .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                        .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                        .ForMember(dest => dest.Role, opt => opt.Ignore());

                //Review
                CreateMap<ReviewCreateDto, Review>()
                        .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
                CreateMap<ReviewUpdateDto, Review>();
                CreateMap<Review, ReviewDto>()
                        .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
                        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.Name : string.Empty));

                //Cart
                CreateMap<CartItem, CartItemDto>()
                        .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
                        .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product != null ? src.Product.Price : 0))
                        .ForMember(dest => dest.ProductImg, opt => opt.MapFrom(src => src.Product != null ? src.Product.ProductImages.Select(a => a.ImageUrl).FirstOrDefault() : string.Empty))
                        .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.Size != null ? src.Size.Name : string.Empty));
                CreateMap<Cart, CartDto>();
                CreateMap<CartItemCreateDto, CartItem>();

                //Order
                CreateMap<OrderItem, OrderItemDto>()
                        .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
                        .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product != null ? src.Product.Price : 0))
                        .ForMember(dest => dest.ProductImg, opt => opt.MapFrom(src => src.Product != null ? src.Product.ProductImages.Select(a => a.ImageUrl).FirstOrDefault() : string.Empty))
                        .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.Size != null ? src.Size.Name : string.Empty));
                CreateMap<Order, OrderDto>()
                        .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
                // .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.Status.GetDescription()));
                CreateMap<OrderCreateDto, Order>();
                CreateMap<OrderUpdateDto, Order>()
                        .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
                CreateMap<CartItem, OrderItem>()
                        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                        .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                        .ForMember(dest => dest.SizeId, opt => opt.MapFrom(src => src.SizeId))
                        .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

                //Discount
                CreateMap<DiscountCreateDto, Discount>();

                //Image - Product

                CreateMap<ProductImage, ProductDto>()
                        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId))
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
                        .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Product != null ? src.Product.Description : string.Empty))
                        .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product != null ? src.Product.Price : 0))
                        .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Product != null ? src.Product.CreatedAt : DateTime.UtcNow))
                        .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.Product != null ? src.Product.UpdatedAt : DateTime.UtcNow))
                        .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Product != null ? src.Product.CategoryId : Guid.Empty))
                        .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Category.Name : string.Empty))
                        .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.Product != null ? src.Product.ProductImages.Select(pi => pi.ImageUrl) : new List<string>()))
                        .ForMember(dest => dest.Sizes, opt => opt.MapFrom(src => src.Product != null ? src.Product.Sizes.Select(s => new SizeDto
                        {
                                Name = s.Size != null ? s.Size.Name : string.Empty,
                                Inventory = s.Inventory
                        }) : new List<SizeDto>()));

                // Contact

                CreateMap<Contact, ContactDto>()
                        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.Name : string.Empty));
                CreateMap<ContactCreateDto, Contact>()
                        .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                        .ForMember(dest => dest.UserId, opt => opt.Ignore());
                CreateMap<ContactUpdateDto, Contact>()
                        .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));

                //InventoryLog
                CreateMap<InventoryLog, InventoryLogDto>()
                        .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductInventory != null ? src.ProductInventory.Product.Name : string.Empty))
                        .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.ProductInventory != null ? src.ProductInventory.Size.Name : string.Empty))
                        .ForMember(dest => dest.ByName, opt => opt.MapFrom(src => src.User != null ? src.User.Name : src.ByName))
                        .ForMember(dest => dest.Message, opt => opt.Ignore());
        }

}
