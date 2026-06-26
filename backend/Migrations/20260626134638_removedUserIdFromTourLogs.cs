using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class removedUserIdFromTourLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourLogs_Users_userId",
                table: "TourLogs");

            migrationBuilder.DropIndex(
                name: "IX_TourLogs_userId",
                table: "TourLogs");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "TourLogs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "TourLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TourLogs_userId",
                table: "TourLogs",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_TourLogs_Users_userId",
                table: "TourLogs",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
