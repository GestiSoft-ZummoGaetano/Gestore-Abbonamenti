using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestoreAbbonamenti.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPrezzoCedolaInCedoleMEnsiliCorretto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrezzoCedolaSettimanale",
                table: "CedoleMensili");

            migrationBuilder.AddColumn<decimal>(
                name: "PrezzoCedolaSettimanale",
                table: "Scuole",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrezzoCedolaSettimanale",
                table: "Scuole");

            migrationBuilder.AddColumn<decimal>(
                name: "PrezzoCedolaSettimanale",
                table: "CedoleMensili",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
