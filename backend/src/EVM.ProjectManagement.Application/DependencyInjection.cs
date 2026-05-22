namespace EVM.ProjectManagement.Application;

using EVM.ProjectManagement.Application.Activities;
using EVM.ProjectManagement.Application.Projects;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IActivityService, ActivityService>();

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}
