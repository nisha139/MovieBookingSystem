using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MovieBooking.Application.Behaviours;
using MovieBooking.Application.Features.Movie.Queries;
using MovieBooking.Application.Features.Movie.Dto;


namespace MovieBooking.Application
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Register LoggingBehaviour as a pipeline behavior
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));


            // Configure MediatR with behaviors
            services.AddMediatR(cfg =>
            {

                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

                // Add other behaviors as needed (UnhandledExceptionBehaviour, AuthorizationBehaviour, ValidationBehaviour, etc.)
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            });

            return services;
        }
    }
}
