using EVM.ProjectManagement.Application.Activities;
using EVM.ProjectManagement.Application.Projects;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace EVM.ProjectManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Registrar servicios de aplicación
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IActivityService, ActivityService>();

        // Registrar validadores de FluentValidation
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}
