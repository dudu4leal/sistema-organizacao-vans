namespace CaronaAlvinegra.Application.DTOs;

public record GrupoRequest(string Nome, Guid RotaId);

public class GrupoResponse
{
    public Guid Id { get; init; }
    public string Nome { get; init; } = string.Empty;
    public Guid RotaId { get; init; }
    public string? RotaNome { get; init; }
    public int TotalMembros { get; init; }
}
