using EVM.ProjectManagement.Application.Activities.DTOs;
using EVM.ProjectManagement.Application.Projects.DTOs;
using EVM.ProjectManagement.Domain.Entities;
using EVM.ProjectManagement.Domain.ValueObjects;

namespace EVM.ProjectManagement.Application.Projects.Extensions;

public static class ProjectMappingExtensions
{
    public static ActivityResponse ToResponse(this Activity activity, EVMIndicators indicators)
        => new(
            activity.Id,
            activity.ProjectId,
            activity.Name,
            activity.BudgetedCost,
            activity.PlannedPercentage,
            activity.ActualPercentage,
            activity.ActualCost,
            activity.PlannedValue,
            activity.EarnedValue,
            indicators);

    public static ProjectResponse ToResponse(this Project project, EVMIndicators? indicators, IReadOnlyList<ActivityResponse> activities)
        => new(
            project.Id,
            project.Name,
            project.Description,
            project.CreatedAt,
            indicators,
            activities);
}
