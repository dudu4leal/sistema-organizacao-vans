using CaronaAlvinegra.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CaronaAlvinegra.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Grupo> Grupos => Set<Grupo>();
    public DbSet<Rota> Rotas => Set<Rota>();
    public DbSet<Jogo> Jogos => Set<Jogo>();
    public DbSet<Presenca> Presencas => Set<Presenca>();
    public DbSet<Passageiro> Passageiros => Set<Passageiro>();
    public DbSet<Veiculo> Veiculos => Set<Veiculo>();
    public DbSet<Alocacao> Alocacoes => Set<Alocacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── Usuario ──────────────────────────────────
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuarios");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Telefone).HasMaxLength(20);

            entity.HasOne(e => e.RotaPreferencial)
                  .WithMany()
                  .HasForeignKey(e => e.RotaPreferencialId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Grupo)
                  .WithMany(g => g.Membros)
                  .HasForeignKey(e => e.GrupoId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // ── Grupo ─────────────────────────────────────
        modelBuilder.Entity<Grupo>(entity =>
        {
            entity.ToTable("Grupos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);

            // Membros é uma collection navigation gerenciada separadamente
            entity.Ignore(e => e.Membros);
        });

        // ── Rota ──────────────────────────────────────
        modelBuilder.Entity<Rota>(entity =>
        {
            entity.ToTable("Rotas");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LocalEmbarque).IsRequired().HasMaxLength(200);
        });

        // ── Jogo ──────────────────────────────────────
        modelBuilder.Entity<Jogo>(entity =>
        {
            entity.ToTable("Jogos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Adversario).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Local).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Data).IsRequired();
        });

        // ── Presenca ──────────────────────────────────
        modelBuilder.Entity<Presenca>(entity =>
        {
            entity.ToTable("Presencas");
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Usuario)
                  .WithMany()
                  .HasForeignKey(e => e.UsuarioId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Jogo)
                  .WithMany()
                  .HasForeignKey(e => e.JogoId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.RotaEfetiva)
                  .WithMany()
                  .HasForeignKey(e => e.RotaEfetivaId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex(e => new { e.UsuarioId, e.JogoId }).IsUnique();
        });

        // ── Passageiro ────────────────────────────────
        modelBuilder.Entity<Passageiro>(entity =>
        {
            entity.ToTable("Passageiros");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);

            entity.HasIndex(e => e.UsuarioId);

            entity.HasOne(e => e.Presenca)
                  .WithMany()
                  .HasForeignKey(e => e.PresencaId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Rota)
                  .WithMany()
                  .HasForeignKey(e => e.RotaId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        // ── Veiculo ──────────────────────────────────
        modelBuilder.Entity<Veiculo>(entity =>
        {
            entity.ToTable("Veiculos");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Classificacao)
                  .HasConversion<int>()
                  .IsRequired();

            entity.HasOne(e => e.Rota)
                  .WithMany()
                  .HasForeignKey(e => e.RotaId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.Ignore(e => e.Alocacoes);
            entity.Ignore(e => e.LotacaoAtual);
            entity.Ignore(e => e.VagasRestantes);
        });

        // ── Alocacao ─────────────────────────────────
        modelBuilder.Entity<Alocacao>(entity =>
        {
            entity.ToTable("Alocacoes");
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Passageiro)
                  .WithMany()
                  .HasForeignKey(e => e.PassageiroId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Veiculo)
                  .WithMany()
                  .HasForeignKey(e => e.VeiculoId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.VeiculoId, e.PassageiroId }).IsUnique();
        });
    }
}
