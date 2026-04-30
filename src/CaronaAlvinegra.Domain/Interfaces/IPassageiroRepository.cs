using CaronaAlvinegra.Domain.Entities;

namespace CaronaAlvinegra.Domain.Interfaces;

public interface IPassageiroRepository : IRepository<Passageiro>
{
    Task<IEnumerable<Passageiro>> GetPassageirosPorJogoAsync(Guid jogoId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Passageiro>> GetPassageirosPorRotaAsync(Guid jogoId, Guid rotaId, CancellationToken cancellationToken = default);
}
