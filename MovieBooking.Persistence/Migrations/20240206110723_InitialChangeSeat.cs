using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieBooking.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialChangeSeat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_seats_Screens_Id",
                table: "seats");

            migrationBuilder.AddColumn<Guid>(
                name: "ScreenId",
                table: "seats",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_seats_ScreenId",
                table: "seats",
                column: "ScreenId");

            migrationBuilder.AddForeignKey(
                name: "FK_seats_Screens_ScreenId",
                table: "seats",
                column: "ScreenId",
                principalTable: "Screens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_seats_Screens_ScreenId",
                table: "seats");

            migrationBuilder.DropIndex(
                name: "IX_seats_ScreenId",
                table: "seats");

            migrationBuilder.DropColumn(
                name: "ScreenId",
                table: "seats");

            migrationBuilder.AddForeignKey(
                name: "FK_seats_Screens_Id",
                table: "seats",
                column: "Id",
                principalTable: "Screens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
