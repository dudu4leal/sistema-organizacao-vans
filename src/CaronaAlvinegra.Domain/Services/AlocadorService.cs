using CaronaAlvinegra.Domain.Entities;
using CaronaAlvinegra.Domain.Enums;
using CaronaAlvinegra.Domain.Interfaces;

namespace CaronaAlvinegra.Domain.Services;

/// <summary>
/// Domain Service responsável pelo algoritmo de alocação de passageiros em veículos.
/// 
/// O algoritmo opera em 3 fases:
/// Fase 1 - Alocação Primária (Greedy com preservação de grupos)
/// Fase 2 - Consolidação (mínimo de 11 para Vans)
/// Fase 3 - Classificação de Saída (Van / DobloSpin / ListaEspera)
/// 
/// Esta classe é isolada da infraestrutura, recebendo dados por parâmetro
/// e retornando o resultado. Isso permite testes unitários sem banco de dados.
/// </summary>
public class AlocadorService
{
    private readonly ValidadorDeIntegridade _validador;

    public AlocadorService(ValidadorDeIntegridade validador)
    {
        _validador = validador;
    }

    /// <summary>
    /// Resultado completo do algoritmo de alocação.
    /// </summary>
    public class ResultadoAlocacao
    {
        public List<Veiculo> Veiculos { get; set; } = new();
        public List<Passageiro> ListaEspera { get; set; } = new();
        public List<string> Erros { get; set; } = new();
        public bool Sucesso => Erros.Count == 0;
    }

    /// <summary>
    /// Executa o algoritmo completo de alocação para um conjunto de passageiros e grupos.
    /// </summary>
    /// <param name="passageiros">Lista de passageiros do jogo</param>
    /// <param name="grupos">Lista de grupos cadastrados</param>
    /// <param name="rotas">Lista de rotas disponíveis</param>
    /// <returns>Resultado da alocação com veículos e lista de espera</returns>
    public ResultadoAlocacao Executar(
        List<Passageiro> passageiros,
        List<Grupo> grupos,
        List<Rota> rotas)
    {
        var resultado = new ResultadoAlocacao();

        // Etapa 2: Validar integridade
        _validador.Validar(passageiros, grupos);
        if (!_validador.IsValid)
        {
            resultado.Erros.AddRange(_validador.Erros);
            return resultado;
        }

        // Etapa 3: Separar por rota
        foreach (var rota in rotas)
        {
            var passageirosDaRota = passageiros
                .Where(p => p.RotaId == rota.Id)
                .ToList();

            if (passageirosDaRota.Count == 0)
                continue;

            var veiculosDaRota = AlocarRota(passageirosDaRota, grupos, rota.Id);

            // Fase 3: Classificar veículos
            foreach (var veiculo in veiculosDaRota)
            {
                var classificacao = veiculo.Classificar();

                if (classificacao == ETipoVeiculo.ListaEspera)
                {
                    // Dissolve o veículo: passageiros vão para lista de espera
                    var passageirosDoVeiculo = veiculo.Alocacoes
                        .Select(a => a.Passageiro)
                        .Where(p => p != null)
                        .Cast<Passageiro>()
                        .ToList();

                    resultado.ListaEspera.AddRange(passageirosDoVeiculo);
                }
                else
                {
                    resultado.Veiculos.Add(veiculo);
                }
            }
        }

        return resultado;
    }

    // ──────────────────────────────────────────────
    //  FASE 1: Alocação Primária (Greedy)
    // ──────────────────────────────────────────────

