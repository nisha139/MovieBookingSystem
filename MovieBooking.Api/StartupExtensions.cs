using Microsoft.OpenApi.Models;
using MovieBooking.API.Services;
using MovieBooking.Identity;
using MovieBooking.Persistence;
using MovieBooking.Application;
using MovieBooking.Application.Contracts.Application;
using MovieBooking.InfraStructure;
using MovieBooking.Identity.Database;
using Microsoft.EntityFrameworkCore;
using MovieBooking.Application.Contracts.Identity;
using MovieBooking.Identity.Services;
using MovieBooking.Application.Contracts.Caching;
namespace MovieBooking.Api
{
    public  static class StartupExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            AddSwagger(builder.Services);
            builder.Services.AddApplicationServices();
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddIdentityServices(builder.Configuration);
            builder.Services.AddInfrastructureSharedServices(builder.Configuration);
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
;           builder.Services.AddHttpContextAccessor();

            builder.Services.AddControllers();
            //builder.Services.AddAuthorization(options =>
            //{
            //     Existing authorization policies...

            //    options.AddPolicy("ChangePasswordPolicy", policy =>
            //    {
            //        policy.RequireClaim("uid"); // Assuming you store user ID in claims
            //    });
            //});

            builder.Services.AddSwaggerGen();

            // builder.Services.AddCorsService(builder.Configuration);

            return builder.Build();

        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {

            if (app.Environment.IsDevelopment() )
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MovieBooking API");
                });
            }


            // app.UseHttpsRedirection();

            app.UseRouting();

           // app.UseInfraEndpoints();

            app.UseAuthentication();

            //app.UseCustomExceptionHandler();

            app.UseAuthorization();

            app.MapControllers();

           // app.UseCorsService();

            return app;

        }

        private static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Movie API",

                });

                //c.OperationFilter<FileResultContentTypeOperationFilter>();
            });
        }


        public static async Task ResetDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            try
            {
                var context = scope.ServiceProvider.GetService<AppIdentityDbContext>();
                if (context != null)
                {
                    await context.Database.EnsureDeletedAsync();
                    await context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
                logger.LogError(ex, "An error occurred while migrating the database.");
            }
        }
    }
}
