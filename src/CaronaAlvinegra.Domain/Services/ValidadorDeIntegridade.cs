using CaronaAlvinegra.Domain.Entities;

namespace CaronaAlvinegra.Domain.Services;

/// <summary>
/// Validador de integridade dos dados antes da execução do algoritmo de alocação.
/// Executa na Etapa 2 do fluxo de processamento.
/// </summary>
public class ValidadorDeIntegridade
{
    private readonly List<string> _erros = new();

    public IReadOnlyList<string> Erros => _erros.AsReadOnly();
    public bool IsValid => _erros.Count == 0;

    /// <summary>
    /// Valida a lista de passageiros e grupos antes da alocação.
    /// </summary>
    public void Validar(List<Passageiro> passageiros, List<Grupo> grupos)
    {
        _erros.Clear();

        ValidarPassageirosSemRota(passageiros);
        ValidarGruposVazios(grupos);
        ValidarGruposExcedemCapacidade(passageiros, grupos);
        ValidarConsistenciaRotaNosGrupos(passageiros, grupos);
    }

    private void ValidarPassageirosSemRota(List<Passageiro> passageiros)
    {
        var semRota = passageiros.Where(p => p.RotaId == Guid.Empty).ToList();
        foreach (var p in semRota)
        {
            _erros.Add($"Passageiro '{p.Nome}' não possui rota definida.");
        }
    }

    private void ValidarGruposVazios(List<Grupo> grupos)
    {
        // Grupos vazios (sem membros) não têm impacto, apenas ignoramos
    }

    private void ValidarGruposExcedemCapacidade(List<Passageiro> passageiros, List<Grupo> grupos)
    {
        var passageirosPorUsuario = passageiros.ToDictionary(p => p.UsuarioId, p => p);

        foreach (var grupo in grupos)
        {
            var membrosNoJogo = grupo.Membros
                .Where(m => passageirosPorUsuario.ContainsKey(m.Id))
                .ToList();

            if (membrosNoJogo.Count > Veiculo.CapacidadeMaxima)
            {
                _erros.Add(
                    $"Grupo '{grupo.Nome}' possui {membrosNoJogo.Count} integrantes " +
                    $"confirmados neste jogo e excede a capacidade de um veículo " +
                    $"({Veiculo.CapacidadeMaxima} lugares). Sugerimos dividir manualmente " +
                    $"o grupo em subgrupos de no máximo {Veiculo.CapacidadeMaxima} pessoas.");
            }
        }
    }

    private void ValidarConsistenciaRotaNosGrupos(List<Passageiro> passageiros, List<Grupo> grupos)
    {
        var passageirosPorUsuario = passageiros.ToDictionary(p => p.UsuarioId, p => p);

        foreach (var grupo in grupos)
        {
            var membrosPresentes = grupo.Membros
                .Where(m => passageirosPorUsuario.ContainsKey(m.Id))
                .Select(m => passageirosPorUsuario[m.Id])
                .ToList();

            if (membrosPresentes.Count < 2)
                continue;

            var rotas = membrosPresentes.Select(p => p.RotaId).Distinct().ToList();
            if (rotas.Count > 1)
            {
                var nomes = string.Join(", ", membrosPresentes.Select(p => p.Nome));
                _erros.Add(
                    $"Grupo '{grupo.Nome}' possui membros em rotas distintas neste jogo " +
                    $"({nomes}). Todos os integrantes de um grupo devem pertencer à mesma rota.");
            }
        }
    }
}
