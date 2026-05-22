namespace EVM.ProjectManagement.Application.Activities.DTOs;

/// <summary>
/// Representa la solicitud para actualizar una actividad existente.
/// </summary>
/// <param name="Name">Nombre actualizado de la actividad.</param>
/// <param name="BudgetedCost">Presupuesto total planificado actualizado (BAC). Debe ser mayor a cero.</param>
/// <param name="PlannedPercentage">Porcentaje de avance planificado actualizado (0-100).</param>
/// <param name="ActualPercentage">Porcentaje de avance real actualizado (0-100).</param>
/// <param name="ActualCost">Costo real actualizado. Debe ser no negativo.</param>
public sealed record UpdateActivityRequest(
    string Name,
    decimal BudgetedCost,
    decimal PlannedPercentage,
    decimal ActualPercentage,
    decimal ActualCost);
