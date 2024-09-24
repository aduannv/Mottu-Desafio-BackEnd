using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mottu.App.Migrations
{
    /// <inheritdoc />
    public partial class FixCheckFieldDeprecated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // just to update migrations in database
            // fixed a deprecated function in AppDbContext
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
