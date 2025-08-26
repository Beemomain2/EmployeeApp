using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.LastName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Email)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(e => e.Email)
            .IsUnique();

        builder.Property(e => e.PhoneNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.Role)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(e => e.HireDate)
            .IsRequired();

        builder.Property(e => e.Salary)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .IsRequired();
    }
}
