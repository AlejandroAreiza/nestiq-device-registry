namespace NestIQ.DeviceRegistry.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NestIQ.DeviceRegistry.Domain.Entities;

public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Type)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(d => d.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(d => d.HomeId)
            .IsRequired();

        builder.Property(d => d.CreatedAt)
            .IsRequired();

        builder.HasIndex(d => new { d.HomeId, d.Name })
            .IsUnique();
    }
}