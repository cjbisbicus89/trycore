namespace EVM.ProjectManagement.Application.Projects;

using EVM.ProjectManagement.Application.Activities;
using EVM.ProjectManagement.Application.Activities.DTOs;
using EVM.ProjectManagement.Application.Common.Exceptions;
using EVM.ProjectManagement.Application.Projects.DTOs;
using EVM.ProjectManagement.Application.Projects.Extensions;
using EVM.ProjectManagement.Domain.Repositories;
using EVM.ProjectManagement.Domain.Services;
using EVM.ProjectManagement.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

public sealed class ProjectService : IProjectService
{
    private readonly IProjectRepository projectRepository;
    private readonly IEVMCalculator evmCalculator;
    private readonly ILogger<ProjectService> logger;

    public ProjectService(
        IProjectRepository projectRepository,
        IEVMCalculator evmCalculator,
        ILogger<ProjectService> logger)
    {
        this.projectRepository = projectRepository;
        this.evmCalculator = evmCalculator;
        this.logger = logger;
    }

    public async Task<IReadOnlyList<ProjectResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var projects = await this.projectRepository.GetAllAsync(cancellationToken);
        return projects.Select(p => p.ToResponse(null, [])).ToList().AsReadOnly();
    }

    public async Task<ProjectResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var project = await this.projectRepository.GetWithActivitiesAsync(id, cancellationToken);
        if (project is null)
        {
            throw new NotFoundException($"Project with ID {id} not found");
        }

        // Calcular indicadores consolidados
        var indicators = this.evmCalculator.Calculate(
            project.TotalPlannedValue,
            project.TotalEarnedValue,
            project.TotalActualCost,
            project.TotalBudgetedCost);

        // Mapear actividades con sus indicadores individuales
        var activityResponses = project.Activities
            .Select(a =>
            {
                var indicators = this.evmCalculator.Calculate(
                    a.PlannedValue,
                    a.EarnedValue,
                    a.ActualCost,
                    a.BudgetedCost);
                return a.ToResponse(indicators);
            })
            .ToList()
            .AsReadOnly();

        return project.ToResponse(indicators, activityResponses);
    }

    public async Task<ProjectResponse> CreateAsync(CreateProjectRequest request, CancellationToken cancellationToken = default)
    {
        var project = Domain.Entities.Project.Create(request.Name, request.Description);
        await this.projectRepository.AddAsync(project, cancellationToken);

#pragma warning disable CA1848
        this.logger.LogInformation("Project {ProjectId} created with name {ProjectName}", project.Id, project.Name);
#pragma warning restore CA1848

        return project.ToResponse(null, []);
    }

    public async Task<ProjectResponse> UpdateAsync(Guid id, UpdateProjectRequest request, CancellationToken cancellationToken = default)
    {
        var project = await this.projectRepository.GetByIdAsync(id, cancellationToken);
        if (project is null)
        {
            throw new NotFoundException($"Project with ID {id} not found");
        }

        project.Update(request.Name, request.Description);
        await this.projectRepository.UpdateAsync(project, cancellationToken);

        return project.ToResponse(null, []);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var project = await this.projectRepository.GetByIdAsync(id, cancellationToken);
        if (project is null)
        {
            throw new NotFoundException($"Project with ID {id} not found");
        }

        await this.projectRepository.DeleteAsync(project, cancellationToken);
    }
}
