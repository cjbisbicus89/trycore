namespace EVM.ProjectManagement.Domain.Repositories;

using EVM.ProjectManagement.Domain.Entities;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Project?> GetWithActivitiesAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Project>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Project project, CancellationToken cancellationToken = default);

    Task UpdateAsync(Project project, CancellationToken cancellationToken = default);

    Task DeleteAsync(Project project, CancellationToken cancellationToken = default);
}
