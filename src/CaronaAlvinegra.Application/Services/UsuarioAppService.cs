using AutoMapper;
using CaronaAlvinegra.Application.DTOs;
using CaronaAlvinegra.Domain.Entities;
using CaronaAlvinegra.Domain.Interfaces;
using FluentValidation;

namespace CaronaAlvinegra.Application.Services;

public class UsuarioAppService
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IGrupoRepository _grupoRepo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IValidator<UsuarioRequest> _validator;
    private readonly IValidator<UpdateUsuarioRequest> _updateValidator;

    public UsuarioAppService(
        IUsuarioRepository usuarioRepo,
        IGrupoRepository grupoRepo,
        IUnitOfWork uow,
        IMapper mapper,
        IValidator<UsuarioRequest> validator,
        IValidator<UpdateUsuarioRequest> updateValidator)
    {
        _usuarioRepo = usuarioRepo;
        _grupoRepo = grupoRepo;
        _uow = uow;
        _mapper = mapper;
        _validator = validator;
        _updateValidator = updateValidator;
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
        await _updateValidator.ValidateAndThrowAsync(request, ct);

        var usuario = await _usuarioRepo.GetByIdAsync(id, ct);
        if (usuario is null) return null;

        if (request.Nome is not null)
            usuario.AlterarNome(request.Nome);

        // Telefone pode ser string ou null (para limpar o campo)
        usuario.AlterarTelefone(request.Telefone);

        // ── Rota Preferencial ─────────────────────────
        if (request.RotaPreferencialId.HasValue)
        {
            // Está definindo uma rota específica (diferente da atual)
            if (request.RotaPreferencialId.Value != usuario.RotaPreferencialId)
            {
                // Se pertence a um grupo, verificar compatibilidade
                if (usuario.GrupoId.HasValue)
                {
                    var grupo = await _grupoRepo.GetByIdAsync(usuario.GrupoId.Value, ct);
                    if (grupo is not null && grupo.RotaId != request.RotaPreferencialId.Value)
                    {
                        throw new DomainException(
                            $"Não é possível alterar a rota de '{usuario.Nome}': " +
                            $"ele pertence ao grupo '{grupo.Nome}' que é de uma rota diferente. " +
                            "Remova o usuário do grupo primeiro ou altere a rota do grupo.");
                    }
                }

                usuario.AlterarRotaPreferencial(request.RotaPreferencialId.Value);
            }
        }
        else
        {
            // request.RotaPreferencialId veio null → está limpando a preferência
            // Se pertence a um grupo, não pode ficar sem rota
            if (usuario.GrupoId.HasValue)
            {
                throw new DomainException(
                    $"Não é possível remover a rota preferencial de '{usuario.Nome}': " +
                    $"ele pertence ao grupo (ID: {usuario.GrupoId.Value}). " +
                    "Remova o usuário do grupo primeiro para poder limpar a rota preferencial.");
            }

            if (usuario.RotaPreferencialId.HasValue)
                usuario.AlterarRotaPreferencial(null);
        }

        // ── Grupo ─────────────────────────────────────
        if (request.GrupoId.HasValue)
        {
            var grupo = await _grupoRepo.GetByIdAsync(request.GrupoId.Value, ct);
            if (grupo is null)
                throw new DomainException("Grupo informado não encontrado.");

            usuario.VincularAoGrupo(grupo);
        }
        else
        {
            usuario.DesvincularDoGrupo();
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
