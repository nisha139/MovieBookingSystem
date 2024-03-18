using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieBooking.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedTableInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BookingMainId",
                table: "movieBookings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "theaterMains",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoOfScreen = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_theaterMains", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "screenMains",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TheaterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_screenMains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_screenMains_theaterMains_TheaterId",
                        column: x => x.TheaterId,
                        principalTable: "theaterMains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "seatMains",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScreenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Row = table.Column<int>(type: "int", nullable: false),
                    Column = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seatMains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_seatMains_screenMains_ScreenId",
                        column: x => x.ScreenId,
                        principalTable: "screenMains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "showtimeMains",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScreenID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_showtimeMains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_showtimeMains_movieBookings_MovieId",
                        column: x => x.MovieId,
                        principalTable: "movieBookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_showtimeMains_screenMains_ScreenID",
                        column: x => x.ScreenID,
                        principalTable: "screenMains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bookingMains",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShowtimeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SeatMainId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookingMains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bookingMains_seatMains_SeatMainId",
                        column: x => x.SeatMainId,
                        principalTable: "seatMains",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_bookingMains_showtimeMains_ShowtimeID",
                        column: x => x.ShowtimeID,
                        principalTable: "showtimeMains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_movieBookings_BookingMainId",
                table: "movieBookings",
                column: "BookingMainId");

            migrationBuilder.CreateIndex(
                name: "IX_bookingMains_SeatMainId",
                table: "bookingMains",
                column: "SeatMainId");

            migrationBuilder.CreateIndex(
                name: "IX_bookingMains_ShowtimeID",
                table: "bookingMains",
                column: "ShowtimeID");

            migrationBuilder.CreateIndex(
                name: "IX_screenMains_TheaterId",
                table: "screenMains",
                column: "TheaterId");

            migrationBuilder.CreateIndex(
                name: "IX_seatMains_ScreenId",
                table: "seatMains",
                column: "ScreenId");

            migrationBuilder.CreateIndex(
                name: "IX_showtimeMains_MovieId",
                table: "showtimeMains",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_showtimeMains_ScreenID",
                table: "showtimeMains",
                column: "ScreenID");

            migrationBuilder.AddForeignKey(
                name: "FK_movieBookings_bookingMains_BookingMainId",
                table: "movieBookings",
                column: "BookingMainId",
                principalTable: "bookingMains",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movieBookings_bookingMains_BookingMainId",
                table: "movieBookings");

            migrationBuilder.DropTable(
                name: "bookingMains");

            migrationBuilder.DropTable(
                name: "seatMains");

            migrationBuilder.DropTable(
                name: "showtimeMains");

            migrationBuilder.DropTable(
                name: "screenMains");

            migrationBuilder.DropTable(
                name: "theaterMains");

            migrationBuilder.DropIndex(
                name: "IX_movieBookings_BookingMainId",
                table: "movieBookings");

            migrationBuilder.DropColumn(
                name: "BookingMainId",
                table: "movieBookings");
        }
    }
}
