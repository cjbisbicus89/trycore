namespace EVM.ProjectManagement.API.Controllers;

using EVM.ProjectManagement.Application.Activities;
using EVM.ProjectManagement.Application.Activities.DTOs;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public sealed class ActivitiesController : ControllerBase
{
    private readonly IActivityService activityService;

    public ActivitiesController(IActivityService activityService)
    {
        this.activityService = activityService;
    }

    /// <summary>
    /// Obtener todas las actividades de un proyecto.
    /// </summary>
    /// <param name="projectId">ID del proyecto.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Lista de todas las actividades del proyecto.</returns>
    [HttpGet("project/{projectId}")]
    [ProducesResponseType(typeof(IReadOnlyList<ActivityResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByProjectId(Guid projectId, CancellationToken cancellationToken)
    {
        var activities = await this.activityService.GetByProjectIdAsync(projectId, cancellationToken);
        return this.Ok(activities);
    }

    /// <summary>
    /// Obtener una actividad por ID con sus indicadores EVM.
    /// </summary>
    /// <param name="id">ID de la actividad.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>La actividad solicitada con sus indicadores EVM.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ActivityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var activity = await this.activityService.GetByIdAsync(id, cancellationToken);
        return this.Ok(activity);
    }

    /// <summary>
    /// Crear una nueva actividad.
    /// </summary>
    /// <param name="request">Datos de la actividad a crear.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>La actividad creada.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ActivityResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateActivityRequest request, CancellationToken cancellationToken)
    {
        var activity = await this.activityService.CreateAsync(request, cancellationToken);
        return this.CreatedAtAction(nameof(this.GetById), new { id = activity.Id }, activity);
    }

    /// <summary>
    /// Actualizar una actividad existente.
    /// </summary>
    /// <param name="id">ID de la actividad a actualizar.</param>
    /// <param name="request">Datos actualizados de la actividad.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>La actividad actualizada.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ActivityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateActivityRequest request, CancellationToken cancellationToken)
    {
        var activity = await this.activityService.UpdateAsync(id, request, cancellationToken);
        return this.Ok(activity);
    }

    /// <summary>
    /// Eliminar una actividad.
    /// </summary>
    /// <param name="id">ID de la actividad a eliminar.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Resultado de la operación.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await this.activityService.DeleteAsync(id, cancellationToken);
        return this.NoContent();
    }
}
