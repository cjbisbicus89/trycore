namespace EVM.ProjectManagement.Infrastructure.Persistence.Configurations;

using EVM.ProjectManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.ProjectId)
            .IsRequired();

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.BudgetedCost)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(a => a.PlannedPercentage)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.Property(a => a.ActualPercentage)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.Property(a => a.ActualCost)
            .IsRequired()
            .HasPrecision(18, 2);

        // Índice para consultas por ProjectId (dashboard carga actividades por proyecto)
        builder.HasIndex(a => a.ProjectId)
            .HasDatabaseName("IX_Activities_ProjectId");

        // Ignorar propiedades calculadas
        builder.Ignore(a => a.PlannedValue);
        builder.Ignore(a => a.EarnedValue);
    }
}
