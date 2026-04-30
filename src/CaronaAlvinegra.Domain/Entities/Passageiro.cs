namespace CaronaAlvinegra.Domain.Entities;

/// <summary>
/// Representa um Usuario em um contexto específico de Jogo.
/// Criado automaticamente a partir de uma Presenca.
/// </summary>
public class Passageiro : AggregateRoot
{
    public Guid UsuarioId { get; private set; }
    public Guid PresencaId { get; private set; }
    public Guid JogoId { get; private set; }
    public Guid RotaId { get; private set; }
    public string Nome { get; private set; } = string.Empty;

    // Navigation properties (EF Core)
    public Usuario? Usuario { get; private set; }
    public Presenca? Presenca { get; private set; }
    public Jogo? Jogo { get; private set; }
    public Rota? Rota { get; private set; }

    // EF Core
    private Passageiro() { }

    public Passageiro(Guid usuarioId, Guid presencaId, Guid jogoId, Guid rotaId, string nome)
    {
        UsuarioId = usuarioId;
        PresencaId = presencaId;
        JogoId = jogoId;
        RotaId = rotaId;
        Nome = nome;
    }
}
