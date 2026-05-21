namespace EVM.ProjectManagement.API.Middleware;

public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await this.next(context);
        }
        catch
        {
            throw;
        }
    }
}
