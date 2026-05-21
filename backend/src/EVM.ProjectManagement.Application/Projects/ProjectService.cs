using EVM.ProjectManagement.Application.Activities.DTOs;
using EVM.ProjectManagement.Application.Projects.DTOs;
using EVM.ProjectManagement.Application.Projects.Extensions;
using EVM.ProjectManagement.Application.Common.Exceptions;
using EVM.ProjectManagement.Domain.Repositories;
using EVM.ProjectManagement.Domain.Services;
using EVM.ProjectManagement.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace EVM.ProjectManagement.Application.Projects;

public sealed class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IEVMCalculator _evmCalculator;
    private readonly ILogger<ProjectService> _logger;

    public ProjectService(
        IProjectRepository projectRepository,
        IEVMCalculator evmCalculator,
        ILogger<ProjectService> logger)
    {
        _projectRepository = projectRepository;
        _evmCalculator = evmCalculator;
        _logger = logger;
    }

    public async Task<IReadOnlyList<ProjectResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var projects = await _projectRepository.GetAllAsync(cancellationToken);
        return projects.Select(p => p.ToResponse(null, [])).ToList().AsReadOnly();
    }

    public async Task<ProjectResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetWithActivitiesAsync(id, cancellationToken);
        if (project is null)
            throw new NotFoundException($"Project with ID {id} not found");

        // Calcular indicadores consolidados
        var indicators = _evmCalculator.Calculate(
            project.TotalPlannedValue,
            project.TotalEarnedValue,
            project.TotalActualCost,
            project.TotalBudgetedCost);

        // Mapear actividades con sus indicadores individuales
        var activityResponses = project.Activities
            .Select(a => a.ToResponse(_evmCalculator))
            .ToList()
            .AsReadOnly();

        return project.ToResponse(indicators, activityResponses);
    }

    public async Task<ProjectResponse> CreateAsync(CreateProjectRequest request, CancellationToken cancellationToken = default)
    {
        var project = Domain.Entities.Project.Create(request.Name, request.Description);
        await _projectRepository.AddAsync(project, cancellationToken);

        _logger.LogInformation("Project {ProjectId} created with name {ProjectName}", project.Id, project.Name);

        return project.ToResponse(null, []);
    }

    public async Task<ProjectResponse> UpdateAsync(Guid id, UpdateProjectRequest request, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetByIdAsync(id, cancellationToken);
        if (project is null)
            throw new NotFoundException($"Project with ID {id} not found");

        project.Update(request.Name, request.Description);
        await _projectRepository.UpdateAsync(project, cancellationToken);

        return project.ToResponse(null, []);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetByIdAsync(id, cancellationToken);
        if (project is null)
            throw new NotFoundException($"Project with ID {id} not found");

        await _projectRepository.DeleteAsync(project, cancellationToken);
    }
}
