using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieBooking.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class BookingChanages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Movies_MovieId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_seats_SeatId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_showtimes_ShowtimeID",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Booking_BookingId",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Booking_Id",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Booking",
                table: "Booking");

            migrationBuilder.RenameTable(
                name: "Booking",
                newName: "Bookings");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_ShowtimeID",
                table: "Bookings",
                newName: "IX_Bookings_ShowtimeID");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_SeatId",
                table: "Bookings",
                newName: "IX_Bookings_SeatId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_MovieId",
                table: "Bookings",
                newName: "IX_Bookings_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Movies_MovieId",
                table: "Bookings",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_seats_SeatId",
                table: "Bookings",
                column: "SeatId",
                principalTable: "seats",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_showtimes_ShowtimeID",
                table: "Bookings",
                column: "ShowtimeID",
                principalTable: "showtimes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Bookings_BookingId",
                table: "Movies",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Bookings_Id",
                table: "Transaction",
                column: "Id",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Movies_MovieId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_seats_SeatId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_showtimes_ShowtimeID",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Bookings_BookingId",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Bookings_Id",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

            migrationBuilder.RenameTable(
                name: "Bookings",
                newName: "Booking");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_ShowtimeID",
                table: "Booking",
                newName: "IX_Booking_ShowtimeID");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_SeatId",
                table: "Booking",
                newName: "IX_Booking_SeatId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_MovieId",
                table: "Booking",
                newName: "IX_Booking_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Booking",
                table: "Booking",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Movies_MovieId",
                table: "Booking",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_seats_SeatId",
                table: "Booking",
                column: "SeatId",
                principalTable: "seats",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_showtimes_ShowtimeID",
                table: "Booking",
                column: "ShowtimeID",
                principalTable: "showtimes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Booking_BookingId",
                table: "Movies",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Booking_Id",
                table: "Transaction",
                column: "Id",
                principalTable: "Booking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
