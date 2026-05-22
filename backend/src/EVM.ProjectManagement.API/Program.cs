namespace EVM.ProjectManagement.API;

using EVM.ProjectManagement.API.Middleware;
using EVM.ProjectManagement.Application;
using EVM.ProjectManagement.Infrastructure;
using EVM.ProjectManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public sealed class Program
{
    private const string DefaultApiUrl = "http://localhost:5001";
    private const string SwaggerApiVersion = "v1";
    private const string SwaggerApiTitle = "API de Gestión EVM";

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder);
        var app = builder.Build();
        ConfigureMiddleware(app);
        RunMigrations(app);
        ConfigureSwagger(app);

        var url = builder.Configuration["ASPNETCORE_URLS"] ?? DefaultApiUrl;
        app.Run(url);
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
        });
    }

    private static void ConfigureMiddleware(WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionMiddleware>();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }

    private static void RunMigrations(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (dbContext.Database.IsRelational())
        {
            dbContext.Database.Migrate();
        }
    }

    private static void ConfigureSwagger(WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            return;
        }

        app.UseSwagger(c => c.RouteTemplate = "api-docs/{documentName}/swagger.json");
        app.UseSwaggerUI(c =>
        {
            c.RoutePrefix = "swagger-ui";
            c.SwaggerEndpoint("/api-docs/v1/swagger.json", SwaggerApiTitle);
        });
    }
}
