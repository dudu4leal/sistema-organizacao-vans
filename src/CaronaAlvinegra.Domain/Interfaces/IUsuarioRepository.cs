using CaronaAlvinegra.Domain.Entities;

namespace CaronaAlvinegra.Domain.Interfaces;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<IEnumerable<Usuario>> GetUsuariosPorGrupoAsync(Guid grupoId, CancellationToken cancellationToken = default);
    Task<Usuario?> GetByNomeAsync(string nome, CancellationToken cancellationToken = default);
}
