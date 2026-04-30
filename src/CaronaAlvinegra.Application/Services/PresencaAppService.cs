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
    private readonly IPassageiroRepository _passageiroRepo;
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public PresencaAppService(
        IPassageiroRepository passageiroRepo,
        IUsuarioRepository usuarioRepo,
        IUnitOfWork uow,
        IMapper mapper)
    {
        _passageiroRepo = passageiroRepo;
        _usuarioRepo = usuarioRepo;
        _uow = uow;
        _mapper = mapper;
    }

    /// <summary>
    /// Marca a presença de um usuário em um jogo e gera o registro de Passageiro.
    /// </summary>
    public async Task<PresencaResponse?> MarcarPresencaAsync(
        Guid jogoId,
        MarcarPresencaRequest request,
        CancellationToken ct = default)
    {
        var usuario = await _usuarioRepo.GetByIdAsync(request.UsuarioId, ct);
        if (usuario is null) return null;

        // Criar presenca
        var presenca = new Presenca(request.UsuarioId, jogoId, request.RotaEfetivaId);

        // Criar passageiro (instância do usuário para este jogo)
        var passageiro = new Passageiro(
            usuario.Id,
            presenca.Id,
            jogoId,
            request.RotaEfetivaId,
            usuario.Nome);

        await _passageiroRepo.AddAsync(passageiro, ct);
        await _uow.CommitAsync(ct);

        return _mapper.Map<PresencaResponse>(presenca);
    }

    /// <summary>
    /// Remove a presença de um usuário em um jogo.
    /// </summary>
    public async Task<bool> RemoverPresencaAsync(Guid presencaId, CancellationToken ct = default)
    {
        var passageiros = await _passageiroRepo.FindAsync(p => p.PresencaId == presencaId, ct);
        var passageiro = passageiros.FirstOrDefault();

        if (passageiro is null) return false;

        _passageiroRepo.Remove(passageiro);
        await _uow.CommitAsync(ct);
        return true;
    }

    public async Task<IEnumerable<PresencaResponse>> ListarPresencasDoJogoAsync(
        Guid jogoId, CancellationToken ct = default)
    {
        var passageiros = await _passageiroRepo.GetPassageirosPorJogoAsync(jogoId, ct);
        // Simplified mapping - in production, join with Presenca table
        return passageiros.Select(p => new PresencaResponse
        {
            Id = p.Id,
            UsuarioId = p.UsuarioId,
            UsuarioNome = p.Nome,
            JogoId = p.JogoId,
            RotaEfetivaId = p.RotaId,
            ConfirmadoEm = DateTime.UtcNow
        });
    }
}
