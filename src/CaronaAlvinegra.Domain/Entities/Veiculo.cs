using CaronaAlvinegra.Domain.Enums;

namespace CaronaAlvinegra.Domain.Entities;

/// <summary>
/// Veículo de transporte. A classificação (Van / DobloSpin / ListaEspera)
/// é definida dinamicamente na Fase 3 do AlocadorService.
/// Capacidade máxima: 15 passageiros.
/// </summary>
public class Veiculo : AggregateRoot
{
    private readonly List<Alocacao> _alocacoes = new();

    public const int CapacidadeMaxima = 15;
    public const int LotacaoMinimaVan = 11;
    public const int LotacaoMinimaDobloSpin = 5;

    public Guid RotaId { get; private set; }
    public Rota? Rota { get; private set; }
    public bool Fechado { get; private set; }
    public ETipoVeiculo Classificacao { get; private set; }
    public int Ordem { get; private set; }

    public IReadOnlyCollection<Alocacao> Alocacoes => _alocacoes.AsReadOnly();
    public int LotacaoAtual => _alocacoes.Count;
    public int VagasRestantes => CapacidadeMaxima - LotacaoAtual;

    // EF Core
    private Veiculo() { }

    public Veiculo(Guid rotaId, int ordem)
    {
        RotaId = rotaId;
        Ordem = ordem;
        Fechado = false;
        Classificacao = ETipoVeiculo.Van; // Classificação padrão, redefinida na Fase 3
    }

    public bool PodeAlocar(int quantidade)
    {
        return !Fechado && VagasRestantes >= quantidade;
    }

    public void AlocarPassageiro(Passageiro passageiro, bool isLider = false)
    {
        if (Fechado)
            throw new DomainException($"Veículo {Ordem} já está fechado.");

        if (!PodeAlocar(1))
            throw new DomainException($"Veículo {Ordem} não tem vagas disponíveis.");

        var alocacao = new Alocacao(Id, passageiro.Id, isLider);
        _alocacoes.Add(alocacao);
        AtualizadoEm = DateTime.UtcNow;
    }

    public void Fechar()
    {
        Fechado = true;
        AtualizadoEm = DateTime.UtcNow;
    }

    /// <summary>
    /// Classifica o veículo conforme sua lotação final (Fase 3 do algoritmo).
    /// </summary>
    public ETipoVeiculo Classificar()
    {
        if (LotacaoAtual >= LotacaoMinimaVan)      // 11 a 15
            Classificacao = ETipoVeiculo.Van;
        else if (LotacaoAtual >= LotacaoMinimaDobloSpin) // 5 a 7
            Classificacao = ETipoVeiculo.DobloSpin;
        else                                            // 1 a 4
            Classificacao = ETipoVeiculo.ListaEspera;

        return Classificacao;
    }

    public Passageiro? ObterLider()
    {
        return _alocacoes.FirstOrDefault(a => a.IsLider)?.Passageiro;
    }

    public void RemoverAlocacao(Guid passageiroId)
    {
        var alocacao = _alocacoes.FirstOrDefault(a => a.PassageiroId == passageiroId);
        if (alocacao != null)
            _alocacoes.Remove(alocacao);
    }
}
