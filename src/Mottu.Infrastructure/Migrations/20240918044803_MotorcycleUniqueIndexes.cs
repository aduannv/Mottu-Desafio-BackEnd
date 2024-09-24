using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mottu.App.Migrations
{
    public partial class MotorcycleUniqueIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Motorcycles_Identificador",
                table: "Motorcycles",
                column: "Identificador",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Motorcycles_Placa",
                table: "Motorcycles",
                column: "Placa",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Motorcycles_Identificador",
                table: "Motorcycles");

            migrationBuilder.DropIndex(
                name: "IX_Motorcycles_Placa",
                table: "Motorcycles");
        }
    }
}
