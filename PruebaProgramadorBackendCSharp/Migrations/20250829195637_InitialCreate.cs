using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PruebaProgramadorBackendCSharp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MarcasAutos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarcasAutos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "MarcasAutos",
                columns: new[] { "Id", "Descripcion", "FechaCreacion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Marca japonesa", new DateTime(1937, 8, 28, 0, 0, 0, 0, DateTimeKind.Utc), "Toyota" },
                    { 2, "Marca estadounidense", new DateTime(1903, 6, 16, 0, 0, 0, 0, DateTimeKind.Utc), "Ford" },
                    { 3, "Marca alemana", new DateTime(1916, 3, 7, 0, 0, 0, 0, DateTimeKind.Utc), "BMW" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarcasAutos");
        }
    }
}
