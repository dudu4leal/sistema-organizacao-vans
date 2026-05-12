using AutoMapper;
using CaronaAlvinegra.Application.DTOs;
using CaronaAlvinegra.Domain.Entities;
using CaronaAlvinegra.Domain.Interfaces;

namespace CaronaAlvinegra.Application.Services;

/// <summary>
/// Application Service para gerenciar presenças em jogos.
/// A partir das presenças, são gerados os Passageiros (instâncias por jogo).
/// </summary>
public class PresencaAppService
{
    private readonly IPresencaRepository _presencaRepo;
    private readonly IPassageiroRepository _passageiroRepo;
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public PresencaAppService(
        IPresencaRepository presencaRepo,
        IPassageiroRepository passageiroRepo,
        IUsuarioRepository usuarioRepo,
        IUnitOfWork uow,
        IMapper mapper)
    {
        _presencaRepo = presencaRepo;
        _passageiroRepo = passageiroRepo;
        _usuarioRepo = usuarioRepo;
        _uow = uow;
        _mapper = mapper;
    }

    /// <summary>
    /// Marca a presença de um usuário em um jogo e gera o registro de Passageiro.
    /// Quando o usuário não tem rota definida (RotaEfetivaId null), o passageiro
    /// será alocado posteriormente pelo algoritmo na van mais conveniente.
    /// </summary>
    public async Task<PresencaResponse?> MarcarPresencaAsync(
        Guid jogoId,
        MarcarPresencaRequest request,
        CancellationToken ct = default)
    {
        var usuario = await _usuarioRepo.GetByIdAsync(request.UsuarioId, ct);
        if (usuario is null) return null;

        // Verifica se já existe presença para este usuário neste jogo
        var presencasExistentes = await _presencaRepo.FindAsync(
            p => p.UsuarioId == request.UsuarioId && p.JogoId == jogoId, ct);
        if (presencasExistentes.Any())
            throw new DomainException("Usuário já possui presença marcada neste jogo.");

        // Se o usuário não tem rota definida, RotaEfetivaId permanece null.
        // O algoritmo de alocação (AlocadorService) tratará esses passageiros
        // distribuindo-os na van mais conveniente.
        var rotaEfetivaId = request.RotaEfetivaId;

        // Criar e salvar a Presenca PRIMEIRO (para que o Passageiro possa referenciá-la)
        var presenca = new Presenca(request.UsuarioId, jogoId, rotaEfetivaId);
        await _presencaRepo.AddAsync(presenca, ct);

        // Criar passageiro (instância do usuário para este jogo)
        var passageiro = new Passageiro(
            usuario.Id,
            presenca.Id,
            jogoId,
            rotaEfetivaId,
            usuario.Nome);

        await _passageiroRepo.AddAsync(passageiro, ct);
        await _uow.CommitAsync(ct);

        return _mapper.Map<PresencaResponse>(presenca);
    }

    /// <summary>
    /// Remove a presença de um usuário em um jogo.
    /// Remove o Passageiro associado e a Presenca original.
    /// </summary>
    public async Task<bool> RemoverPresencaAsync(Guid presencaId, CancellationToken ct = default)
    {
        // Busca o Passageiro cujo PresencaId corresponde ao id recebido
        var passageiros = await _passageiroRepo.FindAsync(p => p.PresencaId == presencaId, ct);
        var passageiro = passageiros.FirstOrDefault();

        if (passageiro is null) return false;

        // Remove o Passageiro primeiro (evita FK violation)
        _passageiroRepo.Remove(passageiro);

        // Agora remove a Presenca original
        var presenca = await _presencaRepo.GetByIdAsync(presencaId, ct);
        if (presenca is not null)
        {
            _presencaRepo.Remove(presenca);
        }

        await _uow.CommitAsync(ct);
        return true;
    }

    public async Task<IEnumerable<PresencaResponse>> ListarPresencasDoJogoAsync(
        Guid jogoId, CancellationToken ct = default)
    {
        var passageiros = await _passageiroRepo.GetPassageirosPorJogoAsync(jogoId, ct);
        var passageirosList = passageiros.ToList();

        // Carregar as presenças reais para obter o ConfirmadoEm original
        var presencaIds = passageirosList.Select(p => p.PresencaId).Distinct();
        var presencasDict = (await _presencaRepo.FindAsync(
                p => presencaIds.Contains(p.Id), ct))
            .ToDictionary(p => p.Id, p => p.ConfirmadoEm);

        // Retorna o PresencaId real (p.PresencaId) como Id, pois é ele que usamos
        // para remover a presença. O p.Id é o Id do Passageiro, não da Presenca.
        return passageirosList.Select(p => new PresencaResponse
        {
            Id = p.PresencaId,
            UsuarioId = p.UsuarioId,
            UsuarioNome = p.Nome,
            JogoId = p.JogoId,
            RotaEfetivaId = p.RotaId,
            ConfirmadoEm = presencasDict.TryGetValue(p.PresencaId, out var confirmadoEm)
                ? confirmadoEm
                : DateTime.UtcNow
        });
    }
}
