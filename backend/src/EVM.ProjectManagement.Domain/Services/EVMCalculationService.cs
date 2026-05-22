namespace EVM.ProjectManagement.Domain.Services;

using EVM.ProjectManagement.Domain.ValueObjects;

public sealed class EVMCalculationService : IEVMCalculator
{
    public EVMIndicators Calculate(decimal plannedValue, decimal earnedValue, decimal actualCost, decimal budgetedCost)
    {
        // Cálculo de variaciones
        var cv = earnedValue - actualCost;
        var sv = earnedValue - plannedValue;

        // Cálculo de índices con manejo de división por cero
        var cpi = actualCost > 0 ? earnedValue / actualCost : (decimal?)null;
        var spi = plannedValue > 0 ? earnedValue / plannedValue : (decimal?)null;

        // Cálculo de proyecciones con manejo de BAC = 0
        var eac = (cpi.HasValue && cpi > 0 && budgetedCost > 0) ? budgetedCost / cpi : (decimal?)null;
        var vac = eac.HasValue ? budgetedCost - eac : (decimal?)null;

        // Interpretación del estado de costos
        var costStatus = cpi switch
        {
            > 1 => EVMStatus.UnderBudget,
            < 1 => EVMStatus.OverBudget,
            1 => EVMStatus.OnBudget,
            _ => EVMStatus.CostNotApplicable,
        };

        // Interpretación del estado del cronograma
        var scheduleStatus = spi switch
        {
            > 1 => EVMStatus.AheadOfSchedule,
            < 1 => EVMStatus.BehindSchedule,
            1 => EVMStatus.OnSchedule,
            _ => EVMStatus.ScheduleNotApplicable,
        };

        return new EVMIndicators(
            plannedValue,
            earnedValue,
            actualCost,
            cv,
            sv,
            cpi,
            spi,
            eac,
            vac,
            costStatus,
            scheduleStatus);
    }
}
