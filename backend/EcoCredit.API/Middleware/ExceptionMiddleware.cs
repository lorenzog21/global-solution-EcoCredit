using System.Net;
using System.Text.Json;

namespace EcoCredit.API.Middleware;

public class ExceptionMiddleware {
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger) {
        _next   = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context) {
        try {
            await _next(context);
        } catch (Exception ex) {
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await WriteErrorAsync(context, ex);
        }
    }

    private static Task WriteErrorAsync(HttpContext context, Exception ex) {
        var (status, message) = ex switch {
            KeyNotFoundException => (HttpStatusCode.NotFound,            "Recurso não encontrado."),
            UnauthorizedAccessException => (HttpStatusCode.Forbidden,    "Acesso negado."),
            ArgumentException    => (HttpStatusCode.BadRequest,          ex.Message),
            _                    => (HttpStatusCode.InternalServerError, "Erro interno do servidor.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode  = (int)status;

        var body = JsonSerializer.Serialize(new { error = message, status = (int)status });
        return context.Response.WriteAsync(body);
    }
}

public static class ExceptionMiddlewareExtensions {
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        => app.UseMiddleware<ExceptionMiddleware>();
}
