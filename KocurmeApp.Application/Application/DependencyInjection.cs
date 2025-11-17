using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using KocurmeApp.Application.Application.Services;
using KocurmeApp.Application.Application;

namespace KocurmeApp.Application.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<Interfaces.IExcelExportService, ExcelExportService>();


            return services;
        }
    }
}
