using CaronaAlvinegra.Domain.Entities;
using CaronaAlvinegra.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CaronaAlvinegra.Infrastructure.Repositories;

public class PassageiroRepository : Repository<Passageiro>, IPassageiroRepository
{
    public PassageiroRepository(DbContext context) : base(context) { }

    public async Task<IEnumerable<Passageiro>> GetPassageirosPorJogoAsync(
        Guid jogoId,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(p => p.JogoId == jogoId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Passageiro>> GetPassageirosPorRotaAsync(
        Guid jogoId,
        Guid rotaId,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(p => p.JogoId == jogoId && p.RotaId == rotaId)
            .ToListAsync(cancellationToken);
    }
}
