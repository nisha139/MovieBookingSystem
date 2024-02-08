using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Identity.Configurations;
public sealed class AuditTrailConfig : IEntityTypeConfiguration<Trial>
{
    public void Configure(EntityTypeBuilder<Trial> builder) =>
        builder
            .ToTable("AuditTrails", "Auditing");
}
