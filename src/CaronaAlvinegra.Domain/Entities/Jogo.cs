using System.Text.Json;

namespace CaronaAlvinegra.Domain.Entities;

public class Jogo : AggregateRoot
{
    public string Adversario { get; private set; } = string.Empty;
    public DateTime Data { get; private set; }
    public string Local { get; private set; } = string.Empty;

    /// <summary>
    /// Armazena as preferências de líder por veículo (Ordem -> UsuarioId).
    /// Persistido como coluna JSON no banco de dados.
    /// Exemplo: {"1":"a1b2c3d4-...","3":"e5f6g7h8-..."}
    /// </summary>
    public string? LiderOverridesJson { get; private set; }

    // EF Core
    private Jogo() { }

    public Jogo(string adversario, DateTime data, string local)
    {
        Adversario = adversario;
        Data = data;
        Local = local;
    }

    /// <summary>
    /// Obtém o ID do usuário que foi definido como líder para um determinado veículo.
    /// </summary>
    public Guid? ObterLiderOverride(int veiculoOrdem)
    {
        if (string.IsNullOrWhiteSpace(LiderOverridesJson))
            return null;

        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(LiderOverridesJson);
        if (dict is null || !dict.TryGetValue(veiculoOrdem.ToString(), out var usuarioIdStr))
            return null;

        if (Guid.TryParse(usuarioIdStr, out var usuarioId))
            return usuarioId;

        return null;
    }

    /// <summary>
    /// Define manualmente um passageiro como líder de um veículo,
    /// sobrescrevendo a liderança definida pelo algoritmo de alocação.
    /// </summary>
    public void DefinirLiderOverride(int veiculoOrdem, Guid usuarioId)
    {
        var dict = string.IsNullOrWhiteSpace(LiderOverridesJson)
            ? new Dictionary<string, string>()
            : JsonSerializer.Deserialize<Dictionary<string, string>>(LiderOverridesJson)
              ?? new Dictionary<string, string>();

        dict[veiculoOrdem.ToString()] = usuarioId.ToString();
        LiderOverridesJson = JsonSerializer.Serialize(dict);
        AtualizadoEm = DateTime.UtcNow;
    }

    /// <summary>
    /// Limpa todos os overrides de líder deste jogo.
    /// </summary>
    public void LimparLiderOverrides()
    {
        LiderOverridesJson = null;
        AtualizadoEm = DateTime.UtcNow;
    }
}
