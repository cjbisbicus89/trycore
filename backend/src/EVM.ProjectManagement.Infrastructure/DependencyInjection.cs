namespace EVM.ProjectManagement.Infrastructure;

using EVM.ProjectManagement.Domain.Repositories;
using EVM.ProjectManagement.Domain.Services;
using EVM.ProjectManagement.Infrastructure.Persistence;
using EVM.ProjectManagement.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Default")));

        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IActivityRepository, ActivityRepository>();

        services.AddSingleton<IEVMCalculator, EVMCalculationService>();

        return services;
    }
}
