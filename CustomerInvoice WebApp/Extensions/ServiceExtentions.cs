using CustomerInvoice_WebApp.Repositories.Interfaces;
using CustomerInvoice_WebApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using CustomerInvoice_WebApp.Services;
using CustomerInvoice_WebApp.Data;
using CustomerInvoice_WebApp.Helpers;
using CustomerInvoice_WebApp.Helpers.interfaces;
using Microsoft.Extensions.Caching.Memory;
using CustomerInvoice_WebApp.Validators;
using CustomerInvoice_WebApp.Models;
using System.Reflection;

namespace CustomerInvoice_WebApp.Extensions
{
    internal static class ServiceExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PaginationSettings>(configuration.GetSection("Pagination"));

            services.AddScoped<IInvoicesService, InvoiceService>();
            services.AddScoped<IInvoiceCacheManager, InvoicesCacheManager>();
            services.AddScoped<IUnitOfWork, InvoicesUnitOfWork>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();

            services.AddDbContext<CustomerInvoiceDbContext>(options =>
                        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(Program).Assembly.FullName)));

            services.AddAutoMapper(typeof(Program));

            services.AddControllers(options =>
            {
                options.Filters.Add(new ModelValidationActionFilter());
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "CustomerInvoice Web App API",
                    Description = "CustomerInvoice Web App API (ASP.NET Core 7.0)",
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            // Add memory cache if not already added
            if (!services.Any(x => x.ServiceType == typeof(IMemoryCache)))
            {
                services.AddMemoryCache();
            }

            services.AddEndpointsApiExplorer();
        }
    }
}
