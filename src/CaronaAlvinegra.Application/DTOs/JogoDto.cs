namespace CaronaAlvinegra.Application.DTOs;

public record JogoRequest(
    string Adversario,
    DateTime Data,
    string Local);

public class JogoResponse
{
    public Guid Id { get; set; }
    public string Adversario { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public string Local { get; set; } = string.Empty;
    public int TotalPresentes { get; set; }
    public bool AlocacaoRealizada { get; set; }
}
