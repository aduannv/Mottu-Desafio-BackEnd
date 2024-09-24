using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mottu.App.Migrations
{
    public partial class AddDelivererEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Deliverers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Identificador = table.Column<string>(type: "text", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Cnpj = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NumeroCnh = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TipoCnh = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    ImagemCnh = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliverers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deliverers_Cnpj",
                table: "Deliverers",
                column: "Cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deliverers_Identificador",
                table: "Deliverers",
                column: "Identificador",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deliverers_NumeroCnh",
                table: "Deliverers",
                column: "NumeroCnh",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deliverers");
        }
    }
}
