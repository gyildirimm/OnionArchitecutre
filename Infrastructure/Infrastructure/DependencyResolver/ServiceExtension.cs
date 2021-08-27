using Domain.Repositories;
using Infrastructure.Context;
using Infrastructure.Repositories.Ef;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DependencyResolver
{
    public static class ServiceExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IOrderRepository, OrderRepository>();
        }

        public static void AddContext(this IServiceCollection services, string orderConnectionString)
        {
            //Configuration.GetConnectionString("DefaultConnection")
            services.AddDbContext<OrderDbContext>(opt =>
            {
                opt.UseSqlServer(orderConnectionString, configure =>
                {
                    configure.MigrationsAssembly("Infrastructure");
                });
            });
        }
    }
}
