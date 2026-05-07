namespace CaronaAlvinegra.Domain.Entities;

public class Usuario : AggregateRoot
{
    public string Nome { get; private set; } = string.Empty;
    public string? Telefone { get; private set; }
    public Guid? RotaPreferencialId { get; private set; }
    public Guid? GrupoId { get; private set; }

    // Navigation properties (EF Core)
    public Rota? RotaPreferencial { get; private set; }
    public Grupo? Grupo { get; private set; }

    // EF Core
    private Usuario() { }

    public Usuario(string nome, Guid? rotaPreferencialId, string? telefone = null, Guid? grupoId = null)
    {
        Nome = nome;
        Telefone = telefone;
        RotaPreferencialId = rotaPreferencialId;
        GrupoId = grupoId;
    }

    public void AlterarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome do usuário não pode ser vazio.");

        Nome = nome;
        AtualizadoEm = DateTime.UtcNow;
    }

    public void AlterarTelefone(string? telefone)
    {
        Telefone = telefone;
        AtualizadoEm = DateTime.UtcNow;
    }

    public void AlterarRotaPreferencial(Guid? novaRotaId)
    {
        RotaPreferencialId = novaRotaId;
        AtualizadoEm = DateTime.UtcNow;
    }

    public void VincularAoGrupo(Grupo grupo)
    {
        GrupoId = grupo.Id;
        AtualizadoEm = DateTime.UtcNow;
    }

    public void DesvincularDoGrupo()
    {
        GrupoId = null;
        AtualizadoEm = DateTime.UtcNow;
    }
}