    private List<Veiculo> AlocarRota(
        List<Passageiro> passageiros,
        List<Grupo> grupos,
        Guid rotaId)
    {
        // Etapa 4: Ordenar elementos (grupos grandes primeiro, avulsos por último)
        var filaOrdenada = MontarFilaOrdenada(passageiros, grupos);

        // Etapa 5: Alocação Greedy
        var veiculos = new List<Veiculo>();
        int ordem = 1;
        var veiculoAtual = new Veiculo(rotaId, ordem);

        foreach (var elemento in filaOrdenada)
        {
            if (elemento.IsGrupo)
            {
                // É um grupo - alocar inteiro
                var grupo = elemento.Grupo!;
                var membrosPresentes = ObterMembrosPresentes(grupo, passageiros);

                if (veiculoAtual.PodeAlocar(membrosPresentes.Count))
                {
                    AlocarGrupo(veiculoAtual, membrosPresentes);
                }
                else
                {
                    // Fechar veículo atual e criar novo
                    veiculoAtual.Fechar();
                    veiculos.Add(veiculoAtual);

                    ordem++;
                    veiculoAtual = new Veiculo(rotaId, ordem);
                    AlocarGrupo(veiculoAtual, membrosPresentes);
                }
            }
            else
            {
                // É um avulso (1 passageiro)
                var passageiro = elemento.Passageiro!;

                if (veiculoAtual.PodeAlocar(1))
                {
                    veiculoAtual.AlocarPassageiro(passageiro);
                }
                else
                {
                    veiculoAtual.Fechar();
                    veiculos.Add(veiculoAtual);

                    ordem++;
                    veiculoAtual = new Veiculo(rotaId, ordem);
                    veiculoAtual.AlocarPassageiro(passageiro);
                }
            }
        }

        // Finalizar último veículo
        if (veiculoAtual.LotacaoAtual > 0)
        {
            veiculoAtual.Fechar();
            veiculos.Add(veiculoAtual);
        }

        // Etapa 6: Fase 2 - Consolidação
        ConsolidarVeiculos(veiculos, passageiros, grupos);

        return veiculos;
    }

    // ──────────────────────────────────────────────
    //  FASE 2: Consolidação (Mínimo de 11)
    // ──────────────────────────────────────────────

    private void ConsolidarVeiculos(
        List<Veiculo> veiculos,
        List<Passageiro> passageiros,
        List<Grupo> grupos)
    {
        if (veiculos.Count <= 1)
            return;

        // Processa todos menos o último
        for (int i = 0; i < veiculos.Count - 1; i++)
        {
            var vanAtual = veiculos[i];

            // Se já tem >= 11 ou tem 5-7 (vai ser DobloSpin), está ok
            if (vanAtual.LotacaoAtual >= Veiculo.LotacaoMinimaVan ||
                vanAtual.LotacaoAtual >= Veiculo.LotacaoMinimaDobloSpin)
                continue;

            // Tem menos de 5 - precisa preencher
            PreencherVeiculo(vanAtual, veiculos, i + 1, grupos);
        }
    }

    private void PreencherVeiculo(
        Veiculo veiculo,
        List<Veiculo> todosVeiculos,
        int inicio,
        List<Grupo> grupos)
    {
        // Pré-computa quais usuários pertencem a grupos
        var usuariosEmGrupos = grupos
            .SelectMany(g => g.Membros.Select(m => m.Id))
            .ToHashSet();

        int deficit = Veiculo.LotacaoMinimaVan - veiculo.LotacaoAtual;
        int vagasDisponiveis = veiculo.VagasRestantes;

        // Tentativa 1: Puxar avulsos de veículos posteriores
        for (int j = inicio; j < todosVeiculos.Count; j++)
        {
            if (deficit <= 0 || vagasDisponiveis <= 0)
                break;

            var veiculoOrigem = todosVeiculos[j];
            var avulsosParaMover = veiculoOrigem.Alocacoes
                .Where(a => a.Passageiro != null && !usuariosEmGrupos.Contains(a.Passageiro.UsuarioId))
                .Take(Math.Min(deficit, vagasDisponiveis))
                .ToList();

            foreach (var alocacao in avulsosParaMover)
            {
                veiculoOrigem.RemoverAlocacao(alocacao.PassageiroId);
                veiculo.AlocarPassageiro(alocacao.Passageiro!);
                deficit--;
                vagasDisponiveis--;
            }
        }

        if (deficit <= 0)
            return;

        // Tentativa 2: Mover grupos pequenos inteiros
        for (int j = inicio; j < todosVeiculos.Count; j++)
        {
            if (deficit <= 0 || vagasDisponiveis <= 0)
                break;

            var veiculoOrigem = todosVeiculos[j];
            var gruposNoVeiculo = IdentificarGruposNoVeiculo(veiculoOrigem, grupos);

            foreach (var (grupo, membros) in gruposNoVeiculo.OrderBy(g => g.membros.Count))
            {
                if (membros.Count <= vagasDisponiveis && membros.Count > 0)
                {
                    foreach (var membro in membros)
                    {
                        veiculoOrigem.RemoverAlocacao(membro.Id);
                        veiculo.AlocarPassageiro(membro);
                    }

                    deficit -= membros.Count;
                    vagasDisponiveis -= membros.Count;

                    if (deficit <= 0)
                        return;
                }
            }
        }

        // Se ainda tem deficit e o veículo tem < 5 pessoas,
        // ele será classificado como ListaEspera na Fase 3
    }

