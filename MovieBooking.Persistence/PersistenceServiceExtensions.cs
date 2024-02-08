using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieBooking.Application.UnitOfWork;
using MovieBooking.Persistence.Database;
using MovieBooking.Persistence.Interceptors;
using MovieBooking.Persistence.UnitofWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Persistence
{
    public static class PersistenceServiceExtensions
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddTransient<AuditableEntitySaveChangesInterceptor>();
            services.AddScoped<DispatchDomainEventsInterceptor>();
            services.AddDbContext<MovieDBContext>((sp, options) =>
            {
                options.AddInterceptors(
                        sp.GetRequiredService<AuditableEntitySaveChangesInterceptor>(),
                        sp.GetRequiredService<DispatchDomainEventsInterceptor>()
                    );

                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            });
           
            services.AddScoped<MovieDbContextInitialiser>();

            services.AddScoped<ICommandUnitOfWork, CommandUnitOfWork>();
            services.AddScoped<IQueryUnitOfWork, QueryUnitofWork>();
            return services;
        }
    }
}
