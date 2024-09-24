using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mottu.App.Migrations
{
    public partial class ForeingKeysAndChangeIdKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Motorcycles",
                table: "Motorcycles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Deliverers",
                table: "Deliverers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BrandNewMotorcycles",
                table: "BrandNewMotorcycles");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Motorcycles");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Deliverers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BrandNewMotorcycles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Motorcycles",
                table: "Motorcycles",
                column: "Identificador");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Deliverers",
                table: "Deliverers",
                column: "Identificador");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BrandNewMotorcycles",
                table: "BrandNewMotorcycles",
                column: "Identificador");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_EntregadorId",
                table: "Rentals",
                column: "EntregadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_MotoId",
                table: "Rentals",
                column: "MotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Deliverers_EntregadorId",
                table: "Rentals",
                column: "EntregadorId",
                principalTable: "Deliverers",
                principalColumn: "Identificador",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Motorcycles_MotoId",
                table: "Rentals",
                column: "MotoId",
                principalTable: "Motorcycles",
                principalColumn: "Identificador",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Deliverers_EntregadorId",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Motorcycles_MotoId",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_Rentals_EntregadorId",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_Rentals_MotoId",
                table: "Rentals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Motorcycles",
                table: "Motorcycles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Deliverers",
                table: "Deliverers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BrandNewMotorcycles",
                table: "BrandNewMotorcycles");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Motorcycles",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Deliverers",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "BrandNewMotorcycles",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Motorcycles",
                table: "Motorcycles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Deliverers",
                table: "Deliverers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BrandNewMotorcycles",
                table: "BrandNewMotorcycles",
                column: "Id");
        }
    }
}
