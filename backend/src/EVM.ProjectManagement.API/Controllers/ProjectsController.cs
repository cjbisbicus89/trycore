namespace EVM.ProjectManagement.API.Controllers;

using EVM.ProjectManagement.Application.Projects;
using EVM.ProjectManagement.Application.Projects.DTOs;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public sealed class ProjectsController : ControllerBase
{
    private readonly IProjectService projectService;

    public ProjectsController(IProjectService projectService)
    {
        this.projectService = projectService;
    }

    /// <summary>
    /// Obtener todos los proyectos.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Lista de todos los proyectos.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProjectResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var projects = await this.projectService.GetAllAsync(cancellationToken);
        return this.Ok(projects);
    }

    /// <summary>
    /// Obtener un proyecto por ID con sus actividades e indicadores consolidados.
    /// </summary>
    /// <param name="id">ID del proyecto.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>El proyecto solicitado con sus actividades e indicadores.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var project = await this.projectService.GetByIdAsync(id, cancellationToken);
        return this.Ok(project);
    }

    /// <summary>
    /// Crear un nuevo proyecto.
    /// </summary>
    /// <param name="request">Datos del proyecto a crear.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>El proyecto creado.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var project = await this.projectService.CreateAsync(request, cancellationToken);
        return this.CreatedAtAction(nameof(this.GetById), new { id = project.Id }, project);
    }

    /// <summary>
    /// Actualizar un proyecto existente.
    /// </summary>
    /// <param name="id">ID del proyecto a actualizar.</param>
    /// <param name="request">Datos actualizados del proyecto.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>El proyecto actualizado.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectRequest request, CancellationToken cancellationToken)
    {
        var project = await this.projectService.UpdateAsync(id, request, cancellationToken);
        return this.Ok(project);
    }

    /// <summary>
    /// Eliminar un proyecto.
    /// </summary>
    /// <param name="id">ID del proyecto a eliminar.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Resultado de la operación.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await this.projectService.DeleteAsync(id, cancellationToken);
        return this.NoContent();
    }
}
