using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaronaAlvinegra.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeRotaNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passageiros_Rotas_RotaId",
                table: "Passageiros");

            migrationBuilder.DropForeignKey(
                name: "FK_Presencas_Rotas_RotaEfetivaId",
                table: "Presencas");

            migrationBuilder.AlterColumn<Guid>(
                name: "RotaEfetivaId",
                table: "Presencas",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<Guid>(
                name: "RotaId",
                table: "Passageiros",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Passageiros_Rotas_RotaId",
                table: "Passageiros",
                column: "RotaId",
                principalTable: "Rotas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Presencas_Rotas_RotaEfetivaId",
                table: "Presencas",
                column: "RotaEfetivaId",
                principalTable: "Rotas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passageiros_Rotas_RotaId",
                table: "Passageiros");

            migrationBuilder.DropForeignKey(
                name: "FK_Presencas_Rotas_RotaEfetivaId",
                table: "Presencas");

            migrationBuilder.AlterColumn<Guid>(
                name: "RotaEfetivaId",
                table: "Presencas",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "RotaId",
                table: "Passageiros",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Passageiros_Rotas_RotaId",
                table: "Passageiros",
                column: "RotaId",
                principalTable: "Rotas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Presencas_Rotas_RotaEfetivaId",
                table: "Presencas",
                column: "RotaEfetivaId",
                principalTable: "Rotas",
                principalColumn: "Id");
        }
    }
}
