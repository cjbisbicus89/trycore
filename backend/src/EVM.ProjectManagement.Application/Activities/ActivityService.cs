using EVM.ProjectManagement.Application.Activities.DTOs;
using EVM.ProjectManagement.Application.Activities.Extensions;
using EVM.ProjectManagement.Application.Common.Exceptions;
using EVM.ProjectManagement.Domain.Entities;
using EVM.ProjectManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace EVM.ProjectManagement.Application.Activities;

public sealed class ActivityService : IActivityService
{
    private readonly IActivityRepository _activityRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ILogger<ActivityService> _logger;

    public ActivityService(
        IActivityRepository activityRepository,
        IProjectRepository projectRepository,
        ILogger<ActivityService> logger)
    {
        _activityRepository = activityRepository;
        _projectRepository = projectRepository;
        _logger = logger;
    }

    public async Task<IReadOnlyList<ActivityResponse>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        var activities = await _activityRepository.GetByProjectIdAsync(projectId, cancellationToken);
        return activities
            .Select(a => a.ToResponse(new Domain.Services.EVMCalculationService()))
            .ToList()
            .AsReadOnly();
    }

    public async Task<ActivityResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var activity = await _activityRepository.GetByIdAsync(id, cancellationToken);
        if (activity is null)
            throw new NotFoundException($"Activity with ID {id} not found");

        return activity.ToResponse(new Domain.Services.EVMCalculationService());
    }

    public async Task<ActivityResponse> CreateAsync(CreateActivityRequest request, CancellationToken cancellationToken = default)
    {
        // Verificar que el proyecto existe
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project is null)
            throw new NotFoundException($"Project with ID {request.ProjectId} not found");

        var activity = Activity.Create(
            request.ProjectId,
            request.Name,
            request.BudgetedCost,
            request.PlannedPercentage,
            request.ActualPercentage,
            request.ActualCost);

        await _activityRepository.AddAsync(activity, cancellationToken);

        _logger.LogInformation("Activity {ActivityId} created for project {ProjectId}", activity.Id, activity.ProjectId);

        return activity.ToResponse(new Domain.Services.EVMCalculationService());
    }

    public async Task<ActivityResponse> UpdateAsync(Guid id, UpdateActivityRequest request, CancellationToken cancellationToken = default)
    {
        var activity = await _activityRepository.GetByIdAsync(id, cancellationToken);
        if (activity is null)
            throw new NotFoundException($"Activity with ID {id} not found");

        activity.Update(
            request.Name,
            request.BudgetedCost,
            request.PlannedPercentage,
            request.ActualPercentage,
            request.ActualCost);

        await _activityRepository.UpdateAsync(activity, cancellationToken);

        return activity.ToResponse(new Domain.Services.EVMCalculationService());
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var activity = await _activityRepository.GetByIdAsync(id, cancellationToken);
        if (activity is null)
            throw new NotFoundException($"Activity with ID {id} not found");

        await _activityRepository.DeleteAsync(activity, cancellationToken);
    }
}
