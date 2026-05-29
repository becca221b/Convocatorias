using Convocatorias.Application.DependencyInjection;
using Convocatorias.Infraestructure.DependencyInjection;
using Convocatorias.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// ─── Capas de la aplicación ──────────────────────────────────────────────────
// Program.cs no sabe nada de PostgreSQL, S3, ni de ningún use case concreto.
// Solo sabe que existen dos capas y les pasa la configuración.
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Migraciones automáticas solo en desarrollo.
    // En producción: usar un job de migración o el comando CLI.
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ConvocatoriasDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
