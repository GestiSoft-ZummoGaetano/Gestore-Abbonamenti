using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestoreAbbonamenti.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameTablecomune : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Comune",
                table: "Comune");

            migrationBuilder.RenameTable(
                name: "Comune",
                newName: "Comuni");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comuni",
                table: "Comuni",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Comuni",
                table: "Comuni");

            migrationBuilder.RenameTable(
                name: "Comuni",
                newName: "Comune");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comune",
                table: "Comune",
                column: "Id");
        }
    }
}
