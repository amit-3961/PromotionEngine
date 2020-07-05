using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PromotionEngine.Business;
using PromotionEngine.Data;
using PromotionEngine.Data.Models;
using PromotionEngine.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PromotionEngine.Injection
{
    public static class Injection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddBusinessDependencies()
                .AddDataDependencies(configuration);               
        }

        private static IServiceCollection AddBusinessDependencies(this IServiceCollection services)
        {
            //Add required custom business injections along with common
            services.AddTransient<IProductEngineBusiness, ProductEngineBusiness>();
            
            return services;
        }

        private static IServiceCollection AddDataDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ekartContext>(options => options.UseSqlServer(configuration["ConnectionStrings:Sql"], x => x.EnableRetryOnFailure()));

            //Add required custom data injections along with common
            services.AddTransient<IProductEngineData, ProductEngineData>();
            services.AddTransient<IRepository, Repository>();
            


            return services;
        }
    }
}
