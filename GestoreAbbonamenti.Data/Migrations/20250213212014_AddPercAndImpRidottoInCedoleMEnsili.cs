using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestoreAbbonamenti.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPercAndImpRidottoInCedoleMEnsili : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ImportoRidotto",
                table: "CedoleMensili",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PercentualeRiduzione",
                table: "CedoleMensili",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImportoRidotto",
                table: "CedoleMensili");

            migrationBuilder.DropColumn(
                name: "PercentualeRiduzione",
                table: "CedoleMensili");
        }
    }
}
