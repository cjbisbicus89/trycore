namespace EVM.ProjectManagement.Application.Projects;

using EVM.ProjectManagement.Application.Projects.DTOs;

public interface IProjectService
{
    Task<IReadOnlyList<ProjectResponse>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<ProjectResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ProjectResponse> CreateAsync(CreateProjectRequest request, CancellationToken cancellationToken = default);

    Task<ProjectResponse> UpdateAsync(Guid id, UpdateProjectRequest request, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
