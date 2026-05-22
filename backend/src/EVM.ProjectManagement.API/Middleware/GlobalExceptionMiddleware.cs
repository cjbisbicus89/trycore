namespace EVM.ProjectManagement.API.Middleware;

using System.Net;
using System.Text.Json;
using EVM.ProjectManagement.Application.Common.Exceptions;
using EVM.ProjectManagement.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

public sealed class GlobalExceptionMiddleware
{
    private const string ApplicationJsonContentType = "application/json";
    private const string HttpStatusUrlTemplate = "https://httpstatuses.com/{0}";
    private const string ValidationErrorTitle = "Error de Validación";
    private const string Rfc7231ValidationUrl = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
    private const string UnexpectedErrorMessage = "Ocurrió un error inesperado";

    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Resource not found: {Message}", ex.Message);
            await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation exception: {Message}", ex.Message);
            var errors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            await HandleValidationExceptionAsync(context, errors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error: {Message}", ex.Message);
            await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, UnexpectedErrorMessage);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = ApplicationJsonContentType;

        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = statusCode.ToString(),
            Detail = message,
            Type = string.Format(HttpStatusUrlTemplate, (int)statusCode),
            Instance = context.Request.Path,
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
    }

    private static async Task HandleValidationExceptionAsync(HttpContext context, Dictionary<string, string[]> errors)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.ContentType = ApplicationJsonContentType;

        var problemDetails = new HttpValidationProblemDetails(errors)
        {
            Status = (int)HttpStatusCode.BadRequest,
            Title = ValidationErrorTitle,
            Type = Rfc7231ValidationUrl,
            Instance = context.Request.Path,
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
    }
}
