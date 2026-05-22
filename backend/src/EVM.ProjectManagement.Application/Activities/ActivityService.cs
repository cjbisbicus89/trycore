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
    private readonly IActivityRepository _activityRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IEVMCalculator _evmCalculator;
    private readonly ILogger<ActivityService> _logger;

    public ActivityService(
        IActivityRepository activityRepository,
        IProjectRepository projectRepository,
        IEVMCalculator evmCalculator,
        ILogger<ActivityService> logger)
    {
        _activityRepository = activityRepository;
        _projectRepository = projectRepository;
        _evmCalculator = evmCalculator;
        _logger = logger;
    }

    public async Task<IReadOnlyList<ActivityResponse>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        var activities = await _activityRepository.GetByProjectIdAsync(projectId, cancellationToken);
        return activities.Select(a => a.ToResponse(_evmCalculator)).ToList().AsReadOnly();
    }

    public async Task<ActivityResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var activity = await _activityRepository.GetByIdOrThrowAsync(id, cancellationToken);
        return activity.ToResponse(_evmCalculator);
    }

    public async Task<ActivityResponse> CreateAsync(CreateActivityRequest request, CancellationToken cancellationToken = default)
    {
        await ValidateProjectExistsAsync(request.ProjectId, cancellationToken);

        var activity = CreateActivityFromRequest(request);
        await _activityRepository.AddAsync(activity, cancellationToken);
        LogActivityCreated(activity);

        return activity.ToResponse(_evmCalculator);
    }

    public async Task<ActivityResponse> UpdateAsync(Guid id, UpdateActivityRequest request, CancellationToken cancellationToken = default)
    {
        var activity = await _activityRepository.GetByIdOrThrowAsync(id, cancellationToken);
        UpdateActivityFromRequest(activity, request);
        await _activityRepository.UpdateAsync(activity, cancellationToken);

        return activity.ToResponse(_evmCalculator);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var activity = await _activityRepository.GetByIdOrThrowAsync(id, cancellationToken);
        await _activityRepository.DeleteAsync(activity, cancellationToken);
    }

    private async Task ValidateProjectExistsAsync(Guid projectId, CancellationToken cancellationToken)
    {
        await _projectRepository.GetByIdOrThrowAsync(projectId, cancellationToken);
    }

    private static Activity CreateActivityFromRequest(CreateActivityRequest request)
    {
        return Activity.Create(
            request.ProjectId,
            request.Name,
            request.BudgetedCost,
            request.PlannedPercentage,
            request.ActualPercentage,
            request.ActualCost);
    }

    private static void UpdateActivityFromRequest(Activity activity, UpdateActivityRequest request)
    {
        activity.Update(
            request.Name,
            request.BudgetedCost,
            request.PlannedPercentage,
            request.ActualPercentage,
            request.ActualCost);
    }

    private void LogActivityCreated(Activity activity)
    {
        _logger.LogInformation(ActivityLogMessages.ActivityCreated, activity.Id, activity.ProjectId);
    }
}
