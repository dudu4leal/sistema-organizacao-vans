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

        var grupo = new Grupo(request.Nome, request.RotaId);
        await _grupoRepo.AddAsync(grupo, ct);
        await _uow.CommitAsync(ct);

        return new GrupoResponse
        {
            Id = grupo.Id,
            Nome = grupo.Nome,
            RotaId = grupo.RotaId,
            RotaNome = grupo.Rota?.Nome,
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
            RotaId = grupo.RotaId,
            RotaNome = grupo.Rota?.Nome,
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
                RotaId = grupo.RotaId,
                RotaNome = grupo.Rota?.Nome,
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

        // Usuário sem rota preferencial não pode entrar em grupo roteado
        if (!usuario.RotaPreferencialId.HasValue)
            throw new DomainException(
                $"Não é possível adicionar '{usuario.Nome}' ao grupo '{grupo.Nome}': " +
                $"o usuário não possui uma rota preferencial definida. " +
                "Defina uma rota preferencial para o usuário antes de adicioná-lo a um grupo.");

        // Verificar se a rota do usuário é compatível com a rota do grupo
        if (usuario.RotaPreferencialId.Value != grupo.RotaId)
            throw new DomainException(
                $"Não é possível adicionar '{usuario.Nome}' ao grupo '{grupo.Nome}': " +
                $"a rota preferencial do usuário é diferente da rota do grupo. " +
                "Todos os membros de um grupo devem pertencer à mesma rota. " +
                "Altere a rota do usuário ou escolha outro grupo.");

        usuario.VincularAoGrupo(grupo);
        grupo.AdicionarMembro(usuario);
        _usuarioRepo.Update(usuario);
        await _uow.CommitAsync(ct);
        return true;
    }

    public async Task<bool> RemoverMembroAsync(Guid usuarioId, CancellationToken ct = default)
    {
        var usuario = await _usuarioRepo.GetByIdAsync(usuarioId, ct);
        if (usuario is null) return false;

        usuario.DesvincularDoGrupo();
        _usuarioRepo.Update(usuario);
        await _uow.CommitAsync(ct);
        return true;
    }

    public async Task<bool> RemoverAsync(Guid id, CancellationToken ct = default)
    {
        var grupo = await _grupoRepo.GetByIdAsync(id, ct);
        if (grupo is null) return false;

        // Verifica se há membros vinculados a este grupo
        var membros = await _usuarioRepo.GetUsuariosPorGrupoAsync(id, ct);
        if (membros.Any())
        {
            var nomes = string.Join(", ", membros.Take(3).Select(u => u.Nome));
            var sufixo = membros.Count() > 3 ? $" e mais {membros.Count() - 3}" : "";
            throw new DomainException(
                $"Não é possível excluir o grupo \"{grupo.Nome}\" pois {membros.Count()} torcedor(es) " +
                $"ainda pertencem a ele: {nomes}{sufixo}. " +
                $"Remova os torcedores do grupo antes de excluí-lo.");
        }

        _grupoRepo.Remove(grupo);
        await _uow.CommitAsync(ct);
        return true;
    }
}
