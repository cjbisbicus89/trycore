namespace EVM.ProjectManagement.Application.Projects.DTOs;

/// <summary>
/// Representa la solicitud para actualizar un proyecto existente.
/// </summary>
/// <param name="Name">Nombre actualizado del proyecto.</param>
/// <param name="Description">Descripción actualizada del proyecto.</param>
public sealed record UpdateProjectRequest(string Name, string Description);
