namespace EVM.ProjectManagement.Domain.ValueObjects;

using EVM.ProjectManagement.Domain.Services;

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
    string ScheduleStatus)
{
    public static EVMIndicators Empty => new(
        PlannedValue: 0m,
        EarnedValue: 0m,
        ActualCost: 0m,
        CostVariance: 0m,
        ScheduleVariance: 0m,
        CostPerformanceIndex: null,
        SchedulePerformanceIndex: null,
        EstimateAtCompletion: null,
        VarianceAtCompletion: null,
        CostStatus: EVMStatus.CostoNoAplicable,
        ScheduleStatus: EVMStatus.CronogramaNoAplicable);
}
