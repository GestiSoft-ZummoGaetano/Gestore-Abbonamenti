using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestoreAbbonamenti.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPrezzoCedolaInCedoleMEnsili : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IstitutoScolastico",
                table: "Figli");

            migrationBuilder.AddColumn<decimal>(
                name: "PrezzoCedolaSettimanale",
                table: "CedoleMensili",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrezzoCedolaSettimanale",
                table: "CedoleMensili");

            migrationBuilder.AddColumn<string>(
                name: "IstitutoScolastico",
                table: "Figli",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
