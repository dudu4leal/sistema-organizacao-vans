namespace CaronaAlvinegra.Domain.Entities;

public abstract class AggregateRoot
{
    public Guid Id { get; protected set; }
    public DateTime CriadoEm { get; protected set; }
    public DateTime? AtualizadoEm { get; protected set; }

    protected AggregateRoot()
    {
        Id = Guid.NewGuid();
        CriadoEm = DateTime.UtcNow;
    }
}