    // ──────────────────────────────────────────────
    //  Métodos auxiliares
    // ──────────────────────────────────────────────

    private record ElementoFila(
        bool IsGrupo,
        Grupo? Grupo,
        Passageiro? Passageiro,
        int Tamanho);

    private List<ElementoFila> MontarFilaOrdenada(
        List<Passageiro> passageiros,
        List<Grupo> grupos)
    {
        var fila = new List<ElementoFila>();
        var passageirosPorUsuario = passageiros.ToDictionary(p => p.UsuarioId, p => p);

        // Grupos ordenados do maior para o menor (considerando apenas membros presentes no jogo)
        foreach (var grupo in grupos.OrderByDescending(g =>
        {
            return g.Membros.Count(m => passageirosPorUsuario.ContainsKey(m.Id));
        }))
        {
            var membrosPresentes = grupo.Membros
                .Where(m => passageirosPorUsuario.ContainsKey(m.Id))
                .Select(m => passageirosPorUsuario[m.Id])
                .ToList();

            if (membrosPresentes.Count > 0)
            {
                fila.Add(new ElementoFila(true, grupo, null, membrosPresentes.Count));
            }
        }

        // Avulsos (usuários sem grupo) por último
        var usuariosComGrupo = grupos.SelectMany(g => g.Membros.Select(m => m.Id)).ToHashSet();
        var avulsos = passageiros
            .Where(p => !usuariosComGrupo.Contains(p.UsuarioId))
            .ToList();

        foreach (var avulso in avulsos)
        {
            fila.Add(new ElementoFila(false, null, avulso, 1));
        }

        return fila;
    }

    private List<Passageiro> ObterMembrosPresentes(Grupo grupo, List<Passageiro> passageiros)
    {
        var passageirosPorUsuario = passageiros.ToDictionary(p => p.UsuarioId, p => p);

        return grupo.Membros
            .Where(m => passageirosPorUsuario.ContainsKey(m.Id))
            .Select(m => passageirosPorUsuario[m.Id])
            .ToList();
    }

    private void AlocarGrupo(Veiculo veiculo, List<Passageiro> membros)
    {
        for (int i = 0; i < membros.Count; i++)
        {
            veiculo.AlocarPassageiro(membros[i], isLider: i == 0);
        }
    }

    /// <summary>
    /// Identifica quais grupos estão alocados em um veículo, retornando
    /// os passageiros que pertencem a cada grupo.
    /// </summary>
    private List<(Grupo grupo, List<Passageiro> membros)> IdentificarGruposNoVeiculo(
        Veiculo veiculo,
        List<Grupo> grupos)
    {
        var resultado = new List<(Grupo, List<Passageiro>)>();

        foreach (var grupo in grupos)
        {
            var idsMembros = grupo.Membros.Select(m => m.Id).ToHashSet();

            var membrosNoVeiculo = veiculo.Alocacoes
                .Select(a => a.Passageiro)
                .Where(p => p != null && idsMembros.Contains(p.UsuarioId))
                .Cast<Passageiro>()
                .ToList();

            if (membrosNoVeiculo.Count > 0)
            {
                resultado.Add((grupo, membrosNoVeiculo));
            }
        }

        return resultado;
    }
}
