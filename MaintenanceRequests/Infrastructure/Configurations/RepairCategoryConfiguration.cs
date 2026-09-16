using MaintenanceRequests.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaintenanceRequests.Infrastructure.Configurations;

public class RepairCategoryConfiguration : IEntityTypeConfiguration<RepairCategory>
{
    public void Configure(EntityTypeBuilder<RepairCategory> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(500);
    }
}