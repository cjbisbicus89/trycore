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
        return activities.Select(a => a.ToResponse(this.evmCalculator)).ToList().AsReadOnly();
    }

    public async Task<ActivityResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var activity = await this.activityRepository.GetByIdOrThrowAsync(id, cancellationToken);
        return activity.ToResponse(this.evmCalculator);
    }

    public async Task<ActivityResponse> CreateAsync(CreateActivityRequest request, CancellationToken cancellationToken = default)
    {
        await ValidateProjectExistsAsync(request.ProjectId, cancellationToken);

        var activity = CreateActivityFromRequest(request);
        await this.activityRepository.AddAsync(activity, cancellationToken);
        LogActivityCreated(activity);

        return activity.ToResponse(this.evmCalculator);
    }

    public async Task<ActivityResponse> UpdateAsync(Guid id, UpdateActivityRequest request, CancellationToken cancellationToken = default)
    {
        var activity = await this.activityRepository.GetByIdOrThrowAsync(id, cancellationToken);
        UpdateActivityFromRequest(activity, request);
        await this.activityRepository.UpdateAsync(activity, cancellationToken);

        return activity.ToResponse(this.evmCalculator);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var activity = await this.activityRepository.GetByIdOrThrowAsync(id, cancellationToken);
        await this.activityRepository.DeleteAsync(activity, cancellationToken);
    }

    private async Task ValidateProjectExistsAsync(Guid projectId, CancellationToken cancellationToken)
    {
        await this.projectRepository.GetByIdOrThrowAsync(projectId, cancellationToken);
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
        this.logger.LogInformation(ActivityLogMessages.ActivityCreated, activity.Id, activity.ProjectId);
    }
}
