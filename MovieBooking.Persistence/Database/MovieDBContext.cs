using Microsoft.EntityFrameworkCore;
using MovieBooking.Domain.Common.Constracts;
using MovieBooking.Domain.Entities;
using MovieBooking.Persistence.Interceptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Persistence.Database
{
    public class MovieDBContext  : DbContext
    {
        private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
       
        public MovieDBContext(
          DbContextOptions<MovieDBContext> options,
          AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
          : base(options)
        {
            _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        }

        public DbSet<Movie> Movies { get; set; }
        //public DbSet<Booking> Bookings { get; set; }
        public DbSet<Screen> Screens { get; set; }
        public DbSet<Seat> seats { get; set; }
        public DbSet<Showtime> showtimes { get; set; }
        public DbSet<Theater> Theater { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // QueryFilters need to be applied before base.OnModelCreating
            // modelBuilder.AppendGlobalQueryFilter<ISoftDelete>(s => s.IsDeleted == false);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //modelBuilder.Entity<Booking>()
            //     .HasOne(b => b.Showtime)
            //     .WithMany(s => s.Bookings)
            //     .HasForeignKey(b => b.ShowtimeID)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Booking>()
            //    .HasOne(b => b.Movie)
            //    .WithMany()
            //    .HasForeignKey(b => b.Id)
            //    .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }


    }
}
