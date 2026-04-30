namespace CaronaAlvinegra.Application.DTOs;

public record UsuarioRequest(
    string Nome,
    string? Telefone,
    Guid RotaPreferencialId,
    Guid? GrupoId);

public class UsuarioResponse
{
    public Guid Id { get; init; }
    public string Nome { get; init; } = string.Empty;
    public string? Telefone { get; init; }
    public Guid RotaPreferencialId { get; init; }
    public string? RotaNome { get; init; }
    public Guid? GrupoId { get; init; }
    public string? GrupoNome { get; init; }
}

public record UpdateUsuarioRequest(
    string? Nome,
    string? Telefone,
    Guid? RotaPreferencialId,
    Guid? GrupoId);
