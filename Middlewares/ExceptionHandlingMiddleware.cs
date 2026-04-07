using System.Net;
using System.Text.Json;
using Wsm.Aplication.DTOs.Common;

namespace net_maui_api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Gerar Correlation ID para rastreamento da requisição
        var correlationId = Guid.NewGuid().ToString();
        context.Items["CorrelationId"] = correlationId;
        context.Response.Headers.Append("X-Correlation-ID", correlationId);

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, correlationId);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception, string correlationId)
    {
        // Log estruturado com Correlation ID e contexto adicional para rastreamento
        _logger.LogError(
            exception,
            "Erro não tratado: {ExceptionType} | CorrelationId: {CorrelationId} | Path: {RequestPath} | Method: {RequestMethod} | User: {UserName}",
            exception.GetType().Name,
            correlationId,
            context.Request.Path,
            context.Request.Method,
            context.User?.Identity?.Name ?? "Anonymous");

        // Determina se deve mostrar detalhes da exceção baseado no ambiente
        var isDevelopment = _environment.IsDevelopment();

        var (statusCode, response) = exception switch
        {
            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                CreateResponse("Acesso não autorizado", exception, isDevelopment, correlationId)
            ),
            KeyNotFoundException => (
                HttpStatusCode.NotFound,
                CreateResponse("Recurso não encontrado", exception, isDevelopment, correlationId)
            ),
            ArgumentException or InvalidOperationException => (
                HttpStatusCode.BadRequest,
                CreateResponse("Requisição inválida", exception, isDevelopment, correlationId)
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                CreateResponse("Erro interno do servidor", exception, isDevelopment, correlationId)
            )
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(json);
    }

    private static ApiResponse<object> CreateResponse(
        string message,
        Exception exception,
        bool isDevelopment,
        string correlationId)
    {
        var errorDetails = new List<string>();

        if (isDevelopment)
        {
            // Em desenvolvimento: mostra detalhes completos da exceção
            errorDetails.Add($"Tipo: {exception.GetType().Name}");
            errorDetails.Add($"Mensagem: {exception.Message}");

            if (exception.InnerException != null)
            {
                errorDetails.Add($"Inner Exception: {exception.InnerException.Message}");
            }

            errorDetails.Add($"StackTrace: {exception.StackTrace}");
        }
        else
        {
            // Em produção: apenas mensagem genérica e Correlation ID para suporte
            errorDetails.Add(exception.Message);
            errorDetails.Add($"Para suporte, informe o Correlation ID: {correlationId}");
        }

        return ApiResponse<object>.Failure(message, errorDetails);
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
