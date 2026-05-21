namespace EVM.ProjectManagement.Infrastructure.Persistence.Repositories;

using EVM.ProjectManagement.Domain.Entities;
using EVM.ProjectManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

public sealed class ActivityRepository : IActivityRepository
{
    private readonly AppDbContext context;

    public ActivityRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<Activity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await this.context.Activities.FindAsync([id], cancellationToken);

    public async Task<IReadOnlyList<Activity>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default)
        => await this.context.Activities.Where(a => a.ProjectId == projectId).ToListAsync(cancellationToken).ContinueWith(t => t.Result.AsReadOnly(), cancellationToken);

    public async Task AddAsync(Activity activity, CancellationToken cancellationToken = default)
    {
        await this.context.Activities.AddAsync(activity, cancellationToken);
        await this.context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Activity activity, CancellationToken cancellationToken = default)
    {
        this.context.Activities.Update(activity);
        await this.context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Activity activity, CancellationToken cancellationToken = default)
    {
        this.context.Activities.Remove(activity);
        await this.context.SaveChangesAsync(cancellationToken);
    }
}
