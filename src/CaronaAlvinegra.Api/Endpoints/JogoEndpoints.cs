using CaronaAlvinegra.Application.DTOs;
using CaronaAlvinegra.Application.Services;

namespace CaronaAlvinegra.Api.Endpoints;

public static class JogoEndpoints
{
    public static void MapJogoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/jogos").WithTags("Jogos");

        group.MapPost("/", async (JogoRequest request, JogoAppService service, CancellationToken ct) =>
        {
            var result = await service.CriarAsync(request, ct);
            return Results.Created($"/api/jogos/{result.Id}", result);
        })
        .WithName("CriarJogo")
        .WithOpenApi();

        group.MapGet("/", async (JogoAppService service, CancellationToken ct) =>
        {
            var result = await service.ListarAsync(ct);
            return Results.Ok(result);
        })
        .WithName("ListarJogos")
        .WithOpenApi();

        group.MapGet("/{id:guid}", async (Guid id, JogoAppService service, CancellationToken ct) =>
        {
            var result = await service.ObterPorIdAsync(id, ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("ObterJogo")
        .WithOpenApi();

        group.MapDelete("/{id:guid}", async (Guid id, JogoAppService service, CancellationToken ct) =>
        {
            var result = await service.RemoverAsync(id, ct);
            return result ? Results.NoContent() : Results.NotFound();
        })
        .WithName("RemoverJogo")
        .WithOpenApi();

        // ── Alocação ──────────────────────────────────
        group.MapPost("/{id:guid}/alocar", async (Guid id, JogoAppService service, CancellationToken ct) =>
        {
            var result = await service.AlocarJogoAsync(id, ct);
            return result.Sucesso ? Results.Ok(result) : Results.BadRequest(result);
        })
        .WithName("AlocarJogo")
        .WithOpenApi();

        // Endpoint GET para consultar o resultado da alocação (sem re-executar)
        // Como a alocação é calculada em memória, ela é recalculada, mas semanticamente
        // é uma operação de leitura (GET), adequada para "Ver Resultado"
        group.MapGet("/{id:guid}/alocar", async (Guid id, JogoAppService service, CancellationToken ct) =>
        {
            var result = await service.AlocarJogoAsync(id, ct);
            return result.Sucesso ? Results.Ok(result) : Results.BadRequest(result);
        })
        .WithName("ObterAlocacao")
        .WithOpenApi();

        // ── Definição manual de líder ──────────────────────
        group.MapPost("/{jogoId:guid}/veiculos/{veiculoOrdem:int}/lider/{passageiroUsuarioId:guid}",
            async (Guid jogoId, int veiculoOrdem, Guid passageiroUsuarioId, JogoAppService service, CancellationToken ct) =>
        {
            var result = await service.DefinirLiderAsync(jogoId, veiculoOrdem, passageiroUsuarioId, ct);
            return result.Sucesso ? Results.Ok(result) : Results.BadRequest(result);
        })
        .WithName("DefinirLiderVeiculo")
        .WithOpenApi();
    }
}
