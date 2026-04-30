using CaronaAlvinegra.Application.DTOs;
using CaronaAlvinegra.Application.Services;

namespace CaronaAlvinegra.Api.Endpoints;

public static class UsuarioEndpoints
{
    public static void MapUsuarioEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/usuarios").WithTags("Usuários");

        group.MapPost("/", async (UsuarioRequest request, UsuarioAppService service, CancellationToken ct) =>
        {
            var result = await service.CriarAsync(request, ct);
            return Results.Created($"/api/usuarios/{result.Id}", result);
        })
        .WithName("CriarUsuario")
        .WithOpenApi();

        group.MapGet("/", async (UsuarioAppService service, CancellationToken ct) =>
        {
            var result = await service.ListarAsync(ct);
            return Results.Ok(result);
        })
        .WithName("ListarUsuarios")
        .WithOpenApi();

        group.MapGet("/{id:guid}", async (Guid id, UsuarioAppService service, CancellationToken ct) =>
        {
            var result = await service.ObterPorIdAsync(id, ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("ObterUsuario")
        .WithOpenApi();

        group.MapPut("/{id:guid}", async (Guid id, UpdateUsuarioRequest request, UsuarioAppService service, CancellationToken ct) =>
        {
            var result = await service.AtualizarAsync(id, request, ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("AtualizarUsuario")
        .WithOpenApi();

        group.MapDelete("/{id:guid}", async (Guid id, UsuarioAppService service, CancellationToken ct) =>
        {
            var result = await service.RemoverAsync(id, ct);
            return result ? Results.NoContent() : Results.NotFound();
        })
        .WithName("RemoverUsuario")
        .WithOpenApi();
    }
}
