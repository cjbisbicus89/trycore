namespace EVM.ProjectManagement.Infrastructure.Persistence;

using EVM.ProjectManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Project> Projects => this.Set<Project>();

    public DbSet<Activity> Activities => this.Set<Activity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
}
