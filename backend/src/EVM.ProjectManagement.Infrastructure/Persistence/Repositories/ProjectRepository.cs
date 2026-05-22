namespace EVM.ProjectManagement.Infrastructure.Persistence.Repositories;

using EVM.ProjectManagement.Domain.Entities;
using EVM.ProjectManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

public sealed class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext context;

    public ProjectRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await this.context.Projects.FindAsync([id], cancellationToken);

    public async Task<Project?> GetWithActivitiesAsync(Guid id, CancellationToken cancellationToken = default)
        => await this.context.Projects.Include(p => p.Activities).FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Project>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var projects = await this.context.Projects.ToListAsync(cancellationToken);
        return projects.AsReadOnly();
    }

    public async Task AddAsync(Project project, CancellationToken cancellationToken = default)
    {
        await this.context.Projects.AddAsync(project, cancellationToken);
        await this.context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Project project, CancellationToken cancellationToken = default)
    {
        this.context.Projects.Update(project);
        await this.context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Project project, CancellationToken cancellationToken = default)
    {
        this.context.Projects.Remove(project);
        await this.context.SaveChangesAsync(cancellationToken);
    }
}
