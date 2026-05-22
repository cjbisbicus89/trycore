namespace EVM.ProjectManagement.Infrastructure.Persistence.Repositories;

using EVM.ProjectManagement.Domain.Entities;
using EVM.ProjectManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

public sealed class ActivityRepository : IActivityRepository
{
    private readonly AppDbContext _context;

    public ActivityRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Activity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Activities.FindAsync([id], cancellationToken).ConfigureAwait(false);

    public async Task<IReadOnlyList<Activity>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        var activities = await _context.Activities.Where(a => a.ProjectId == projectId).ToListAsync(cancellationToken).ConfigureAwait(false);
        return activities.AsReadOnly();
    }

    public async Task AddAsync(Activity activity, CancellationToken cancellationToken = default)
    {
        await _context.Activities.AddAsync(activity, cancellationToken).ConfigureAwait(false);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateAsync(Activity activity, CancellationToken cancellationToken = default)
    {
        _context.Activities.Update(activity);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteAsync(Activity activity, CancellationToken cancellationToken = default)
    {
        _context.Activities.Remove(activity);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
