using TesteTecnicoDigitas.Domain.Repository;
using TesteTecnicoDigitas.Infrastructure.Repository;
using TesteTecnicoDigitas.Infrastructure.DataServices;
using TesteTecnicoDigitas.Domain.DataServices;
using TesteTecnicoDigitas.Domain.Handlers;
using System.Reflection;
using TesteTecnicoDigitas.Domain.Services;
using TesteTecnicoDigitas.Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace TesteTecnicoDigitas.Api
{
    public static class Configurations
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddInfrastructureServices();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(BestPriceHandler).Assembly));
            services.AddScoped<StartupBtc>().BuildServiceProvider();
            services.AddScoped<StartupEth>().BuildServiceProvider();

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IBestPriceRepository, BestPriceRepository>();
            services.AddScoped<IOrderBookRepository, OrderBookRepository>();
            services.AddScoped<IWebSocketDataService, WebSocketDataService>();
            services.AddScoped<IWebSocketService, WebSocketService>();

            return services;
        }
    }
}
