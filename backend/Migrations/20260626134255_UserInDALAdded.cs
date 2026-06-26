using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class UserInDALAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Tours",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "TourLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tours_UserId",
                table: "Tours",
                column: "UserId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Tours_Users_UserId",
                table: "Tours",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourLogs_Users_userId",
                table: "TourLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Tours_Users_UserId",
                table: "Tours");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Tours_UserId",
                table: "Tours");

            migrationBuilder.DropIndex(
                name: "IX_TourLogs_userId",
                table: "TourLogs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "TourLogs");
        }
    }
}
