using CaronaAlvinegra.Domain.Entities;
using CaronaAlvinegra.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CaronaAlvinegra.Infrastructure.Repositories;

public class PresencaRepository : Repository<Presenca>, IPresencaRepository
{
    private readonly DbContext _context;

    public PresencaRepository(DbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Presenca>> GetPresencasPorJogoAsync(Guid jogoId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Presenca>()
            .Where(p => p.JogoId == jogoId)
            .ToListAsync(cancellationToken);
    }
}
