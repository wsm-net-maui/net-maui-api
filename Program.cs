using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Context;
using net_maui_api.Middlewares;
using Wsm.Aplication.Interfaces;
using Wsm.Aplication.Services;
using Wsm.Domain.Interfaces;
using Wsm.Domain.Interfaces.Repositories;
using Wsm.Infra.Core.Interfaces;
using Wsm.Infra.Core.Services;
using Wsm.Infra.Core.Settings;
using Wsm.Infra.Estrutura.Data;
using Wsm.Infra.Estrutura.Repositories;

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

    // Configuração de JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettings);

var secretKey = jwtSettings.Get<JwtSettings>()?.SecretKey ?? throw new InvalidOperationException("JWT SecretKey não configurada");

// Configuração de Autenticação
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Get<JwtSettings>()?.Issuer,
        ValidAudience = jwtSettings.Get<JwtSettings>()?.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddAuthorization();

var host = builder.Configuration["DBHOST"] ?? "localhost";
var port = builder.Configuration["DBPORT"] ?? "3306";
var password = builder.Configuration["DBPASSWORD"] ?? "numSey";
string mySqlConnection = $"server={host};port={port};user=root;password={password};database=controle_estoque";

// Configuração do DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));

// Repositórios
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IMovimentacaoEstoqueRepository, MovimentacaoEstoqueRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IMovimentacaoEstoqueService, MovimentacaoEstoqueService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();

// Infraestrutura
builder.Services.AddScoped<ITokenService, TokenService>();

// Controllers
builder.Services.AddControllers();

// OpenAPI/Swagger com suporte a JWT Bearer
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "API Controle de Estoque", Version = "v1" });
});

var app = builder.Build();

// Middleware de Exceções (deve ser o primeiro)
app.UseExceptionHandlingMiddleware();

// Middleware para enriquecer logs com contexto de requisição
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

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirecionar raiz para Swagger
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

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
