using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mottu.App.Migrations
{
    public partial class RemoveImageCnh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagemCnh",
                table: "Deliverers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagemCnh",
                table: "Deliverers",
                type: "text",
                nullable: true);
        }
    }
}
