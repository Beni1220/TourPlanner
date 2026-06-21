using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddedTourLogsToToursNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourCoordinate_Tours_TourId",
                table: "TourCoordinate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TourCoordinate",
                table: "TourCoordinate");

            migrationBuilder.RenameTable(
                name: "TourCoordinate",
                newName: "TourCoordinates");

            migrationBuilder.RenameIndex(
                name: "IX_TourCoordinate_TourId",
                table: "TourCoordinates",
                newName: "IX_TourCoordinates_TourId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TourCoordinates",
                table: "TourCoordinates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TourCoordinates_Tours_TourId",
                table: "TourCoordinates",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourCoordinates_Tours_TourId",
                table: "TourCoordinates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TourCoordinates",
                table: "TourCoordinates");

            migrationBuilder.RenameTable(
                name: "TourCoordinates",
                newName: "TourCoordinate");

            migrationBuilder.RenameIndex(
                name: "IX_TourCoordinates_TourId",
                table: "TourCoordinate",
                newName: "IX_TourCoordinate_TourId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TourCoordinate",
                table: "TourCoordinate",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TourCoordinate_Tours_TourId",
                table: "TourCoordinate",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
