namespace CaronaAlvinegra.Domain.Entities;

/// <summary>
/// Entidade associativa entre Veiculo e Passageiro.
/// </summary>
public class Alocacao : AggregateRoot
{
    public Guid VeiculoId { get; private set; }
    public Guid PassageiroId { get; private set; }
    public bool IsLider { get; private set; }

    // Navigation properties (EF Core)
    public Veiculo? Veiculo { get; private set; }
    public Passageiro? Passageiro { get; private set; }

    // EF Core
    private Alocacao() { }

    public Alocacao(Guid veiculoId, Guid passageiroId, bool isLider = false)
    {
        VeiculoId = veiculoId;
        PassageiroId = passageiroId;
        IsLider = isLider;
    }

    /// <summary>
    /// Construtor usado pelo algoritmo de alocação em memória.
    /// Mantém a referência do Passageiro para navegação sem banco.
    /// </summary>
    public Alocacao(Guid veiculoId, Passageiro passageiro, bool isLider = false)
    {
        VeiculoId = veiculoId;
        PassageiroId = passageiro.Id;
        Passageiro = passageiro;
        IsLider = isLider;
    }

    public void DefinirComoLider()
    {
        IsLider = true;
    }

    public void RemoverLideranca()
    {
        IsLider = false;
    }
}
