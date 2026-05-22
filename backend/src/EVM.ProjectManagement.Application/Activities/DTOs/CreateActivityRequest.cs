namespace EVM.ProjectManagement.Application.Activities.DTOs;

/// <summary>
/// Representa la solicitud para crear una nueva actividad en un proyecto.
/// </summary>
/// <param name="ProjectId">Identificador único del proyecto al que pertenece la actividad.</param>
/// <param name="Name">Nombre de la actividad a crear.</param>
/// <param name="BudgetedCost">Presupuesto total planificado (BAC - Budget at Completion). Debe ser mayor a cero.</param>
/// <param name="PlannedPercentage">Porcentaje de avance planificado a la fecha de corte (0-100).</param>
/// <param name="ActualPercentage">Porcentaje de avance real completado (0-100).</param>
/// <param name="ActualCost">Costo real incurrido hasta la fecha. Debe ser no negativo.</param>
public sealed record CreateActivityRequest(
    Guid ProjectId,
    string Name,
    decimal BudgetedCost,
    decimal PlannedPercentage,
    decimal ActualPercentage,
    decimal ActualCost);
