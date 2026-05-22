namespace EVM.ProjectManagement.Application.Activities.DTOs;

using EVM.ProjectManagement.Domain.ValueObjects;

/// <summary>
/// Representa la respuesta de una actividad con sus indicadores EVM calculados.
/// </summary>
/// <param name="Id">Identificador único de la actividad.</param>
/// <param name="ProjectId">Identificador único del proyecto al que pertenece.</param>
/// <param name="Name">Nombre de la actividad.</param>
/// <param name="BudgetedCost">Presupuesto total planificado (BAC - Budget at Completion).</param>
/// <param name="PlannedPercentage">Porcentaje de avance planificado a la fecha de corte (0-100).</param>
/// <param name="ActualPercentage">Porcentaje de avance real completado (0-100).</param>
/// <param name="ActualCost">Costo real incurrido hasta la fecha.</param>
/// <param name="PlannedValue">Valor planificado (PV) calculado automáticamente.</param>
/// <param name="EarnedValue">Valor ganado (EV) calculado automáticamente.</param>
/// <param name="Indicators">Indicadores EVM completos de la actividad.</param>
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
