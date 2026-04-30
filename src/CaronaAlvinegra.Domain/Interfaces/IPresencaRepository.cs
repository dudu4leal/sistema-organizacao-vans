using CaronaAlvinegra.Domain.Entities;

namespace CaronaAlvinegra.Domain.Interfaces;

public interface IPresencaRepository : IRepository<Presenca>
{
    Task<IEnumerable<Presenca>> GetPresencasPorJogoAsync(Guid jogoId, CancellationToken cancellationToken = default);
}
