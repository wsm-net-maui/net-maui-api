using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace net_maui_api.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class LogExecutionTimeAttribute : ActionFilterAttribute
{
    private const string StopwatchKey = "Stopwatch";

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        context.HttpContext.Items[StopwatchKey] = stopwatch;
        base.OnActionExecuting(context);
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.HttpContext.Items[StopwatchKey] is Stopwatch stopwatch)
        {
            stopwatch.Stop();
            var executionTime = stopwatch.ElapsedMilliseconds;

            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<LogExecutionTimeAttribute>>();

            var actionName = context.ActionDescriptor.DisplayName;
            logger.LogInformation(
                "Action {ActionName} executada em {ExecutionTime}ms",
                actionName,
                executionTime);
        }

        base.OnActionExecuted(context);
    }
}
