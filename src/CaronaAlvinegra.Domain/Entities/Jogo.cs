namespace CaronaAlvinegra.Domain.Entities;

public class Jogo : AggregateRoot
{
    public string Adversario { get; private set; } = string.Empty;
    public DateTime Data { get; private set; }
    public string Local { get; private set; } = string.Empty;

    // EF Core
    private Jogo() { }

    public Jogo(string adversario, DateTime data, string local)
    {
        Adversario = adversario;
        Data = data;
        Local = local;
    }
}
