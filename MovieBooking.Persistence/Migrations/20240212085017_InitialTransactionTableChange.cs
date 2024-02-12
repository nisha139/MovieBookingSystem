using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieBooking.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialTransactionTableChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Bookings_Id",
                table: "Transaction");

            migrationBuilder.AddColumn<Guid>(
                name: "BookingId",
                table: "Transaction",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_BookingId",
                table: "Transaction",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Bookings_BookingId",
                table: "Transaction",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Bookings_BookingId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_BookingId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "Transaction");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Bookings_Id",
                table: "Transaction",
                column: "Id",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
