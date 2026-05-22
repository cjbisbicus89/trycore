namespace EVM.ProjectManagement.API.Controllers;

using EVM.ProjectManagement.Application.Activities;
using EVM.ProjectManagement.Application.Activities.DTOs;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class ActivitiesController : ControllerBase
{
    private readonly IActivityService _activityService;

    public ActivitiesController(IActivityService activityService)
    {
        _activityService = activityService;
    }

    /// <summary>
    /// Obtiene todas las actividades de un proyecto específico.
    /// </summary>
    /// <param name="projectId">Identificador único del proyecto.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>Lista de todas las actividades del proyecto con sus indicadores EVM.</returns>
    /// <response code="200">Retorna la lista de actividades exitosamente.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpGet("project/{projectId}")]
    [ProducesResponseType(typeof(IReadOnlyList<ActivityResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByProjectId(Guid projectId, CancellationToken cancellationToken)
    {
        var activities = await _activityService.GetByProjectIdAsync(projectId, cancellationToken);
        return Ok(activities);
    }

    /// <summary>
    /// Obtiene una actividad específica por su identificador único con sus indicadores EVM calculados.
    /// </summary>
    /// <param name="id">Identificador único de la actividad (GUID).</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>La actividad solicitada con sus indicadores EVM.</returns>
    /// <response code="200">Retorna la actividad solicitada.</response>
    /// <response code="404">La actividad con el ID especificado no existe.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ActivityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var activity = await _activityService.GetByIdAsync(id, cancellationToken);
        return Ok(activity);
    }

    /// <summary>
    /// Crea una nueva actividad en el sistema asociada a un proyecto existente.
    /// </summary>
    /// <param name="request">Datos de la actividad a crear.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>La actividad creada con su identificador asignado.</returns>
    /// <response code="201">Actividad creada exitosamente. Retorna el Location header.</response>
    /// <response code="400">Los datos de entrada son inválidos (validación fallida) o el proyecto no existe.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ActivityResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateActivityRequest request, CancellationToken cancellationToken)
    {
        var activity = await _activityService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = activity.Id }, activity);
    }

    /// <summary>
    /// Actualiza los datos de una actividad existente.
    /// </summary>
    /// <param name="id">Identificador único de la actividad a actualizar.</param>
    /// <param name="request">Datos actualizados de la actividad.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>La actividad con los datos actualizados.</returns>
    /// <response code="200">Actividad actualizada exitosamente.</response>
    /// <response code="400">Los datos de entrada son inválidos (validación fallida).</response>
    /// <response code="404">La actividad con el ID especificado no existe.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ActivityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateActivityRequest request, CancellationToken cancellationToken)
    {
        var activity = await _activityService.UpdateAsync(id, request, cancellationToken);
        return Ok(activity);
    }

    /// <summary>
    /// Elimina una actividad del sistema permanentemente.
    /// </summary>
    /// <param name="id">Identificador único de la actividad a eliminar.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>Resultado de la operación sin contenido (204 No Content).</returns>
    /// <response code="204">Actividad eliminada exitosamente. No retorna contenido.</response>
    /// <response code="404">La actividad con el ID especificado no existe.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _activityService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
