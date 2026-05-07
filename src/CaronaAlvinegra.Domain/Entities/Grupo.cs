namespace CaronaAlvinegra.Domain.Entities;

public class Grupo : AggregateRoot
{
    public string Nome { get; private set; } = string.Empty;
    public Guid RotaId { get; private set; }

    // Navigation properties
    public Rota? Rota { get; private set; }
    private readonly List<Usuario> _membros = new();
    public IReadOnlyCollection<Usuario> Membros => _membros.AsReadOnly();

    // EF Core
    private Grupo() { }

    public Grupo(string nome, Guid rotaId)
    {
        Nome = nome;
        RotaId = rotaId;
    }

    public void AdicionarMembro(Usuario usuario)
    {
        if (_membros.Any(m => m.Id == usuario.Id))
            throw new DomainException($"Usuário {usuario.Nome} já é membro do grupo {Nome}.");

        _membros.Add(usuario);
    }

    public int ObterTamanhoNoJogo(IEnumerable<Guid> idsPresentesNoJogo)
    {
        return _membros.Count(m => idsPresentesNoJogo.Contains(m.Id));
    }
}
