using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GrowSphere.Infrastructure.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(e.Message);
        }
    }
}