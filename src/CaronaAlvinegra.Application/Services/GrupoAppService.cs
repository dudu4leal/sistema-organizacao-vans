using AutoMapper;
using CaronaAlvinegra.Application.DTOs;
using CaronaAlvinegra.Domain.Entities;
using CaronaAlvinegra.Domain.Interfaces;
using FluentValidation;

namespace CaronaAlvinegra.Application.Services;

public class GrupoAppService
{
    private readonly IGrupoRepository _grupoRepo;
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IValidator<GrupoRequest> _validator;

    public GrupoAppService(
        IGrupoRepository grupoRepo,
        IUsuarioRepository usuarioRepo,
        IUnitOfWork uow,
        IMapper mapper,
        IValidator<GrupoRequest> validator)
    {
        _grupoRepo = grupoRepo;
        _usuarioRepo = usuarioRepo;
        _uow = uow;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<GrupoResponse> CriarAsync(GrupoRequest request, CancellationToken ct = default)
    {
        await _validator.ValidateAndThrowAsync(request, ct);

        var grupo = new Grupo(request.Nome);
        await _grupoRepo.AddAsync(grupo, ct);
        await _uow.CommitAsync(ct);

        return new GrupoResponse
        {
            Id = grupo.Id,
            Nome = grupo.Nome,
            TotalMembros = 0
        };
    }

    public async Task<GrupoResponse?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
    {
        var grupo = await _grupoRepo.GetByIdAsync(id, ct);
        if (grupo is null) return null;

        var membros = await _usuarioRepo.GetUsuariosPorGrupoAsync(id, ct);
        return new GrupoResponse
        {
            Id = grupo.Id,
            Nome = grupo.Nome,
            TotalMembros = membros.Count()
        };
    }

    public async Task<IEnumerable<GrupoResponse>> ListarAsync(CancellationToken ct = default)
    {
        var grupos = await _grupoRepo.GetAllAsync(ct);
        var responses = new List<GrupoResponse>();

        foreach (var grupo in grupos)
        {
            var membros = await _usuarioRepo.GetUsuariosPorGrupoAsync(grupo.Id, ct);
            responses.Add(new GrupoResponse
            {
                Id = grupo.Id,
                Nome = grupo.Nome,
                TotalMembros = membros.Count()
            });
        }

        return responses;
    }

    public async Task<bool> AdicionarMembroAsync(Guid grupoId, Guid usuarioId, CancellationToken ct = default)
    {
        var grupo = await _grupoRepo.GetByIdAsync(grupoId, ct);
        var usuario = await _usuarioRepo.GetByIdAsync(usuarioId, ct);

        if (grupo is null || usuario is null) return false;

        usuario.VincularAoGrupo(grupo);
        _usuarioRepo.Update(usuario);
        await _uow.CommitAsync(ct);
        return true;
    }

    public async Task<bool> RemoverAsync(Guid id, CancellationToken ct = default)
    {
        var grupo = await _grupoRepo.GetByIdAsync(id, ct);
        if (grupo is null) return false;

        _grupoRepo.Remove(grupo);
        await _uow.CommitAsync(ct);
        return true;
    }
}
