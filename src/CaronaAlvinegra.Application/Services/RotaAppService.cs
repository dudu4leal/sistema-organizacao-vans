using AutoMapper;
using CaronaAlvinegra.Application.DTOs;
using CaronaAlvinegra.Domain.Entities;
using CaronaAlvinegra.Domain.Interfaces;
using FluentValidation;

namespace CaronaAlvinegra.Application.Services;

public class RotaAppService
{
    private readonly IRotaRepository _rotaRepo;
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IGrupoRepository _grupoRepo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IValidator<RotaRequest> _validator;

    public RotaAppService(
        IRotaRepository rotaRepo,
        IUsuarioRepository usuarioRepo,
        IGrupoRepository grupoRepo,
        IUnitOfWork uow,
        IMapper mapper,
        IValidator<RotaRequest> validator)
    {
        _rotaRepo = rotaRepo;
        _usuarioRepo = usuarioRepo;
        _grupoRepo = grupoRepo;
        _uow = uow;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<RotaResponse> CriarAsync(RotaRequest request, CancellationToken ct = default)
    {
        await _validator.ValidateAndThrowAsync(request, ct);

        var rota = new Rota(request.Nome, request.LocalEmbarque);
        await _rotaRepo.AddAsync(rota, ct);
        await _uow.CommitAsync(ct);

        return _mapper.Map<RotaResponse>(rota);
    }

    public async Task<RotaResponse?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
    {
        var rota = await _rotaRepo.GetByIdAsync(id, ct);
        return rota is null ? null : _mapper.Map<RotaResponse>(rota);
    }

    public async Task<IEnumerable<RotaResponse>> ListarAsync(CancellationToken ct = default)
    {
        var rotas = await _rotaRepo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<RotaResponse>>(rotas);
    }

    public async Task<bool> RemoverAsync(Guid id, CancellationToken ct = default)
    {
        var rota = await _rotaRepo.GetByIdAsync(id, ct);
        if (rota is null) return false;

        // Verifica se há usuários vinculados a esta rota
        var usuariosVinculados = await _usuarioRepo.FindAsync(u => u.RotaPreferencialId == id, ct);
        if (usuariosVinculados.Any())
        {
            var nomes = string.Join(", ", usuariosVinculados.Take(3).Select(u => u.Nome));
            var sufixo = usuariosVinculados.Count() > 3 ? $" e mais {usuariosVinculados.Count() - 3}" : "";
            throw new DomainException(
                $"Não é possível excluir a rota \"{rota.Nome}\" pois {usuariosVinculados.Count()} usuário(s) " +
                $"ainda a possuem como rota preferencial: {nomes}{sufixo}. " +
                $"Reatribua esses usuários para outra rota antes de excluir.");
        }

        // Verifica se há grupos vinculados a esta rota
        var gruposVinculados = await _grupoRepo.FindAsync(g => g.RotaId == id, ct);
        if (gruposVinculados.Any())
        {
            var nomes = string.Join(", ", gruposVinculados.Take(3).Select(g => g.Nome));
            var sufixo = gruposVinculados.Count() > 3 ? $" e mais {gruposVinculados.Count() - 3}" : "";
            throw new DomainException(
                $"Não é possível excluir a rota \"{rota.Nome}\" pois {gruposVinculados.Count()} grupo(s) " +
                $"ainda a utilizam: {nomes}{sufixo}. " +
                $"Reatribua esses grupos para outra rota antes de excluir.");
        }

        _rotaRepo.Remove(rota);
        await _uow.CommitAsync(ct);
        return true;
    }
}
