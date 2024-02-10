using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MovieBooking.InfraStructure.Mailing
{
    internal static class Startup
    {
        internal static IServiceCollection AddMailing(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<MailSettings>(options =>
            {
                options.From = config.GetSection(nameof(MailSettings))["From"];
                options.Host = config.GetSection(nameof(MailSettings))["Host"];
                options.Port = int.Parse(config.GetSection(nameof(MailSettings))["Port"]);
                options.UserName = config.GetSection(nameof(MailSettings))["UserName"];
                options.Password = config.GetSection(nameof(MailSettings))["Password"];
                options.DisplayName = config.GetSection(nameof(MailSettings))["DisplayName"];
            });

            return services;
        }
    }
}
