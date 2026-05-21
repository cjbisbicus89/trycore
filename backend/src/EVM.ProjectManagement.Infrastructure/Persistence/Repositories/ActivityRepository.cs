using EVM.ProjectManagement.Domain.Entities;
using EVM.ProjectManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EVM.ProjectManagement.Infrastructure.Persistence.Repositories;

public sealed class ActivityRepository : IActivityRepository
{
    private readonly AppDbContext _context;

    public ActivityRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Activity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Activities.FindAsync([id], cancellationToken);

    public async Task<IReadOnlyList<Activity>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default)
        => await _context.Activities.Where(a => a.ProjectId == projectId).ToListAsync(cancellationToken).ContinueWith(t => t.Result.AsReadOnly(), cancellationToken);

    public async Task AddAsync(Activity activity, CancellationToken cancellationToken = default)
        => await _context.Activities.AddAsync(activity, cancellationToken);

    public async Task UpdateAsync(Activity activity, CancellationToken cancellationToken = default)
    {
        _context.Activities.Update(activity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Activity activity, CancellationToken cancellationToken = default)
    {
        _context.Activities.Remove(activity);
        await Task.CompletedTask;
    }
}
