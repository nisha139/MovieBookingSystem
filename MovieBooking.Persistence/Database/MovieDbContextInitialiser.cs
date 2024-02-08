using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Persistence.Database
{
    public static class MovieDBInitialiserExtensions
    {
        public static async Task InitialiseAppDatabaseAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var initialiser = scope.ServiceProvider.GetRequiredService<MovieDbContextInitialiser>();

            await initialiser.InitialiseAsync();

            await initialiser.SeedAsync();
        }
    }
    public class MovieDbContextInitialiser(ILogger<MovieDBContext> logger,MovieDBContext context)
    {
        private readonly ILogger<MovieDBContext> _logger = logger;
        private readonly MovieDBContext _context = context;
        public async Task InitialiseAsync()
        {
            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {

                try
                {
                    await _context.Database.MigrateAsync();

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while initialising the database.");
                    throw;
                }
            }
            var appliedMigrations = await _context.Database.GetAppliedMigrationsAsync();
            var lastAppliedMigration = appliedMigrations.LastOrDefault();

            if (lastAppliedMigration != null)
            {
                Console.WriteLine($"You're on schema version: {lastAppliedMigration}");
            }
            else
            {
                Console.WriteLine("No migrations have been applied yet.");
            }


            Console.WriteLine($"You're on schema version: {lastAppliedMigration}");
        }
        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }
        public async Task TrySeedAsync()
        {
            // Default data
            // Seed, if necessary
            if (!_context.Movies.Any())
            {
                _context.Movies.Add(new Domain.Entities.Movie { Title = "Make a movie list 📃",  });

                await _context.SaveChangesAsync();
            }

        }
    }
}
