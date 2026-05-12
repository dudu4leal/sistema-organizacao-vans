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

    // Thresholds para consolidação (Fase 2) — mínimo de 11 para fechar uma Van
    public const int LotacaoMinimaVan = 11;

    // Thresholds para classificação (Fase 3) — conforme especificação:
    // Van: 8 a 15 | Doblo/Spin: 5 a 7 | Lista de Espera: 1 a 4
    public const int ClassificacaoMinimaVan = 8;
    public const int ClassificacaoMinimaDobloSpin = 5;
    public const int ClassificacaoMaximaDobloSpin = 7;

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

        // Apenas um líder por veículo: se já existe um líder, ignora o flag isLider
        bool efetivoLider = isLider && !_alocacoes.Any(a => a.IsLider);

        var alocacao = new Alocacao(Id, passageiro, efetivoLider);
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
    /// Van: 8 a 15 | Doblo/Spin: 5 a 7 | Lista de Espera: 1 a 4
    /// </summary>
    public ETipoVeiculo Classificar()
    {
        if (LotacaoAtual >= ClassificacaoMinimaVan)              // 8 a 15 → Van
            Classificacao = ETipoVeiculo.Van;
        else if (LotacaoAtual >= ClassificacaoMinimaDobloSpin &&
                 LotacaoAtual <= ClassificacaoMaximaDobloSpin)    // 5 a 7 → Doblo/Spin
            Classificacao = ETipoVeiculo.DobloSpin;
        else                                                     // 1 a 4 → Lista de Espera
            Classificacao = ETipoVeiculo.ListaEspera;

        return Classificacao;
    }

    public Passageiro? ObterLider()
    {
        return _alocacoes.FirstOrDefault(a => a.IsLider)?.Passageiro;
    }

    /// <summary>
    /// Define um passageiro como líder do veículo, removendo o líder anterior (se houver).
    /// Garante que apenas UM passageiro seja líder por vez.
    /// </summary>
    public void DefinirLider(Guid passageiroId)
    {
        var alocacaoAlvo = _alocacoes.FirstOrDefault(a => a.PassageiroId == passageiroId)
            ?? throw new DomainException($"Passageiro {passageiroId} não está alocado neste veículo.");

        // Remove liderança de todas as outras alocações
        foreach (var aloc in _alocacoes)
        {
            if (aloc.IsLider && aloc.PassageiroId != passageiroId)
            {
                aloc.RemoverLideranca();
            }
        }

        alocacaoAlvo.DefinirComoLider();
        AtualizadoEm = DateTime.UtcNow;
    }

    public void RemoverAlocacao(Guid passageiroId)
    {
        var alocacao = _alocacoes.FirstOrDefault(a => a.PassageiroId == passageiroId);
        if (alocacao != null)
            _alocacoes.Remove(alocacao);
    }

    /// <summary>
    /// Reabre o veículo para permitir alocação de mais passageiros,
    /// desde que ainda haja vagas restantes.
    /// Usado pelo AlocadorService para distribuir passageiros sem rota
    /// em veículos que foram fechados durante a alocação primária.
    /// </summary>
    public void Reabrir()
    {
        if (VagasRestantes > 0)
            Fechado = false;
    }
}
