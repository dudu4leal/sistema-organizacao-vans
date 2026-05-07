using CaronaAlvinegra.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CaronaAlvinegra.Infrastructure.Data;

public static class DataSeed
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Usuarios.AnyAsync())
            return;

        // ── Rotas ─────────────────────────────────────
        var rotaCascatinha = new Rota("Cascatinha", "Praça da Cascatinha");
        var rotaCentro = new Rota("Centro", "Praça da Inconfidência");
        var rotaMosela = new Rota("Mosela", "Ponto Final Mosela");
        var rotaQuitandinha = new Rota("Quitandinha", "Portão do Palácio Quitandinha");

        context.Rotas.AddRange(rotaCascatinha, rotaCentro, rotaMosela, rotaQuitandinha);
        await context.SaveChangesAsync();

        // ── Grupos ────────────────────────────────────
        var grupoAmigos = new Grupo("Amigos do Bairro", rotaCascatinha.Id);
        var grupoFamilia = new Grupo("Família Souza", rotaMosela.Id);
        var grupoTrabalho = new Grupo("Colegas de Trabalho", rotaCentro.Id);

        context.Grupos.AddRange(grupoAmigos, grupoFamilia, grupoTrabalho);
        await context.SaveChangesAsync();

        // ── Usuários ──────────────────────────────────
        // Construtor: Usuario(nome, rotaPreferencialId?, telefone?, grupoId?)
        var usuarios = new List<Usuario>
        {
            new Usuario("Carlos Silva",     rotaCascatinha.Id,   "11999990001", grupoAmigos.Id),
            new Usuario("Ana Silva",        rotaCascatinha.Id,   "11999990002", grupoAmigos.Id),
            new Usuario("Pedro Santos",     rotaCentro.Id,       "11999990003", grupoAmigos.Id),
            new Usuario("Maria Souza",      rotaMosela.Id,       "11999990004", grupoFamilia.Id),
            new Usuario("João Souza",       rotaMosela.Id,       "11999990005", grupoFamilia.Id),
            new Usuario("José Souza",       rotaMosela.Id,       "11999990006", grupoFamilia.Id),
            new Usuario("Lucia Costa",      rotaCentro.Id,       "11999990007", grupoTrabalho.Id),
            new Usuario("Rafael Costa",     rotaCentro.Id,       "11999990008", grupoTrabalho.Id),
            new Usuario("Fernanda Lima",    rotaQuitandinha.Id,  "11999990009"),
            new Usuario("Roberto Alves",    rotaQuitandinha.Id,  "11999990010"),
            new Usuario("Paulo Oliveira",   null,                "11999990011"), // Sem rota preferencial
            new Usuario("Marina Dias",      null,                "11999990012"), // Sem rota preferencial
        };

        // Vincular membros aos grupos (popula a coleção _membros em memória)
        var gruposList = new[] { grupoAmigos, grupoFamilia, grupoTrabalho };
        foreach (var usuario in usuarios.Where(u => u.GrupoId.HasValue))
        {
            var grupo = gruposList.FirstOrDefault(g => g.Id == usuario.GrupoId.Value);
            if (grupo is not null)
                grupo.AdicionarMembro(usuario);
        }

        context.Usuarios.AddRange(usuarios);
        await context.SaveChangesAsync();
    }
}
