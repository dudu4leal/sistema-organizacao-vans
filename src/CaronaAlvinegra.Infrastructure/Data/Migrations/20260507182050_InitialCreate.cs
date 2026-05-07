using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaronaAlvinegra.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jogos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Adversario = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Data = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Local = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jogos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rotas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LocalEmbarque = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rotas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Grupos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    RotaId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grupos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grupos_Rotas_RotaId",
                        column: x => x.RotaId,
                        principalTable: "Rotas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Veiculos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RotaId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Fechado = table.Column<bool>(type: "INTEGER", nullable: false),
                    Classificacao = table.Column<int>(type: "INTEGER", nullable: false),
                    Ordem = table.Column<int>(type: "INTEGER", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veiculos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Veiculos_Rotas_RotaId",
                        column: x => x.RotaId,
                        principalTable: "Rotas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Telefone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    RotaPreferencialId = table.Column<Guid>(type: "TEXT", nullable: true),
                    GrupoId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Grupos_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Usuarios_Rotas_RotaPreferencialId",
                        column: x => x.RotaPreferencialId,
                        principalTable: "Rotas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Presencas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "TEXT", nullable: false),
                    JogoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RotaEfetivaId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ConfirmadoEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Presencas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Presencas_Jogos_JogoId",
                        column: x => x.JogoId,
                        principalTable: "Jogos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Presencas_Rotas_RotaEfetivaId",
                        column: x => x.RotaEfetivaId,
                        principalTable: "Rotas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Presencas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Passageiros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PresencaId = table.Column<Guid>(type: "TEXT", nullable: false),
                    JogoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RotaId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passageiros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Passageiros_Jogos_JogoId",
                        column: x => x.JogoId,
                        principalTable: "Jogos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Passageiros_Presencas_PresencaId",
                        column: x => x.PresencaId,
                        principalTable: "Presencas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Passageiros_Rotas_RotaId",
                        column: x => x.RotaId,
                        principalTable: "Rotas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Passageiros_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alocacoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    VeiculoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PassageiroId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsLider = table.Column<bool>(type: "INTEGER", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alocacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alocacoes_Passageiros_PassageiroId",
                        column: x => x.PassageiroId,
                        principalTable: "Passageiros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alocacoes_Veiculos_VeiculoId",
                        column: x => x.VeiculoId,
                        principalTable: "Veiculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alocacoes_PassageiroId",
                table: "Alocacoes",
                column: "PassageiroId");

            migrationBuilder.CreateIndex(
                name: "IX_Alocacoes_VeiculoId_PassageiroId",
                table: "Alocacoes",
                columns: new[] { "VeiculoId", "PassageiroId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Grupos_RotaId",
                table: "Grupos",
                column: "RotaId");

            migrationBuilder.CreateIndex(
                name: "IX_Passageiros_JogoId",
                table: "Passageiros",
                column: "JogoId");

            migrationBuilder.CreateIndex(
                name: "IX_Passageiros_PresencaId",
                table: "Passageiros",
                column: "PresencaId");

            migrationBuilder.CreateIndex(
                name: "IX_Passageiros_RotaId",
                table: "Passageiros",
                column: "RotaId");

            migrationBuilder.CreateIndex(
                name: "IX_Passageiros_UsuarioId",
                table: "Passageiros",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Presencas_JogoId",
                table: "Presencas",
                column: "JogoId");

            migrationBuilder.CreateIndex(
                name: "IX_Presencas_RotaEfetivaId",
                table: "Presencas",
                column: "RotaEfetivaId");

            migrationBuilder.CreateIndex(
                name: "IX_Presencas_UsuarioId_JogoId",
                table: "Presencas",
                columns: new[] { "UsuarioId", "JogoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_GrupoId",
                table: "Usuarios",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RotaPreferencialId",
                table: "Usuarios",
                column: "RotaPreferencialId");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_RotaId",
                table: "Veiculos",
                column: "RotaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alocacoes");

            migrationBuilder.DropTable(
                name: "Passageiros");

            migrationBuilder.DropTable(
                name: "Veiculos");

            migrationBuilder.DropTable(
                name: "Presencas");

            migrationBuilder.DropTable(
                name: "Jogos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Grupos");

            migrationBuilder.DropTable(
                name: "Rotas");
        }
    }
}
