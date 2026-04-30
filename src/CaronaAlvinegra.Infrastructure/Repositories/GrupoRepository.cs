using CaronaAlvinegra.Domain.Entities;
using CaronaAlvinegra.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CaronaAlvinegra.Infrastructure.Repositories;

public class GrupoRepository : Repository<Grupo>, IGrupoRepository
{
    public GrupoRepository(DbContext context) : base(context) { }
}
