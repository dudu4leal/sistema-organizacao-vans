using CaronaAlvinegra.Application.DTOs;
using CaronaAlvinegra.Application.Services;

namespace CaronaAlvinegra.Api.Endpoints;

public static class PresencaEndpoints
{
    public static void MapPresencaEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/jogos/{jogoId:guid}/presencas").WithTags("Presenças");

        group.MapPost("/", async (Guid jogoId, MarcarPresencaRequest request, PresencaAppService service, CancellationToken ct) =>
        {
            var result = await service.MarcarPresencaAsync(jogoId, request, ct);
            return result is null ? Results.NotFound() : Results.Created($"/api/jogos/{jogoId}/presencas", result);
        })
        .WithName("MarcarPresenca")
        .WithOpenApi();

        group.MapGet("/", async (Guid jogoId, PresencaAppService service, CancellationToken ct) =>
        {
            var result = await service.ListarPresencasDoJogoAsync(jogoId, ct);
            return Results.Ok(result);
        })
        .WithName("ListarPresencas")
        .WithOpenApi();

        group.MapDelete("/{presencaId:guid}", async (Guid presencaId, PresencaAppService service, CancellationToken ct) =>
        {
            var result = await service.RemoverPresencaAsync(presencaId, ct);
            return result ? Results.NoContent() : Results.NotFound();
        })
        .WithName("RemoverPresenca")
        .WithOpenApi();
    }
}
