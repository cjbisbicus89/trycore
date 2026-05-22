namespace EVM.ProjectManagement.Infrastructure.Persistence.Configurations;

using EVM.ProjectManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(ProjectConstants.MaxNameLength);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(ProjectConstants.MaxDescriptionLength);

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(p => p.RowVersion)
            .IsRowVersion();

        builder.HasIndex(p => p.CreatedAt)
            .IsDescending()
            .HasDatabaseName("IX_Projects_CreatedAt");

        builder.HasMany(p => p.Activities)
            .WithOne()
            .HasForeignKey(a => a.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
