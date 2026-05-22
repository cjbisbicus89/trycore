namespace EVM.ProjectManagement.Infrastructure.Persistence.Configurations;

using EVM.ProjectManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.HasKey(a => a.Id);

        builder.Property(a => a.ProjectId)
            .IsRequired();

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(ActivityConstants.MaxNameLength);

        builder.Property(a => a.BudgetedCost)
            .IsRequired()
            .HasPrecision(ActivityConstants.BudgetedCostPrecision, ActivityConstants.BudgetedCostScale);

        builder.Property(a => a.PlannedPercentage)
            .IsRequired()
            .HasPrecision(ActivityConstants.PercentagePrecision, ActivityConstants.PercentageScale);

        builder.Property(a => a.ActualPercentage)
            .IsRequired()
            .HasPrecision(ActivityConstants.PercentagePrecision, ActivityConstants.PercentageScale);

        builder.Property(a => a.ActualCost)
            .IsRequired()
            .HasPrecision(ActivityConstants.BudgetedCostPrecision, ActivityConstants.BudgetedCostScale);

        builder.Property(a => a.RowVersion)
            .IsRowVersion();

        builder.HasIndex(a => a.ProjectId)
            .HasDatabaseName("IX_Activities_ProjectId");

        builder.Ignore(a => a.PlannedValue);
        builder.Ignore(a => a.EarnedValue);
    }
}
