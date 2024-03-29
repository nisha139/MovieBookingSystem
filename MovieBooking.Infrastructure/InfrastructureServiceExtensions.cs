﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MovieBooking.Application.Contracts.Application;
using MovieBooking.Application.Interfaces;
using MovieBooking.Infrastructure.DistributedLock;
using MovieBooking.InfraStructure.Caching;
using MovieBooking.InfraStructure.Cors;
using MovieBooking.InfraStructure.Mailing;
using MovieBooking.InfraStructure.Services;
using StackExchange.Redis;

namespace MovieBooking.InfraStructure;

public static class InfrastructureServiceExtensions
{
    public static void AddInfrastructureSharedServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
       .AddServices()
       .AddMailing(configuration)
       //.AddBackgroundJobs(configuration)
        .AddCaching(configuration)
       .AddScoped<IDateTimeService, DateTimeService>();
        services.AddSingleton<IDistributedLockProvider>(provider =>
        {
            var connectionString = configuration.GetValue<string>("CacheSettings:RedisURL");
            var options = ConfigurationOptions.Parse(connectionString);
            options.AbortOnConnectFail = false;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("RedisURL is missing in appsettings.json.");
            }
            return new RedisDistributedLockProvider(connectionString);
        });
    }

    internal static IServiceCollection AddServices(this IServiceCollection services) =>
    services
        .AddServices(typeof(ITransientService), ServiceLifetime.Transient)
        .AddServices(typeof(IScopedService), ServiceLifetime.Scoped);


    internal static IServiceCollection AddServices(this IServiceCollection services, Type interfaceType, ServiceLifetime lifetime)
    {
        var interfaceTypes =
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => interfaceType.IsAssignableFrom(t)
                            && t.IsClass && !t.IsAbstract)
                .Select(t => new
                {
                    Service = t.GetInterfaces().FirstOrDefault(),
                    Implementation = t
                })
                .Where(t => t.Service is not null
                            && interfaceType.IsAssignableFrom(t.Service));

        foreach (var type in interfaceTypes)
        {
            services.AddService(type.Service!, type.Implementation, lifetime);
        }

        return services;
    }

    internal static IServiceCollection AddService(this IServiceCollection services, Type serviceType, Type implementationType, ServiceLifetime lifetime) =>
    lifetime switch
    {
        ServiceLifetime.Transient => services.AddTransient(serviceType, implementationType),
        ServiceLifetime.Scoped => services.AddScoped(serviceType, implementationType),
        ServiceLifetime.Singleton => services.AddSingleton(serviceType, implementationType),
        _ => throw new ArgumentException("Invalid lifeTime", nameof(lifetime))
    };

    //public static void AddCorsService(this IServiceCollection services, IConfiguration configuration)
    //{
    //    services
    //        .AddCorsPolicy(configuration);
    //}

    //public static IApplicationBuilder UseCorsService(this IApplicationBuilder builder)
    //{
    //    return builder
    //        .UseCorsPolicy();
    //}

    //public static IApplicationBuilder UseHangfire(this IApplicationBuilder builder, IConfiguration config)
    //{
    //    return builder
    //        .UseHangfire Dashboard(config);
    //}

    //public static IEndpointRouteBuilder UseInfraEndpoints(this IEndpointRouteBuilder builder)
    //{
    //    builder.MapNotifications();
    //    return builder;
    //}

    //internal static IEndpointRouteBuilder MapNotifications(this IEndpointRouteBuilder endpoints)
    //{
    //    endpoints.MapHub<NotificationHub>("/notifications", options =>
    //    {
    //        options.CloseOnAuthenticationExpiration = true;
    //    });

    //    return endpoints;
    //}
}
