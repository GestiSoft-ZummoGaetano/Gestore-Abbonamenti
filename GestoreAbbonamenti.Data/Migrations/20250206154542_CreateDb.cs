using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestoreAbbonamenti.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genitori",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cognome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataNascita = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Sesso = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LuogoNascita = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodiceFiscale = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Indirizzo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NCivico = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cap = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Localita = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Iban = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genitori", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Scuole",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Citta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Istituto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodiceIstituto = table.Column<int>(type: "int", nullable: true),
                    DistanzaDalComune = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scuole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Figli",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GenitoreId = table.Column<long>(type: "bigint", nullable: false),
                    IdScuola = table.Column<long>(type: "bigint", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cognome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataNascita = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Sesso = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LuogoNascita = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Indirizzo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IstitutoScolastico = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Frequenza = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Figli", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Figli_Genitori_GenitoreId",
                        column: x => x.GenitoreId,
                        principalTable: "Genitori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Figli_Scuole_IdScuola",
                        column: x => x.IdScuola,
                        principalTable: "Scuole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CedoleMensili",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FiglioId = table.Column<long>(type: "bigint", nullable: false),
                    Mese = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NCedola = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCedola = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Importo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Anno = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CedoleMensili", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CedoleMensili_Figli_FiglioId",
                        column: x => x.FiglioId,
                        principalTable: "Figli",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CedoleMensili_FiglioId",
                table: "CedoleMensili",
                column: "FiglioId");

            migrationBuilder.CreateIndex(
                name: "IX_Figli_GenitoreId",
                table: "Figli",
                column: "GenitoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Figli_IdScuola",
                table: "Figli",
                column: "IdScuola");

            migrationBuilder.CreateIndex(
                name: "IX_Genitori_CodiceFiscale",
                table: "Genitori",
                column: "CodiceFiscale",
                unique: true,
                filter: "[CodiceFiscale] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CedoleMensili");

            migrationBuilder.DropTable(
                name: "Figli");

            migrationBuilder.DropTable(
                name: "Genitori");

            migrationBuilder.DropTable(
                name: "Scuole");
        }
    }
}
