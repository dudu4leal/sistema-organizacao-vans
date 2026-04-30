using CaronaAlvinegra.Domain.Entities;

namespace CaronaAlvinegra.Domain.Interfaces;

public interface IRotaRepository : IRepository<Rota>
{
    Task<Rota?> GetByNomeAsync(string nome, CancellationToken cancellationToken = default);
}
