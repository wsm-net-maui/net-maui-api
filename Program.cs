using Microsoft.EntityFrameworkCore;
using net_maui_api.Configuration;
using net_maui_api.Middlewares;
using Serilog;
using Serilog.Context;
using Wsm.Infra.Estrutura.Data;

// Configurar Serilog para logging inicial (bootstrap)
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Iniciando aplicação Controle de Estoque API");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Configurar Serilog como provedor de logging principal
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithThreadId()
        .Enrich.WithProperty("Application", "ControleEstoqueAPI")
        .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName));
      

builder.Services.AddAuthorization();

var host = builder.Configuration["DBHOST"] ?? "localhost";
var port = builder.Configuration["DBPORT"] ?? "3306";
var password = builder.Configuration["DBPASSWORD"] ?? "numSey";
string mySqlConnection = $"server={host};port={port};user=root;password={password};database=controle_estoque";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.ResolveDependencies();
builder.Services.AddAuthorizationConfig(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandlingMiddleware();

app.Use(async (context, next) =>
{
    var correlationId = context.Items["CorrelationId"]?.ToString() ?? Guid.NewGuid().ToString();
    var userName = context.User?.Identity?.Name ?? "Anonymous";
    var requestPath = context.Request.Path;

    using (LogContext.PushProperty("CorrelationId", correlationId))
    using (LogContext.PushProperty("UserName", userName))
    using (LogContext.PushProperty("RequestPath", requestPath))
    {
        await next();
    }
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.UseHttpsRedirection();

app.UseIdentityConfiguration();

app.MapControllers();

Log.Information("Aplicação iniciada com sucesso");

app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Aplicação falhou ao iniciar");
    throw;
}
finally
{
    Log.Information("Encerrando aplicação");
    Log.CloseAndFlush();
}
