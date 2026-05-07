using CaronaAlvinegra.Domain.Entities;
using CaronaAlvinegra.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CaronaAlvinegra.Infrastructure.Repositories;

public class GrupoRepository : Repository<Grupo>, IGrupoRepository
{
    public GrupoRepository(DbContext context) : base(context) { }

    public override async Task<IEnumerable<Grupo>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(g => g.Membros)
            .ToListAsync(cancellationToken);
    }
}
