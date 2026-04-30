namespace CaronaAlvinegra.Application.DTOs;

public record MarcarPresencaRequest(
    Guid UsuarioId,
    Guid RotaEfetivaId);

public class PresencaResponse
{
    public Guid Id { get; init; }
    public Guid UsuarioId { get; init; }
    public string? UsuarioNome { get; init; }
    public Guid JogoId { get; init; }
    public Guid RotaEfetivaId { get; init; }
    public string? RotaNome { get; init; }
    public DateTime ConfirmadoEm { get; init; }
}
