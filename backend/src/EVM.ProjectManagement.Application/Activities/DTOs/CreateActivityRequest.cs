namespace EVM.ProjectManagement.Application.Activities.DTOs;

public sealed record CreateActivityRequest(
    Guid ProjectId,
    string Name,
    decimal BudgetedCost,
    decimal PlannedPercentage,
    decimal ActualPercentage,
    decimal ActualCost);
