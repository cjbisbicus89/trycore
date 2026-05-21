namespace EVM.ProjectManagement.Application.Activities;

using EVM.ProjectManagement.Application.Activities.DTOs;
using EVM.ProjectManagement.Application.Activities.Extensions;
using EVM.ProjectManagement.Application.Common.Exceptions;
using EVM.ProjectManagement.Domain.Entities;
using EVM.ProjectManagement.Domain.Repositories;
using EVM.ProjectManagement.Domain.Services;
using Microsoft.Extensions.Logging;

public sealed class ActivityService : IActivityService
{
    private readonly IActivityRepository activityRepository;
    private readonly IProjectRepository projectRepository;
    private readonly IEVMCalculator evmCalculator;
    private readonly ILogger<ActivityService> logger;

    public ActivityService(
        IActivityRepository activityRepository,
        IProjectRepository projectRepository,
        IEVMCalculator evmCalculator,
        ILogger<ActivityService> logger)
    {
        this.activityRepository = activityRepository;
        this.projectRepository = projectRepository;
        this.evmCalculator = evmCalculator;
        this.logger = logger;
    }

    public async Task<IReadOnlyList<ActivityResponse>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        var activities = await this.activityRepository.GetByProjectIdAsync(projectId, cancellationToken);
        return activities
            .Select(a => a.ToResponse(this.evmCalculator))
            .ToList()
            .AsReadOnly();
    }

    public async Task<ActivityResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var activity = await this.activityRepository.GetByIdAsync(id, cancellationToken);
        if (activity is null)
        {
            throw new NotFoundException($"Activity with ID {id} not found");
        }

        return activity.ToResponse(this.evmCalculator);
    }

    public async Task<ActivityResponse> CreateAsync(CreateActivityRequest request, CancellationToken cancellationToken = default)
    {
        // Verificar que el proyecto existe
        var project = await this.projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project is null)
        {
            throw new NotFoundException($"Project with ID {request.ProjectId} not found");
        }

        var activity = Activity.Create(
            request.ProjectId,
            request.Name,
            request.BudgetedCost,
            request.PlannedPercentage,
            request.ActualPercentage,
            request.ActualCost);

        await this.activityRepository.AddAsync(activity, cancellationToken);

#pragma warning disable CA1848
        this.logger.LogInformation("Activity {ActivityId} created for project {ProjectId}", activity.Id, activity.ProjectId);
#pragma warning restore CA1848

        return activity.ToResponse(this.evmCalculator);
    }

    public async Task<ActivityResponse> UpdateAsync(Guid id, UpdateActivityRequest request, CancellationToken cancellationToken = default)
    {
        var activity = await this.activityRepository.GetByIdAsync(id, cancellationToken);
        if (activity is null)
        {
            throw new NotFoundException($"Activity with ID {id} not found");
        }

        activity.Update(
            request.Name,
            request.BudgetedCost,
            request.PlannedPercentage,
            request.ActualPercentage,
            request.ActualCost);

        await this.activityRepository.UpdateAsync(activity, cancellationToken);

        return activity.ToResponse(this.evmCalculator);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var activity = await this.activityRepository.GetByIdAsync(id, cancellationToken);
        if (activity is null)
        {
            throw new NotFoundException($"Activity with ID {id} not found");
        }

        await this.activityRepository.DeleteAsync(activity, cancellationToken);
    }
}
