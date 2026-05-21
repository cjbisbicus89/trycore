namespace EVM.ProjectManagement.API;

using EVM.ProjectManagement.API.Middleware;
using EVM.ProjectManagement.Application;
using EVM.ProjectManagement.Infrastructure;
using EVM.ProjectManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public sealed class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Apply database migrations
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate();
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(c => c.RouteTemplate = "api-docs/{documentName}/swagger.json");
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger-ui";
                c.SwaggerEndpoint("/api-docs/v1/swagger.json", "EVM API v1");
            });
        }

        app.UseMiddleware<GlobalExceptionMiddleware>();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        var url = app.Configuration["ASPNETCORE_URLS"] ?? "http://localhost:5001";
        app.Run(url);
    }
}
