namespace EVM.ProjectManagement.Application.Projects;

using EVM.ProjectManagement.Application.Activities.DTOs;
using EVM.ProjectManagement.Application.Common.Exceptions;
using EVM.ProjectManagement.Application.Projects.DTOs;
using EVM.ProjectManagement.Application.Projects.Extensions;
using EVM.ProjectManagement.Domain.Entities;
using EVM.ProjectManagement.Domain.Repositories;
using EVM.ProjectManagement.Domain.Services;
using EVM.ProjectManagement.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

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
        var project = await _projectRepository.GetWithActivitiesOrThrowAsync(id, cancellationToken);
        var consolidatedIndicators = CalculateConsolidatedIndicators(project);
        var activityResponses = MapActivitiesWithIndicators(project.Activities);

        return project.ToResponse(consolidatedIndicators, activityResponses);
    }

    private EVMIndicators CalculateConsolidatedIndicators(Project project)
    {
        return _evmCalculator.Calculate(
            project.TotalPlannedValue,
            project.TotalEarnedValue,
            project.TotalActualCost,
            project.TotalBudgetedCost);
    }

    private IReadOnlyList<ActivityResponse> MapActivitiesWithIndicators(IReadOnlyCollection<Activity> activities)
    {
        return activities
            .Select(a =>
            {
                var indicators = _evmCalculator.Calculate(
                    a.PlannedValue,
                    a.EarnedValue,
                    a.ActualCost,
                    a.BudgetedCost);
                return a.ToResponse(indicators);
            })
            .ToList()
            .AsReadOnly();
    }

    public async Task<ProjectResponse> CreateAsync(CreateProjectRequest request, CancellationToken cancellationToken = default)
    {
        var project = Domain.Entities.Project.Create(request.Name, request.Description);
        await _projectRepository.AddAsync(project, cancellationToken);

        _logger.LogInformation(ProjectLogMessages.ProjectCreated, project.Id, project.Name);

        return project.ToResponse(null, []);
    }

    public async Task<ProjectResponse> UpdateAsync(Guid id, UpdateProjectRequest request, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetByIdOrThrowAsync(id, cancellationToken);
        project.Update(request.Name, request.Description);
        await _projectRepository.UpdateAsync(project, cancellationToken);

        return project.ToResponse(null, []);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetByIdOrThrowAsync(id, cancellationToken);
        await _projectRepository.DeleteAsync(project, cancellationToken);
    }
}
