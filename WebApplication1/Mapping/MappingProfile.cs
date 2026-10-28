using AutoMapper;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Category,  CategoryDTO>();

            CreateMap<CreateCategoryDTO, Category>();
            CreateMap<UpdateCategoryDTO, Category>();

            // --------

            CreateMap<Product, ProductDTO>();

            CreateMap<CreateProductDTO, Product>()
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<UpdateProductDTO, Product>()
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            // --------

            CreateMap<Role, RoleDTO>();

            CreateMap<CreateRoleDTO, Role>();
            CreateMap<UpdateRoleDTO, Role>();

            // --------

            CreateMap<User, UserDTO>();

            CreateMap<CreateUserDTO, User>()
                .ForMember(dest => dest.Role, opt => opt.Ignore());

            CreateMap<UpdateUserDTO, User>()
                .ForMember(dest => dest.Role, opt => opt.Ignore());

            // --------

            CreateMap<Order, OrderDTO>();

            CreateMap<CreateOrderDTO, Order>()
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<UpdateOrderDTO, Order>()
                .ForMember(dest => dest.User, opt => opt.Ignore());

            // --------

            CreateMap<OrderItem, OrderItemDTO>();

            CreateMap<CreateOrderItemDTO, OrderItem>()
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore());

            CreateMap<UpdateOrderItemDTO, OrderItem>()
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore());

            // --------

            CreateMap<Cart, CartDTO>();

            CreateMap<CreateCartDTO, Cart>()
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<UpdateCartDTO, Cart>()
                .ForMember(dest => dest.User, opt => opt.Ignore());

            // --------

            CreateMap<CartItem, CartItemDTO>();

            CreateMap<CreateCartItemDTO, CartItem>()
                .ForMember(dest => dest.Cart, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore());

            CreateMap<UpdateCartItemDTO, CartItem>()
                .ForMember(dest => dest.Cart, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore());
        }
    }
}
