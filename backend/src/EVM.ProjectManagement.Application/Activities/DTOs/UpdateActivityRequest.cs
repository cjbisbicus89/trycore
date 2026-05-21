namespace EVM.ProjectManagement.Application.Activities.DTOs;

public sealed record UpdateActivityRequest(
    string Name,
    decimal BudgetedCost,
    decimal PlannedPercentage,
    decimal ActualPercentage,
    decimal ActualCost);
