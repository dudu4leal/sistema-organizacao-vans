using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaronaAlvinegra.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLiderOverridesToJogo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LiderOverrides",
                table: "Jogos",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LiderOverrides",
                table: "Jogos");
        }
    }
}
