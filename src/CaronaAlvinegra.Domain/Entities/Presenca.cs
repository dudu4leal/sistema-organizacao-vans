namespace CaronaAlvinegra.Domain.Entities;

public class Presenca : AggregateRoot
{
    public Guid UsuarioId { get; private set; }
    public Guid JogoId { get; private set; }
    public Guid RotaEfetivaId { get; private set; }
    public DateTime ConfirmadoEm { get; private set; }

    // Navigation properties (EF Core)
    public Usuario? Usuario { get; private set; }
    public Jogo? Jogo { get; private set; }
    public Rota? RotaEfetiva { get; private set; }

    // EF Core
    private Presenca() { }

    public Presenca(Guid usuarioId, Guid jogoId, Guid rotaEfetivaId)
    {
        UsuarioId = usuarioId;
        JogoId = jogoId;
        RotaEfetivaId = rotaEfetivaId;
        ConfirmadoEm = DateTime.UtcNow;
    }
}
