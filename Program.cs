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

await InicializarBancoDeDadosAsync(app.Services);

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

static async Task InicializarBancoDeDadosAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseInitialization");

const int maxTentativas = 10;
var delay = TimeSpan.FromSeconds(5);

for (var tentativa = 1; tentativa <= maxTentativas; tentativa++)
{
    try
    {
        dbContext.Database.Migrate();
        logger.LogInformation("Banco de dados inicializado com migrations.");

        return;
    }
    catch (Exception ex) when (tentativa < maxTentativas)
    {
        logger.LogWarning(ex, "Tentativa {Tentativa}/{MaxTentativas} de inicializar o banco falhou. Nova tentativa em {Delay}s.", tentativa, maxTentativas, delay.TotalSeconds);
        await Task.Delay(delay);
    }
}

throw new InvalidOperationException("Não foi possível inicializar o banco de dados após múltiplas tentativas.");
}
