namespace EVM.ProjectManagement.Application.Activities.Extensions;

using EVM.ProjectManagement.Application.Activities.DTOs;
using EVM.ProjectManagement.Domain.Entities;
using EVM.ProjectManagement.Domain.Services;
using EVM.ProjectManagement.Domain.ValueObjects;

public static class ActivityMappingExtensions
{
    public static ActivityResponse ToResponse(this Activity activity, IEVMCalculator calculator)
    {
        var indicators = calculator.Calculate(
            activity.PlannedValue,
            activity.EarnedValue,
            activity.ActualCost,
            activity.BudgetedCost);

        return new ActivityResponse(
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
    }
}
