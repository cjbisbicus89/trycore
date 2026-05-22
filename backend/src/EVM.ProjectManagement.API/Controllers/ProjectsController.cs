namespace EVM.ProjectManagement.API.Controllers;

using EVM.ProjectManagement.Application.Projects;
using EVM.ProjectManagement.Application.Projects.DTOs;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    /// <summary>
    /// Obtiene todos los proyectos registrados en el sistema.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>Lista de todos los proyectos con sus datos básicos.</returns>
    /// <response code="200">Retorna la lista de proyectos exitosamente.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProjectResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var projects = await _projectService.GetAllAsync(cancellationToken);
        return Ok(projects);
    }

    /// <summary>
    /// Obtiene un proyecto específico por su identificador único, incluyendo sus actividades e indicadores EVM consolidados.
    /// </summary>
    /// <param name="id">Identificador único del proyecto (GUID).</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>El proyecto solicitado con todas sus actividades y los indicadores EVM calculados.</returns>
    /// <response code="200">Retorna el proyecto solicitado.</response>
    /// <response code="404">El proyecto con el ID especificado no existe.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var project = await _projectService.GetByIdAsync(id, cancellationToken);
        return Ok(project);
    }

    /// <summary>
    /// Crea un nuevo proyecto en el sistema.
    /// </summary>
    /// <param name="request">Datos del proyecto a crear (nombre y descripción).</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>El proyecto creado con su identificador asignado.</returns>
    /// <response code="201">Proyecto creado exitosamente. Retorna el Location header.</response>
    /// <response code="400">Los datos de entrada son inválidos (validación fallida).</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var project = await _projectService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
    }

    /// <summary>
    /// Actualiza los datos de un proyecto existente.
    /// </summary>
    /// <param name="id">Identificador único del proyecto a actualizar.</param>
    /// <param name="request">Datos actualizados del proyecto (nombre y descripción).</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>El proyecto con los datos actualizados.</returns>
    /// <response code="200">Proyecto actualizado exitosamente.</response>
    /// <response code="400">Los datos de entrada son inválidos (validación fallida).</response>
    /// <response code="404">El proyecto con el ID especificado no existe.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectRequest request, CancellationToken cancellationToken)
    {
        var project = await _projectService.UpdateAsync(id, request, cancellationToken);
        return Ok(project);
    }

    /// <summary>
    /// Elimina un proyecto del sistema permanentemente.
    /// </summary>
    /// <param name="id">Identificador único del proyecto a eliminar.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>Resultado de la operación sin contenido (204 No Content).</returns>
    /// <response code="204">Proyecto eliminado exitosamente. No retorna contenido.</response>
    /// <response code="404">El proyecto con el ID especificado no existe.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _projectService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
