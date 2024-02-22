using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MovieBooking.Application.Contracts.Identity;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Models.Authentication;
using MovieBooking.Identity.Authorizations.Permissions;
using MovieBooking.Identity.Database;
using MovieBooking.Identity.Interceptors;
using MovieBooking.Identity.Models;
using MovieBooking.Identity.Services;

namespace MovieBooking.Identity;

public static class IdentityServiceExtensions
{
    public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        services.AddDbContext<AppIdentityDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetRequiredService<AuditableEntitySaveChangesInterceptor>());

            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AppIdentityDbContext).Assembly.FullName));
        });
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        services.AddScoped<AppIdentityDbContextInitialiser>();

        services.AddIdentity<User, ApplicationRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();

        services
          .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
          .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddAuthentication(options =>
         {
             options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
             options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
         })
                 .AddJwtBearer(o =>
                 {
                     o.RequireHttpsMetadata = false;
                     o.SaveToken = false;
#pragma warning disable CS8604 // Possible null reference argument.
                     o.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuerSigningKey = true,
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ClockSkew = TimeSpan.Zero,
                         ValidIssuer = configuration["JwtSettings:Issuer"],
                         ValidAudience = configuration["JwtSettings:Audience"],
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]))
                     };
#pragma warning restore CS8604 // Possible null reference argument.

                     o.Events = new JwtBearerEvents()
                     {
                         OnChallenge = context =>
                         {
                             context.HandleResponse();
                             if (!context.Response.HasStarted)
                             {
                                 throw new UnauthorizedException("Authentication Failed.");
                             }

                             return Task.CompletedTask;
                         },
                         OnForbidden = _ => throw new UnauthorizedException("You are not authorized to access this resource."),
                         OnMessageReceived = context =>
                         {
                             var accessToken = context.Request.Query["access_token"];

                             if (!string.IsNullOrEmpty(accessToken) &&
                                     (context.HttpContext.Request.Path.StartsWithSegments("/notifications")
                                     )
                                 )
                             {
                                 // Read the token out of the query string
                                 context.Token = accessToken;
                             }

                             return Task.CompletedTask;
                         }
                         //OnAuthenticationFailed = c =>
                         //{
                         //    c.NoResult();
                         //    c.Response.StatusCode = 500;
                         //    c.Response.ContentType = "text/plain";
                         //    return c.Response.WriteAsync(c.Exception.ToString());
                         //},
                         //OnChallenge = context =>
                         //{
                         //    context.HandleResponse();
                         //    context.Response.StatusCode = 401;
                         //    context.Response.ContentType = "application/json";
                         //    var result = JsonSerializer.Serialize("401 Not authorized");
                         //    return context.Response.WriteAsync(result);
                         //},
                         //OnForbidden = context =>
                         //{
                         //    context.Response.StatusCode = 403;
                         //    context.Response.ContentType = "application/json";
                         //    var result = JsonSerializer.Serialize("403 Not authorized");
                         //    return context.Response.WriteAsync(result);
                         //}
                     };
                 });
    }

}
