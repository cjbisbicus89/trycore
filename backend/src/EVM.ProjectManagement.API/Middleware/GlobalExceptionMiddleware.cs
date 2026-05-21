namespace EVM.ProjectManagement.API.Middleware;

using System.Net;
using System.Text.Json;
using EVM.ProjectManagement.Application.Common.Exceptions;
using EVM.ProjectManagement.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CA1848 // Use LoggerMessage delegates for performance

public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<GlobalExceptionMiddleware> logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await this.next(context);
        }
        catch (NotFoundException ex)
        {
            this.logger.LogWarning(ex, "Resource not found: {Message}", ex.Message);
            await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (DomainException ex)
        {
            this.logger.LogWarning(ex, "Domain exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (ValidationException ex)
        {
            this.logger.LogWarning(ex, "Validation exception: {Message}", ex.Message);
            var errors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            await HandleValidationExceptionAsync(context, errors);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error: {Message}", ex.Message);
            await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred");
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";
        var response = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = statusCode.ToString(),
            Detail = message,
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static async Task HandleValidationExceptionAsync(HttpContext context, Dictionary<string, string[]> errors)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.ContentType = "application/json";
        var response = new
        {
            Status = (int)HttpStatusCode.BadRequest,
            Title = "Validation Error",
            Errors = errors,
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}

#pragma warning restore CA1848
