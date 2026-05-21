using EVM.ProjectManagement.Application.Activities;
using EVM.ProjectManagement.Application.Activities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EVM.ProjectManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ActivitiesController : ControllerBase
{
    private readonly IActivityService _activityService;

    public ActivitiesController(IActivityService activityService)
    {
        _activityService = activityService;
    }

    /// <summary>
    /// Obtener todas las actividades de un proyecto
    /// </summary>
    [HttpGet("project/{projectId}")]
    [ProducesResponseType(typeof(IReadOnlyList<ActivityResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByProjectId(Guid projectId, CancellationToken cancellationToken)
    {
        var activities = await _activityService.GetByProjectIdAsync(projectId, cancellationToken);
        return Ok(activities);
    }

    /// <summary>
    /// Obtener una actividad por ID con sus indicadores EVM
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ActivityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var activity = await _activityService.GetByIdAsync(id, cancellationToken);
        return Ok(activity);
    }

    /// <summary>
    /// Crear una nueva actividad
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ActivityResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateActivityRequest request, CancellationToken cancellationToken)
    {
        var activity = await _activityService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = activity.Id }, activity);
    }

    /// <summary>
    /// Actualizar una actividad existente
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ActivityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateActivityRequest request, CancellationToken cancellationToken)
    {
        var activity = await _activityService.UpdateAsync(id, request, cancellationToken);
        return Ok(activity);
    }

    /// <summary>
    /// Eliminar una actividad
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _activityService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
