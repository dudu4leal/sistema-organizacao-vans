namespace CaronaAlvinegra.Application.DTOs;

public record GrupoRequest(string Nome);

public class GrupoResponse
{
    public Guid Id { get; init; }
    public string Nome { get; init; } = string.Empty;
    public int TotalMembros { get; init; }
}
