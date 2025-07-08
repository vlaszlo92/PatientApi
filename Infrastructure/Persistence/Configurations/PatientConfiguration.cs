using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.Property(p => p.Id)
            .HasColumnType("uniqueidentifier");

        builder.Property(p => p.HealthInsuranceNumber)
            .IsRequired()
            .HasMaxLength(11)
            .HasColumnType("nvarchar(11)");

        builder.HasIndex(p => p.HealthInsuranceNumber)
            .IsUnique();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Complaints)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(p => p.Diagnosis)
            .HasMaxLength(1000);

        builder.Property(p => p.CreatedAt)
            .IsRequired();
    }
}
