namespace EVM.ProjectManagement.Application.Projects.DTOs;

using EVM.ProjectManagement.Application.Activities.DTOs;
using EVM.ProjectManagement.Domain.ValueObjects;

/// <summary>
/// Representa la respuesta de un proyecto con sus indicadores EVM y actividades asociadas.
/// </summary>
/// <param name="Id">Identificador único del proyecto.</param>
/// <param name="Name">Nombre del proyecto.</param>
/// <param name="Description">Descripción detallada del proyecto.</param>
/// <param name="CreatedAt">Fecha y hora de creación del proyecto (UTC).</param>
/// <param name="Indicators">Indicadores EVM consolidados del proyecto (null si no tiene actividades).</param>
/// <param name="Activities">Lista de actividades del proyecto con sus indicadores individuales.</param>
public sealed record ProjectResponse(
    Guid Id,
    string Name,
    string Description,
    DateTime CreatedAt,
    EVMIndicators? Indicators,
    IReadOnlyList<ActivityResponse> Activities);
