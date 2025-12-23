
using Ecom.Core.Interfaces;
using Ecomm.infrastructure.Data;
using Ecomm.infrastructure.Repositries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

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
