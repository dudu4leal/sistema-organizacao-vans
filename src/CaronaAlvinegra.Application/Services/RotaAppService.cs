using AutoMapper;
using CaronaAlvinegra.Application.DTOs;
using CaronaAlvinegra.Domain.Entities;
using CaronaAlvinegra.Domain.Interfaces;
using FluentValidation;

namespace CaronaAlvinegra.Application.Services;

public class RotaAppService
{
    private readonly IRotaRepository _rotaRepo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IValidator<RotaRequest> _validator;

    public RotaAppService(
        IRotaRepository rotaRepo,
        IUnitOfWork uow,
        IMapper mapper,
        IValidator<RotaRequest> validator)
    {
        _rotaRepo = rotaRepo;
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

        _rotaRepo.Remove(rota);
        await _uow.CommitAsync(ct);
        return true;
    }
}
