using EVM.ProjectManagement.Application.Activities.DTOs;

namespace EVM.ProjectManagement.Application.Activities;

public interface IActivityService
{
    Task<IReadOnlyList<ActivityResponse>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default);
    Task<ActivityResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ActivityResponse> CreateAsync(CreateActivityRequest request, CancellationToken cancellationToken = default);
    Task<ActivityResponse> UpdateAsync(Guid id, UpdateActivityRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
