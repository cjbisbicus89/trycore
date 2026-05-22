namespace EVM.ProjectManagement.Domain.Services;

using EVM.ProjectManagement.Domain.ValueObjects;

public sealed class EVMCalculationService : IEVMCalculator
{
    public EVMIndicators Calculate(decimal plannedValue, decimal earnedValue, decimal actualCost, decimal budgetedCost)
    {
        var costVariance = earnedValue - actualCost;
        var scheduleVariance = earnedValue - plannedValue;

        var costPerformanceIndex = actualCost > EVMConstants.MinimumValue ? earnedValue / actualCost : (decimal?)null;
        var schedulePerformanceIndex = plannedValue > EVMConstants.MinimumValue ? earnedValue / plannedValue : (decimal?)null;

        var estimateAtCompletion = (costPerformanceIndex.HasValue && costPerformanceIndex > EVMConstants.MinimumValue && budgetedCost > EVMConstants.MinimumValue)
            ? budgetedCost / costPerformanceIndex
            : (decimal?)null;
        var varianceAtCompletion = estimateAtCompletion.HasValue
            ? budgetedCost - estimateAtCompletion
            : (decimal?)null;

        var costStatus = DetermineCostStatus(costPerformanceIndex);
        var scheduleStatus = DetermineScheduleStatus(schedulePerformanceIndex);

        return new EVMIndicators(
            plannedValue,
            earnedValue,
            actualCost,
            costVariance,
            scheduleVariance,
            costPerformanceIndex,
            schedulePerformanceIndex,
            estimateAtCompletion,
            varianceAtCompletion,
            costStatus,
            scheduleStatus);
    }

    private static string DetermineCostStatus(decimal? costPerformanceIndex)
    {
        return costPerformanceIndex switch
        {
            > EVMConstants.PerformanceIndexThreshold => EVMStatus.BajoPresupuesto,
            < EVMConstants.PerformanceIndexThreshold => EVMStatus.SobrePresupuesto,
            EVMConstants.PerformanceIndexThreshold => EVMStatus.EnPresupuesto,
            _ => EVMStatus.CostoNoAplicable,
        };
    }

    private static string DetermineScheduleStatus(decimal? schedulePerformanceIndex)
    {
        return schedulePerformanceIndex switch
        {
            > EVMConstants.PerformanceIndexThreshold => EVMStatus.AdelantadoCronograma,
            < EVMConstants.PerformanceIndexThreshold => EVMStatus.AtrasadoCronograma,
            EVMConstants.PerformanceIndexThreshold => EVMStatus.EnCronograma,
            _ => EVMStatus.CronogramaNoAplicable,
        };
    }
}
