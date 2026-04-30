using AutoMapper;
using CaronaAlvinegra.Application.DTOs;
using CaronaAlvinegra.Domain.Entities;
using CaronaAlvinegra.Domain.Interfaces;
using CaronaAlvinegra.Domain.Services;
using FluentValidation;

namespace CaronaAlvinegra.Application.Services;

/// <summary>
/// Application Service que orquestra o ciclo de vida de um Jogo:
/// criação, marcação de presença, geração de passageiros, alocação.
/// </summary>
public class JogoAppService
{
    private readonly IJogoRepository _jogoRepo;
    private readonly IPassageiroRepository _passageiroRepo;
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IGrupoRepository _grupoRepo;
    private readonly IRotaRepository _rotaRepo;
    private readonly IUnitOfWork _uow;
    private readonly AlocadorService _alocador;
    private readonly IMapper _mapper;
    private readonly IValidator<JogoRequest> _validator;

    public JogoAppService(
        IJogoRepository jogoRepo,
        IPassageiroRepository passageiroRepo,
        IUsuarioRepository usuarioRepo,
        IGrupoRepository grupoRepo,
        IRotaRepository rotaRepo,
        IUnitOfWork uow,
        AlocadorService alocador,
        IMapper mapper,
        IValidator<JogoRequest> validator)
    {
        _jogoRepo = jogoRepo;
        _passageiroRepo = passageiroRepo;
        _usuarioRepo = usuarioRepo;
        _grupoRepo = grupoRepo;
        _rotaRepo = rotaRepo;
        _uow = uow;
        _alocador = alocador;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<JogoResponse> CriarAsync(JogoRequest request, CancellationToken ct = default)
    {
        await _validator.ValidateAndThrowAsync(request, ct);

        var jogo = new Jogo(request.Adversario, request.Data, request.Local);
        await _jogoRepo.AddAsync(jogo, ct);
        await _uow.CommitAsync(ct);

        return _mapper.Map<JogoResponse>(jogo);
    }

    public async Task<JogoResponse?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
    {
        var jogo = await _jogoRepo.GetByIdAsync(id, ct);
        if (jogo is null) return null;

        var presentes = (await _passageiroRepo.GetPassageirosPorJogoAsync(id, ct)).ToList();
        var response = _mapper.Map<JogoResponse>(jogo);
        response.TotalPresentes = presentes.Count;
        response.AlocacaoRealizada = presentes.Any();
        return response;
    }

    public async Task<IEnumerable<JogoResponse>> ListarAsync(CancellationToken ct = default)
    {
        var jogos = await _jogoRepo.GetAllAsync(ct);
        var responses = new List<JogoResponse>();

        foreach (var jogo in jogos)
        {
            var presentes = (await _passageiroRepo.GetPassageirosPorJogoAsync(jogo.Id, ct)).ToList();
            var dto = _mapper.Map<JogoResponse>(jogo);
            dto.TotalPresentes = presentes.Count;
            dto.AlocacaoRealizada = presentes.Any();
            responses.Add(dto);
        }

        return responses;
    }

    /// <summary>
    /// Gera os passageiros para um jogo a partir das presenças confirmadas
    /// e executa o algoritmo de alocação.
    /// </summary>
    public async Task<ResultadoAlocacaoDto> AlocarJogoAsync(Guid jogoId, CancellationToken ct = default)
    {
        var jogo = await _jogoRepo.GetByIdAsync(jogoId, ct);
        if (jogo is null)
            return new ResultadoAlocacaoDto
            {
                JogoId = jogoId,
                Erros = ["Jogo não encontrado."],
                Sucesso = false
            };

        // Buscar presenças do jogo
        var presencas = await _passageiroRepo.FindAsync(p => p.JogoId == jogoId, ct);
        var presencasList = presencas.ToList();

        if (!presencasList.Any())
            return new ResultadoAlocacaoDto
            {
                JogoId = jogoId,
                JogoDescricao = $"{jogo.Adversario} - {jogo.Data:dd/MM}",
                Erros = ["Nenhuma presença confirmada para este jogo."],
                Sucesso = false
            };

        // Buscar dados de apoio
        var grupos = (await _grupoRepo.GetAllAsync(ct)).ToList();
        var rotas = (await _rotaRepo.GetAllAsync(ct)).ToList();

        // Executar algoritmo de alocação
        var resultado = _alocador.Executar(presencasList, grupos, rotas);

        // Mapear resultado para DTO
        var veiculosDto = resultado.Veiculos.Select(v =>
        {
            int passageiroNum = 1;
            var passageiros = v.Alocacoes.Select(a =>
            {
                var p = a.Passageiro;
                return new PassageiroDto
                {
                    Numero = passageiroNum++,
                    Nome = p?.Nome ?? "?",
                    IsLider = a.IsLider
                };
            }).ToList();

            return new VeiculoDto
            {
                Ordem = v.Ordem,
                Classificacao = v.Classificacao,
                TipoDescricao = ObterDescricaoTipo(v.Classificacao),
                Lotacao = v.LotacaoAtual,
                VagasRestantes = v.VagasRestantes,
                Passageiros = passageiros
            };
        }).ToList();

        int esperaNum = 1;
        var esperaDto = resultado.ListaEspera.Select(p =>
            new PassageiroDto
            {
                Numero = esperaNum++,
                Nome = p.Nome,
                IsLider = false
            }).ToList();

        return new ResultadoAlocacaoDto
        {
            JogoId = jogoId,
            JogoDescricao = $"{jogo.Adversario} - {jogo.Data:dd/MM/yyyy}",
            Veiculos = veiculosDto,
            ListaEspera = esperaDto,
            Erros = resultado.Erros,
            Sucesso = resultado.Sucesso
        };
    }

    public async Task<bool> RemoverAsync(Guid id, CancellationToken ct = default)
    {
        var jogo = await _jogoRepo.GetByIdAsync(id, ct);
        if (jogo is null) return false;

        _jogoRepo.Remove(jogo);
        await _uow.CommitAsync(ct);
        return true;
    }

    private static string ObterDescricaoTipo(Domain.Enums.ETipoVeiculo tipo) => tipo switch
    {
        Domain.Enums.ETipoVeiculo.Van => "Van",
        Domain.Enums.ETipoVeiculo.DobloSpin => "Doblo/Spin",
        Domain.Enums.ETipoVeiculo.ListaEspera => "Lista de Espera",
        _ => "Desconhecido"
    };
}
