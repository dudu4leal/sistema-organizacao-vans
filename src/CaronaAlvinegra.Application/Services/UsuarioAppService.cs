using AutoMapper;
using CaronaAlvinegra.Application.DTOs;
using CaronaAlvinegra.Domain.Entities;
using CaronaAlvinegra.Domain.Interfaces;
using FluentValidation;

namespace CaronaAlvinegra.Application.Services;

public class UsuarioAppService
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IValidator<UsuarioRequest> _validator;

    public UsuarioAppService(
        IUsuarioRepository usuarioRepo,
        IUnitOfWork uow,
        IMapper mapper,
        IValidator<UsuarioRequest> validator)
    {
        _usuarioRepo = usuarioRepo;
        _uow = uow;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<UsuarioResponse> CriarAsync(UsuarioRequest request, CancellationToken ct = default)
    {
        await _validator.ValidateAndThrowAsync(request, ct);

        var usuario = new Usuario(request.Nome, request.RotaPreferencialId, request.Telefone, request.GrupoId);
        await _usuarioRepo.AddAsync(usuario, ct);
        await _uow.CommitAsync(ct);

        return _mapper.Map<UsuarioResponse>(usuario);
    }

    public async Task<UsuarioResponse?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
    {
        var usuario = await _usuarioRepo.GetByIdAsync(id, ct);
        return usuario is null ? null : _mapper.Map<UsuarioResponse>(usuario);
    }

    public async Task<IEnumerable<UsuarioResponse>> ListarAsync(CancellationToken ct = default)
    {
        var usuarios = await _usuarioRepo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<UsuarioResponse>>(usuarios);
    }

    public async Task<UsuarioResponse?> AtualizarAsync(Guid id, UpdateUsuarioRequest request, CancellationToken ct = default)
    {
        var usuario = await _usuarioRepo.GetByIdAsync(id, ct);
        if (usuario is null) return null;

        if (request.Nome is not null) usuario = new Usuario(request.Nome, usuario.RotaPreferencialId, usuario.Telefone, usuario.GrupoId);
        if (request.RotaPreferencialId.HasValue) usuario.AlterarRotaPreferencial(request.RotaPreferencialId.Value);
        if (request.GrupoId.HasValue)
        {
            var grupo = await _usuarioRepo.GetByIdAsync(request.GrupoId.Value, ct);
            if (grupo is Usuario u) usuario.VincularAoGrupo(u.Grupo ?? null!); // simplified
        }

        _usuarioRepo.Update(usuario);
        await _uow.CommitAsync(ct);

        return _mapper.Map<UsuarioResponse>(usuario);
    }

    public async Task<bool> RemoverAsync(Guid id, CancellationToken ct = default)
    {
        var usuario = await _usuarioRepo.GetByIdAsync(id, ct);
        if (usuario is null) return false;

        _usuarioRepo.Remove(usuario);
        await _uow.CommitAsync(ct);
        return true;
    }
}
