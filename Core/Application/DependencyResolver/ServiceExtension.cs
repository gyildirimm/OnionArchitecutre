using Application.Core.CrossCuttingConcerns;
using Application.Core.CrossCuttingConcerns.Redis;
using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Application.DependencyResolver
{
    public static class ServiceExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IOrderService, OrderService>();

            services.AddStackExchangeRedisCache(o => 
            {
                o.Configuration = "localhost:6379";
            });

            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6379"));

            services.AddScoped<ICacheService, RedisCacheService>();
        }
    }
}
