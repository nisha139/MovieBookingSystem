using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieBooking.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CHangeMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "movieBookings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "movieBookings");
        }
    }
}
