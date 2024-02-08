using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieBooking.Domain.Common;
using MovieBooking.Domain.Common.Constracts;
using MovieBooking.Domain.Entities;
using MovieBooking.Identity.Interceptors;
using MovieBooking.Identity.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Identity.Database
{
    public class AppIdentityDbContext : IdentityDbContext<User, ApplicationRole, string, IdentityUserClaim<string>, IdentityUserRole<string>, ApplicationUserLogin, ApplicationRoleClaim, IdentityUserToken<string>>
    {
        private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options,
                AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options)
        {
            _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        }
        public DbSet<Trial> AuditTrails => Set<Trial>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // QueryFilters need to be applied before base.OnModelCreating
            builder.AppendGlobalQueryFilter<ISoftDelete>(s => s.IsDeleted == false);

            builder.HasDefaultSchema(SchemaNames.Identity);
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
