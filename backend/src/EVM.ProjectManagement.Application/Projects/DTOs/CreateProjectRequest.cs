namespace EVM.ProjectManagement.Application.Projects.DTOs;

/// <summary>
/// Representa la solicitud para crear un nuevo proyecto.
/// </summary>
/// <param name="Name">Nombre del proyecto a crear.</param>
/// <param name="Description">Descripción detallada del proyecto.</param>
public sealed record CreateProjectRequest(string Name, string Description);
