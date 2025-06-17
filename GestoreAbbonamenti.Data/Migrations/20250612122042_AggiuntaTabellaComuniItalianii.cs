using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestoreAbbonamenti.Data.Migrations
{
    /// <inheritdoc />
    public partial class AggiuntaTabellaComuniItalianii : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComuniItaliani",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodiceBelfiore = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comune = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComuniItaliani", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComuniItaliani");
        }
    }
}
