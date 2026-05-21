namespace EVM.ProjectManagement.Application.Activities.DTOs;

using EVM.ProjectManagement.Domain.ValueObjects;

public sealed record ActivityResponse(
    Guid Id,
    Guid ProjectId,
    string Name,
    decimal BudgetedCost,
    decimal PlannedPercentage,
    decimal ActualPercentage,
    decimal ActualCost,
    decimal PlannedValue,
    decimal EarnedValue,
    EVMIndicators Indicators);
