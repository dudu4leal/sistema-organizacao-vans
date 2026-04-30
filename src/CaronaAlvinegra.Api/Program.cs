using CaronaAlvinegra.Api.Endpoints;
using CaronaAlvinegra.Api.Middleware;
using CaronaAlvinegra.Application.Mapping;
using CaronaAlvinegra.Application.Services;
using CaronaAlvinegra.Application.Validators;
using CaronaAlvinegra.Domain.Interfaces;
using CaronaAlvinegra.Domain.Services;
using CaronaAlvinegra.Infrastructure.Data;
using CaronaAlvinegra.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;

// ──────────────────────────────────────────────
//  CONFIGURAÇÃO
// ──────────────────────────────────────────────

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((ctx, cfg) =>
    cfg.ReadFrom.Configuration(ctx.Configuration));

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro adicional para que repositorios que dependem de DbContext (base)
// possam resolver AppDbContext corretamente via DI
builder.Services.AddScoped<DbContext>(sp => sp.GetRequiredService<AppDbContext>());

// Domain Services
builder.Services.AddScoped<ValidadorDeIntegridade>();
builder.Services.AddScoped<AlocadorService>();

// Application Services
builder.Services.AddScoped<UsuarioAppService>();
builder.Services.AddScoped<GrupoAppService>();
builder.Services.AddScoped<RotaAppService>();
builder.Services.AddScoped<JogoAppService>();
builder.Services.AddScoped<PresencaAppService>();

// Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IGrupoRepository, GrupoRepository>();
builder.Services.AddScoped<IJogoRepository, JogoRepository>();
builder.Services.AddScoped<IPassageiroRepository, PassageiroRepository>();
builder.Services.AddScoped<IRotaRepository, RotaRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(DomainToDtoProfile));

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<UsuarioValidator>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Carona Alvinegra API", Version = "v1" });
});

// CORS (para WebView2)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ──────────────────────────────────────────────
//  MIDDLEWARE PIPELINE
// ──────────────────────────────────────────────

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSerilogRequestLogging();
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ──────────────────────────────────────────────
//  DATA SEED (DEV)
// ──────────────────────────────────────────────

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await context.Database.EnsureCreatedAsync();
    await DataSeed.SeedAsync(context);
}

// ──────────────────────────────────────────────
//  ENDPOINTS
// ──────────────────────────────────────────────

app.MapUsuarioEndpoints();
app.MapGrupoEndpoints();
app.MapRotaEndpoints();
app.MapJogoEndpoints();
app.MapPresencaEndpoints();

app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));

await app.RunAsync();
