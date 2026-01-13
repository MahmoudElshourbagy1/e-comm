
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecomm.infrastructure.Data;
using Ecomm.infrastructure.Repositries;
using Ecomm.infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;

namespace Ecomm.infrastructure
{
    public static class infrastructureRegisteration
    {
        public static IServiceCollection infrastructureConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            //// Add DbContext
            //services.AddDbContext<AppDbContext>(options =>
            //    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            // Add Generic Repository
            services.AddScoped(typeof(IGenericRepositry<>), typeof(GenericRepositry<>));
            //services.AddScoped<ICategoryRepositry, CategoryRepositry>();
            //services.AddScoped<IProductRepositry, ProductRepositry>();
            //services.AddScoped<IPhotoRepositry, PhotoRepositry>();
            //apply Redis Cache
            services.AddSingleton<IConnectionMultiplexer>(
                i =>
                { var config = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis"), true);
                    return ConnectionMultiplexer.Connect(config);});
            services.AddScoped<IProductRepositry, ProductRepositry>();
            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),"wwwroot")));
            services.AddScoped<IImageManagementService, ImageManagementService>();
            services.AddScoped<ICustomerBasketRepository, CustomerBasketRepository>();
            //apply unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // applay DbContext
            services.AddDbContext<AppDbContext>(op =>
            {
                op.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
    }
}
