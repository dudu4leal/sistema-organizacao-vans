namespace CaronaAlvinegra.Domain.Entities;

public class Grupo : AggregateRoot
{
    public string Nome { get; private set; } = string.Empty;

    // Navigation property
    private readonly List<Usuario> _membros = new();
    public IReadOnlyCollection<Usuario> Membros => _membros.AsReadOnly();

    // EF Core
    private Grupo() { }

    public Grupo(string nome)
    {
        Nome = nome;
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
