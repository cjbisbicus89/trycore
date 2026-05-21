using EVM.ProjectManagement.Domain.Repositories;
using EVM.ProjectManagement.Domain.Services;
using EVM.ProjectManagement.Infrastructure.Persistence;
using EVM.ProjectManagement.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EVM.ProjectManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Registrar DbContext con PostgreSQL
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Registrar repositorios
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IActivityRepository, ActivityRepository>();

        // Registrar servicios de dominio
        services.AddSingleton<IEVMCalculator, EVMCalculationService>();

        return services;
    }
}
