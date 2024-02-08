using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieBooking.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialChangeShowtime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_showtimes_Movies_Id",
                table: "showtimes");

            migrationBuilder.AddColumn<Guid>(
                name: "MovieId",
                table: "showtimes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_showtimes_MovieId",
                table: "showtimes",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_showtimes_Movies_MovieId",
                table: "showtimes",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_showtimes_Movies_MovieId",
                table: "showtimes");

            migrationBuilder.DropIndex(
                name: "IX_showtimes_MovieId",
                table: "showtimes");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "showtimes");

            migrationBuilder.AddForeignKey(
                name: "FK_showtimes_Movies_Id",
                table: "showtimes",
                column: "Id",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
