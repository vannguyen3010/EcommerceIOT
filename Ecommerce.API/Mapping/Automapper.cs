using AutoMapper;
using Entities.Models;
using Shared.DTO.Cart;
using Shared.DTO.CategoryIntroduce;
using Shared.DTO.Contact;
using Shared.DTO.Introduce;
using Shared.DTO.Order;
using Shared.DTO.Product;
using Shared.DTO.Profile;
using Shared.DTO.User;

namespace Ecommerce.API.Mapping
{
    public class Automapper : Profile
    {
        public Automapper()
        {
            CreateMap<RegisterDto, User>();

            CreateMap<User, UserDto>();

            CreateMap<CreateContactDto, Contact>();

            CreateMap<Contact, ContactDto>();

            CreateMap<CreateProfileDto, ProfileInfo>();

            CreateMap<ProfileInfo, ProfileInfoDto>();

            CreateMap<UpdateProfile, ProfileInfo>();

            CreateMap<CreateCategoryIntroDto, CategoryNew>();

            CreateMap<CategoryNew, CategoryIntroduceDto>();

            CreateMap<UpdateCateIntroDto, CategoryNew>();

            CreateMap<CreateIntroduceDto, New>();

            CreateMap<New, IntroduceDto>();

            CreateMap<UpdateIntroduceDto, New>();

            CreateMap<CreateCateProductDto, CateProduct>();

            CreateMap<CateProduct, CateProductDto>();

            CreateMap<UpdateCateProductDto, CateProduct>();

            CreateMap<CreateProductDto, Product>();

            CreateMap<Product, ProductDto>();

            CreateMap<UpdateProductDto, Product>();

            CreateMap<AddToCartDto, CartItem>();

            CreateMap<CartItem, CartItemDto>()
           .ForMember(dest => dest.FinalPrice, opt => opt.MapFrom(src => (src.Price * src.Quantity) - (src.Discount * src.Quantity)));

            CreateMap<UpdateCartItemDto, CartItem>();

            CreateMap<CartItemDto, CartItem>();

            CreateMap<CreateOrderDto, Order>();

            CreateMap<OrderItemDto, OrderItem>();

            CreateMap<Order, OrderDto>();

            CreateMap<OrderDto, Order>();

            CreateMap<OrderItem, OrderItemDto>();

            CreateMap<Order, CartItemDto>();

            CreateMap<Order, CartItemDto>();

            CreateMap<CartItemDto, Order>();
        }
    }
}
