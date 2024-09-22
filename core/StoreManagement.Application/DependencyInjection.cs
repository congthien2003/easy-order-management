using Microsoft.Extensions.DependencyInjection;
using StoreManagement.Application.Interfaces.IServices;
using StoreManagement.Application.Services;
using StoreManagement.Services;
namespace StoreManagement.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // add scoped services
            services.AddSignalR();
            services.AddTransient<IJwtManager, JwtManager>();
            services.AddTransient<IJwtManager, JwtManager>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IStoreService, StoreService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IFoodService, FoodService>();
            services.AddTransient<ITableService, TableService>();
            services.AddTransient<IPaymentTypeService, PaymentTypeService>();
            services.AddTransient<IOrderSerivce, OrderService>();
            services.AddTransient<IOrderDetailService, OrderDetailService>();
            services.AddTransient<IInvoiceService, InvoiceService>();
            services.AddTransient<IProductSellService, ProductSellService>();
            return services; 
        }
    }
}
