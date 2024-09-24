using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mottu.App.Migrations
{
    public partial class AddBrandNewMotorcyclesEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BrandNewMotorcycles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Identificador = table.Column<string>(type: "text", nullable: false),
                    Ano = table.Column<int>(type: "integer", nullable: false),
                    Modelo = table.Column<string>(type: "text", nullable: false),
                    Placa = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandNewMotorcycles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrandNewMotorcycles_Identificador",
                table: "BrandNewMotorcycles",
                column: "Identificador",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BrandNewMotorcycles_Placa",
                table: "BrandNewMotorcycles",
                column: "Placa",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrandNewMotorcycles");
        }
    }
}
