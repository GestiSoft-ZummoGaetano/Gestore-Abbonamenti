using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestoreAbbonamenti.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateScuoleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Indirizzo",
                table: "Scuole",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Indirizzo",
                table: "Scuole");
        }
    }
}
