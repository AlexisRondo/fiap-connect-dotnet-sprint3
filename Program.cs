using Serilog;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

// ── Configurar Serilog ────────────────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/fiapconnect-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Iniciando FIAP Connect API...");

    var builder = WebApplication.CreateBuilder(args);

    // ── Substituir logging padrão pelo Serilog ────────────────────────────────
    builder.Host.UseSerilog();

    // ── Controllers e Swagger ─────────────────────────────────────────────────
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "FIAP Connect API",
            Version = "v1",
            Description = "API REST para formação de grupos acadêmicos - Sprint 3"
        }));

    // ── Health Checks ─────────────────────────────────────────────────────────
    builder.Services.AddHealthChecks()
        .AddCheck("api", () =>
            Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy("API funcionando normalmente"))
        .AddCheck("memoria", () =>
        {
            var memoriaUsada = GC.GetTotalMemory(false) / 1024 / 1024;
            return memoriaUsada < 500
                ? Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy($"Memória OK: {memoriaUsada}MB")
                : Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Degraded($"Memória alta: {memoriaUsada}MB");
        });

    // ── OpenTelemetry Tracing ─────────────────────────────────────────────────
    builder.Services.AddOpenTelemetry()
        .WithTracing(tracing => tracing
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("FiapConnect"))
            .AddAspNetCoreInstrumentation()
            .AddConsoleExporter());

    var app = builder.Build();

    // ── Middleware ────────────────────────────────────────────────────────────
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FIAP Connect API v1"));

    // Log de todas as requisições
    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    // ── Endpoint de Health Check ──────────────────────────────────────────────
    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    app.MapHealthChecks("/health/simple", new HealthCheckOptions
    {
        ResponseWriter = async (context, report) =>
        {
            context.Response.ContentType = "application/json";
            var resultado = new
            {
                status = report.Status.ToString(),
                timestamp = DateTime.UtcNow,
                checks = report.Entries.Select(e => new
                {
                    nome = e.Key,
                    status = e.Value.Status.ToString(),
                    descricao = e.Value.Description
                })
            };
            await context.Response.WriteAsJsonAsync(resultado);
        }
    });

    Log.Information("FIAP Connect API iniciada com sucesso.");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Erro fatal ao iniciar a aplicação.");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }
