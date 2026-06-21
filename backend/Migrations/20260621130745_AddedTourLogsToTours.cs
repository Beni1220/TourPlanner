using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddedTourLogsToTours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TourCoordinate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TourId = table.Column<int>(type: "integer", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Sequence = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourCoordinate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourCoordinate_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TourLogs_TourId",
                table: "TourLogs",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_TourCoordinate_TourId",
                table: "TourCoordinate",
                column: "TourId");

            migrationBuilder.AddForeignKey(
                name: "FK_TourLogs_Tours_TourId",
                table: "TourLogs",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourLogs_Tours_TourId",
                table: "TourLogs");

            migrationBuilder.DropTable(
                name: "TourCoordinate");

            migrationBuilder.DropIndex(
                name: "IX_TourLogs_TourId",
                table: "TourLogs");
        }
    }
}
