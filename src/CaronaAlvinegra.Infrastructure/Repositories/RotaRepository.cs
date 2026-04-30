using CaronaAlvinegra.Domain.Entities;
using CaronaAlvinegra.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CaronaAlvinegra.Infrastructure.Repositories;

public class RotaRepository : Repository<Rota>, IRotaRepository
{
    public RotaRepository(DbContext context) : base(context) { }

    public async Task<Rota?> GetByNomeAsync(
        string nome,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(r => r.Nome == nome, cancellationToken);
    }
}
