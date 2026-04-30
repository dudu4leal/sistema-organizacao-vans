using CaronaAlvinegra.Domain.Entities;
using CaronaAlvinegra.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CaronaAlvinegra.Infrastructure.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(DbContext context) : base(context) { }

    public async Task<IEnumerable<Usuario>> GetUsuariosPorGrupoAsync(
        Guid grupoId,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(u => u.GrupoId == grupoId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Usuario?> GetByNomeAsync(
        string nome,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(u => u.Nome == nome, cancellationToken);
    }
}
