namespace EVM.ProjectManagement.Application.Common.Exceptions;

using EVM.ProjectManagement.Domain.Entities;
using EVM.ProjectManagement.Domain.Repositories;

public static class RepositoryExtensions
{
    public static async Task<Activity> GetByIdOrThrowAsync(
        this IActivityRepository repository,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var activity = await repository.GetByIdAsync(id, cancellationToken);
        if (activity is null)
        {
            throw new NotFoundException(string.Format(ActivityErrors.ActivityNotFound, id));
        }
        return activity;
    }

    public static async Task<Project> GetByIdOrThrowAsync(
        this IProjectRepository repository,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var project = await repository.GetByIdAsync(id, cancellationToken);
        if (project is null)
        {
            throw new NotFoundException(string.Format(ProjectErrors.ProjectNotFound, id));
        }
        return project;
    }

    public static async Task<Project> GetWithActivitiesOrThrowAsync(
        this IProjectRepository repository,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var project = await repository.GetWithActivitiesAsync(id, cancellationToken);
        if (project is null)
        {
            throw new NotFoundException(string.Format(ProjectErrors.ProjectNotFound, id));
        }
        return project;
    }
}
