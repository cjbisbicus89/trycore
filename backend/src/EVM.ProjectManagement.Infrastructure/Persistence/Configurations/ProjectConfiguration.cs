namespace EVM.ProjectManagement.Infrastructure.Persistence.Configurations;

using EVM.ProjectManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        // Índice para ordenamiento por fecha de creación
        builder.HasIndex(p => p.CreatedAt)
            .IsDescending()
            .HasDatabaseName("IX_Projects_CreatedAt");

        // Relación 1:N con Activity
        builder.HasMany(p => p.Activities)
            .WithOne()
            .HasForeignKey(a => a.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
