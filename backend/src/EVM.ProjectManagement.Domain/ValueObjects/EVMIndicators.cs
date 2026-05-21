namespace EVM.ProjectManagement.Domain.ValueObjects;

public sealed record EVMIndicators(
    decimal PlannedValue,
    decimal EarnedValue,
    decimal ActualCost,
    decimal CostVariance,
    decimal ScheduleVariance,
    decimal? CostPerformanceIndex,
    decimal? SchedulePerformanceIndex,
    decimal? EstimateAtCompletion,
    decimal? VarianceAtCompletion,
    string CostStatus,
    string ScheduleStatus);
