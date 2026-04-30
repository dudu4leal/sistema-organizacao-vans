using CaronaAlvinegra.Application.DTOs;
using CaronaAlvinegra.Application.Services;

namespace CaronaAlvinegra.Api.Endpoints;

public static class RotaEndpoints
{
    public static void MapRotaEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/rotas").WithTags("Rotas");

        group.MapPost("/", async (RotaRequest request, RotaAppService service, CancellationToken ct) =>
        {
            var result = await service.CriarAsync(request, ct);
            return Results.Created($"/api/rotas/{result.Id}", result);
        })
        .WithName("CriarRota")
        .WithOpenApi();

        group.MapGet("/", async (RotaAppService service, CancellationToken ct) =>
        {
            var result = await service.ListarAsync(ct);
            return Results.Ok(result);
        })
        .WithName("ListarRotas")
        .WithOpenApi();

        group.MapGet("/{id:guid}", async (Guid id, RotaAppService service, CancellationToken ct) =>
        {
            var result = await service.ObterPorIdAsync(id, ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("ObterRota")
        .WithOpenApi();

        group.MapDelete("/{id:guid}", async (Guid id, RotaAppService service, CancellationToken ct) =>
        {
            var result = await service.RemoverAsync(id, ct);
            return result ? Results.NoContent() : Results.NotFound();
        })
        .WithName("RemoverRota")
        .WithOpenApi();
    }
}
