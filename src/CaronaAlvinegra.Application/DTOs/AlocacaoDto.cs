using CaronaAlvinegra.Domain.Enums;

namespace CaronaAlvinegra.Application.DTOs;

/// <summary>
/// DTO de resultado da alocação de um jogo,
/// formatado para exibição no WhatsApp.
/// </summary>
public class ResultadoAlocacaoDto
{
    public Guid JogoId { get; init; }
    public string? JogoDescricao { get; init; }
    public List<VeiculoDto> Veiculos { get; init; } = [];
    public List<PassageiroDto> ListaEspera { get; init; } = [];
    public List<string> Erros { get; init; } = [];
    public bool Sucesso { get; init; }
}

public class VeiculoDto
{
    public int Ordem { get; init; }
    public ETipoVeiculo Classificacao { get; init; }
    public string TipoDescricao { get; init; } = string.Empty;
    public int Lotacao { get; init; }
    public int VagasRestantes { get; init; }
    public List<PassageiroDto> Passageiros { get; init; } = [];
}

public class PassageiroDto
{
    public int Numero { get; init; }
    public string Nome { get; init; } = string.Empty;
    public bool IsLider { get; init; }
    public string? Telefone { get; init; }
}
