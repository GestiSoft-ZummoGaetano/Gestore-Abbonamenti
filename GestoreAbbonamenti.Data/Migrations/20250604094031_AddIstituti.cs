using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestoreAbbonamenti.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIstituti : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Istituti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Percorso = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Settore = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Codiceindirizzo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zero = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Indirizzo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descrizione_Diploma = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    N_lingue_Straniere = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FLG_MNS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NUM_ORD_IND_ESA = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Istituti", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Istituti");
        }
    }
}
