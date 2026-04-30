namespace CaronaAlvinegra.Domain.Entities;

public class Rota : AggregateRoot
{
    public string Nome { get; private set; } = string.Empty;
    public string LocalEmbarque { get; private set; } = string.Empty;

    // EF Core
    private Rota() { }

    public Rota(string nome, string localEmbarque)
    {
        Nome = nome;
        LocalEmbarque = localEmbarque;
    }
}
