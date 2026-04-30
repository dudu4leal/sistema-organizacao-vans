namespace CaronaAlvinegra.Application.DTOs;

public record RotaRequest(
    string Nome,
    string LocalEmbarque);

public class RotaResponse
{
    public Guid Id { get; init; }
    public string Nome { get; init; } = string.Empty;
    public string LocalEmbarque { get; init; } = string.Empty;
}
