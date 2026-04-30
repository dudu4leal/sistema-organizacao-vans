namespace CaronaAlvinegra.Domain.ValueObjects;

/// <summary>
/// Value Object que representa a lotação de um veículo.
/// Imutável e auto-validável.
/// </summary>
public record Lotacao
{
    public int Ocupados { get; init; }
    public int Capacidade { get; init; }
    public int VagasRestantes => Capacidade - Ocupados;

    public Lotacao(int ocupados, int capacidade)
    {
        if (ocupados < 0)
            throw new ArgumentException("Ocupados não pode ser negativo.", nameof(ocupados));
        if (capacidade <= 0)
            throw new ArgumentException("Capacidade deve ser maior que zero.", nameof(capacidade));
        if (ocupados > capacidade)
            throw new ArgumentException("Ocupados não pode exceder a capacidade.", nameof(ocupados));

        Ocupados = ocupados;
        Capacidade = capacidade;
    }

    public bool EstaCompleta => Ocupados >= Capacidade;
    public bool EstaVazia => Ocupados == 0;

    public override string ToString() => $"{Ocupados}/{Capacidade}";
}
