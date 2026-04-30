using CaronaAlvinegra.Application.DTOs;
using CaronaAlvinegra.Application.Services;

namespace CaronaAlvinegra.Api.Endpoints;

public static class GrupoEndpoints
{
    public static void MapGrupoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/grupos").WithTags("Grupos");

        group.MapPost("/", async (GrupoRequest request, GrupoAppService service, CancellationToken ct) =>
        {
            var result = await service.CriarAsync(request, ct);
            return Results.Created($"/api/grupos/{result.Id}", result);
        })
        .WithName("CriarGrupo")
        .WithOpenApi();

        group.MapGet("/", async (GrupoAppService service, CancellationToken ct) =>
        {
            var result = await service.ListarAsync(ct);
            return Results.Ok(result);
        })
        .WithName("ListarGrupos")
        .WithOpenApi();

        group.MapGet("/{id:guid}", async (Guid id, GrupoAppService service, CancellationToken ct) =>
        {
            var result = await service.ObterPorIdAsync(id, ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("ObterGrupo")
        .WithOpenApi();

        group.MapPost("/{id:guid}/membros/{usuarioId:guid}", async (Guid id, Guid usuarioId, GrupoAppService service, CancellationToken ct) =>
        {
            var result = await service.AdicionarMembroAsync(id, usuarioId, ct);
            return result ? Results.NoContent() : Results.NotFound();
        })
        .WithName("AdicionarMembro")
        .WithOpenApi();

        group.MapDelete("/{id:guid}", async (Guid id, GrupoAppService service, CancellationToken ct) =>
        {
            var result = await service.RemoverAsync(id, ct);
            return result ? Results.NoContent() : Results.NotFound();
        })
        .WithName("RemoverGrupo")
        .WithOpenApi();
    }
}
