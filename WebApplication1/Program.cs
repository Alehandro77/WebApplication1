using Microsoft.EntityFrameworkCore;
using WebApplication1.Mapping;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.Services;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ShopContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("con")));

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            
            builder.Services.AddScoped<IRoleRepository, RoleRepository>(); 
            builder.Services.AddScoped<IRoleService, RoleService>(); 
            
            builder.Services.AddScoped<IUserRepository, UserRepository>(); 
            builder.Services.AddScoped<IUserService, UserService>(); 
            
            builder.Services.AddScoped<IOrderRepository, OrderRepository>(); 
            builder.Services.AddScoped<IOrderService, OrderService>(); 
            
            builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>(); 
            builder.Services.AddScoped<IOrderItemService, OrderItemService>(); 
            
            builder.Services.AddScoped<ICartRepository, CartRepository>(); 
            builder.Services.AddScoped<ICartService, CartService>(); 
            
            builder.Services.AddScoped<ICartItemRepository, CartItemRepository>(); 
            builder.Services.AddScoped<ICartItemService, CartItemService>();

            builder.Services.AddScoped<ICheckoutService, CheckoutService>();

            builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
