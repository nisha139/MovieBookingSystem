using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieBooking.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Screens_Theater_Id",
                table: "Screens");

            migrationBuilder.AddColumn<Guid>(
                name: "TheaterId",
                table: "Screens",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Screens_TheaterId",
                table: "Screens",
                column: "TheaterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Screens_Theater_TheaterId",
                table: "Screens",
                column: "TheaterId",
                principalTable: "Theater",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Screens_Theater_TheaterId",
                table: "Screens");

            migrationBuilder.DropIndex(
                name: "IX_Screens_TheaterId",
                table: "Screens");

            migrationBuilder.DropColumn(
                name: "TheaterId",
                table: "Screens");

            migrationBuilder.AddForeignKey(
                name: "FK_Screens_Theater_Id",
                table: "Screens",
                column: "Id",
                principalTable: "Theater",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
