namespace EVM.ProjectManagement.Domain.Repositories;

using EVM.ProjectManagement.Domain.Entities;

public interface IActivityRepository
{
    Task<Activity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Activity>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default);

    Task AddAsync(Activity activity, CancellationToken cancellationToken = default);

    Task UpdateAsync(Activity activity, CancellationToken cancellationToken = default);

    Task DeleteAsync(Activity activity, CancellationToken cancellationToken = default);
}
