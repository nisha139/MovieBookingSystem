using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Booking.Query;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Movie.Query;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Screen.Query;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Seat.Query;
using MovieBooking.Application.Contracts.Persistence.Repositoris.ShowTime.Query;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Theater.Query;
using MovieBooking.Application.Services;
using MovieBooking.Application.UnitOfWork;
using MovieBooking.Persistence.Database;
using MovieBooking.Persistence.Interceptors;
using MovieBooking.Persistence.Repositories.Booking.Query;
using MovieBooking.Persistence.Repositories.Movie.Query;
using MovieBooking.Persistence.Repositories.Screen.Query;
using MovieBooking.Persistence.Repositories.Seat.Query;
using MovieBooking.Persistence.Repositories.Services;
using MovieBooking.Persistence.Repositories.ShowTIme.Query;
using MovieBooking.Persistence.Repositories.Theater.Query;
using MovieBooking.Persistence.UnitofWork;
using MovieBooking.Persistence.UnitOfWork;
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
            services.AddScoped<IMovieQueryRepository, MovieQueryRepository>();
            services.AddScoped<ISeatQueryRepository, SeatQueryRepository>();
            services.AddScoped<IBookingDataService, BookingDataService>();
            services.AddScoped<ITheaterQueryRepository, TheaterQueryRepository>();
            services.AddScoped<IShowTimeQueryRepostory, ShowTimeRepository>();
            services.AddScoped<ISeatQueryRepository, SeatQueryRepository>();
            services.AddScoped<IScreenQueryRepository, ScreenQueryRepository>();
            services.AddScoped<IBookingQueryRepository, BookingQueryRepository>();
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
            services.AddScoped<IQueryUnitOfWork, QueryUnitOfWork>();
            return services;
        }
    }
}
