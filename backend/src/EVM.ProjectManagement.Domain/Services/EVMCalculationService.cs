using EVM.ProjectManagement.Domain.ValueObjects;

namespace EVM.ProjectManagement.Domain.Services;

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

        // Cálculo de proyecciones
        var eac = cpi.HasValue && cpi > 0 ? budgetedCost / cpi : (decimal?)null;
        var vac = eac.HasValue ? budgetedCost - eac : (decimal?)null;

        // Interpretación del estado de costos
        var costStatus = cpi switch
        {
            > 1 => "Under Budget",
            < 1 => "Over Budget",
            1 => "On Budget",
            _ => "N/A"
        };

        // Interpretación del estado del cronograma
        var scheduleStatus = spi switch
        {
            > 1 => "Ahead of Schedule",
            < 1 => "Behind Schedule",
            1 => "On Schedule",
            _ => "N/A"
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
