using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Wsm.Infra.Core.Logging;

/// <summary>
/// Enricher customizado para adicionar informações de requisição HTTP aos logs
/// </summary>
public class HttpContextEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextEnricher(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
            return;

        // Correlation ID
        if (httpContext.Items.TryGetValue("CorrelationId", out var correlationId))
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                "CorrelationId", correlationId));
        }

        // User Identity
        if (httpContext.User?.Identity?.IsAuthenticated == true)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                "UserName", httpContext.User.Identity.Name));
        }

        // Request Path
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
            "RequestPath", httpContext.Request.Path));

        // Request Method
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
            "RequestMethod", httpContext.Request.Method));

        // Client IP
        var clientIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
            "ClientIp", clientIp));

        // User Agent
        if (httpContext.Request.Headers.TryGetValue("User-Agent", out var userAgent))
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                "UserAgent", userAgent.ToString()));
        }
    }
}

/// <summary>
/// Enricher para adicionar informações de performance
/// </summary>
public class PerformanceEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        // Adicionar timestamp de alta precisão
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
            "TimestampUtc", DateTimeOffset.UtcNow));

        // Adicionar informações de processo
        var process = System.Diagnostics.Process.GetCurrentProcess();
        
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
            "ProcessId", process.Id));

        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
            "WorkingSetMB", Math.Round(process.WorkingSet64 / 1024.0 / 1024.0, 2)));
    }
}
