using CaronaAlvinegra.Domain.Entities;
using CaronaAlvinegra.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CaronaAlvinegra.Infrastructure.Repositories;

public class JogoRepository : Repository<Jogo>, IJogoRepository
{
    public JogoRepository(DbContext context) : base(context) { }
}
